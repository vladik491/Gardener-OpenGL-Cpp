using OpenTK.Graphics.OpenGL;

namespace Gardener.gameClasses.Rendering
{
    // Отвечает за рендеринг карты, включая воду, траву, тропы и препятствия
    public class MapRenderer
    {
        private readonly TextureRegistry _textureRegistry; // Реестр текстур для доступа к изображениям
        private readonly Map _map; // Карта игры
        private (float texLeft, float texRight, float texTop, float texBottom)[] _grassSprites; // Спрайты травы
        private (float texLeft, float texRight, float texTop, float texBottom)[] _tropaSprites; // Спрайты тропы

        // Смещения для препятствий (кусты, бочки, бревна, пруды)
        private readonly float _onlyBushOffsetX = 0.12f * TileSize;
        private readonly float _onlyBushOffsetY = 0.32f * TileSize;
        private readonly float _onlyBarrelOffsetX = 0.32f * TileSize;
        private readonly float _onlyBarrelOffsetY = 0.24f * TileSize;
        private readonly float _onlyBrevnoOffsetX = 0.07f * TileSize;
        private readonly float _onlyBrevnoOffsetY = 0.57f * TileSize;
        private readonly float _onlyPondOffsetX = 0.11f * TileSize;
        private readonly float _onlyPondOffsetY = 0.12f * TileSize;

        internal const int TileSize = GameRenderer.TileSize; // Размер тайла

        // Константы для типов тайлов
        public const int WATER_TILE = -1; // Тайл воды
        public const int BUSH_TOP_LEFT = 33; // Тайл куста
        public const int BREVNO_TOP_LEFT = 34; // Тайл бревна
        public const int BARREL_TOP_LEFT = 35; // Тайл бочки
        public const int POND_TOP_LEFT = 36; // Тайл пруда

        // Конструктор: инициализирует рендерер с реестром текстур и картой
        public MapRenderer(TextureRegistry textureRegistry, Map map)
        {
            _textureRegistry = textureRegistry ?? throw new ArgumentNullException(nameof(textureRegistry));
            _map = map ?? throw new ArgumentNullException(nameof(map));
            InitializeSprites();
        }

        // Инициализирует спрайты для травы и тропы
        private void InitializeSprites()
        {
            _grassSprites = new (float, float, float, float)[21];
            int spriteSize = 16, grassTextureWidth = 112, grassTextureHeight = 48;
            for (int i = 0; i < _grassSprites.Length; i++)
            {
                int sx = i % 7, sy = i / 7;
                _grassSprites[i] = (
                    texLeft: (sx * spriteSize) / (float)grassTextureWidth,
                    texRight: ((sx + 1) * spriteSize) / (float)grassTextureWidth,
                    texTop: (sy * spriteSize) / (float)grassTextureHeight,
                    texBottom: ((sy + 1) * spriteSize) / (float)grassTextureHeight
                );
            }

            _tropaSprites = new (float, float, float, float)[12];
            int tropaTextureWidth = 48, tropaTextureHeight = 64;
            int baseSpriteSize = 16;
            for (int i = 0; i < _tropaSprites.Length; i++)
            {
                int sx = i % 3, sy = i / 3;
                _tropaSprites[i] = (
                    texLeft: (sx * baseSpriteSize) / (float)tropaTextureWidth,
                    texRight: ((sx + 1) * baseSpriteSize) / (float)tropaTextureWidth,
                    texTop: (sy * baseSpriteSize) / (float)tropaTextureHeight,
                    texBottom: ((sy + 1) * baseSpriteSize) / (float)tropaTextureHeight
                );
            }
        }

        /// <summary>
        /// Рендерит карту в заданной области экрана.
        /// </summary>
        /// <param name="width">Ширина экрана</param>
        /// <param name="height">Высота экрана</param>
        /// <param name="cameraX">Позиция камеры по X</param>
        /// <param name="cameraY">Позиция камеры по Y</param>
        /// <param name="playerId">ID игрока</param>
        public void Render(int width, int height, float cameraX, float cameraY, int playerId)
        {
            GL.Enable(EnableCap.Texture2D); // Включаем поддержку текстур
            GL.Enable(EnableCap.Blend); // Включаем прозрачность
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha); // Настраиваем смешивание цветов

            var (startX, endX, startY, endY) = CalculateRenderBounds(width, height, cameraX, cameraY);
            var (grassStartX, grassEndX, grassStartY, grassEndY) = CalculateGrassBounds();

            RenderWater(startX, endX, startY, endY); // Отрисовка воды
            RenderGrass(startX, endX, startY, endY, grassStartX, grassEndX, grassStartY, grassEndY, playerId); // Отрисовка травы
            RenderTropa(startX, endX, startY, endY, grassStartX, grassEndX, grassStartY, grassEndY, playerId); // Отрисовка троп

            // Рендеринг препятствий
            RenderBush(startX, endX, startY, endY, grassStartX, grassEndX, grassStartY, grassEndY, playerId);
            RenderBrevno(startX, endX, startY, endY, grassStartX, grassEndX, grassStartY, grassEndY, playerId);
            RenderBarrel(startX, endX, startY, endY, grassStartX, grassEndX, grassStartY, grassEndY, playerId);
            RenderPond(startX, endX, startY, endY, grassStartX, grassEndX, grassStartY, grassEndY, playerId);

            // Рендеринг отдельных элементов препятствий
            RenderOnlyBush(startX, endX, startY, endY, grassStartX, grassEndX, grassStartY, grassEndY, playerId);
            RenderOnlyBrevno(startX, endX, startY, endY, grassStartX, grassEndX, grassStartY, grassEndY, playerId);
            RenderOnlyBarrel(startX, endX, startY, endY, grassStartX, grassEndX, grassStartY, grassEndY, playerId);
            RenderOnlyPond(startX, endX, startY, endY, grassStartX, grassEndX, grassStartY, grassEndY, playerId);

            RenderFence(grassStartX, grassStartY); // Отрисовка забора

            GL.Disable(EnableCap.Blend); // Отключаем прозрачность
            GL.Disable(EnableCap.Texture2D); // Отключаем текстуры
        }

        // Вычисляет границы рендеринга на основе камеры и размеров экрана
        private (int startX, int endX, int startY, int endY) CalculateRenderBounds(int width, int height, float cameraX, float cameraY)
        {
            int tilesX = _map.Width / TileSize;
            int tilesY = _map.Height / TileSize;

            int startX = Math.Max(0, (int)Math.Floor(cameraX / TileSize));
            int endX = Math.Min(tilesX, (int)Math.Ceiling((cameraX + width) / TileSize));
            int startY = Math.Max(0, (int)Math.Floor(cameraY / TileSize));
            int endY = Math.Min(tilesY, (int)Math.Ceiling((cameraY + height) / TileSize));

            return (startX, endX, startY, endY);
        }

        // Вычисляет границы травяной области
        private (int grassStartX, int grassEndX, int grassStartY, int grassEndY) CalculateGrassBounds()
        {
            int tilesX = _map.Width / TileSize;
            int tilesY = _map.Height / TileSize;
            const int totalGrassTilesX = 11;
            const int totalGrassTilesY = 17;

            int grassStartX = (tilesX - totalGrassTilesX) / 2;
            int grassEndX = grassStartX + totalGrassTilesX;
            int grassStartY = (tilesY - totalGrassTilesY) / 2;
            int grassEndY = grassStartY + totalGrassTilesY;

            return (grassStartX, grassEndX, grassStartY, grassEndY);
        }

        // Рендерит воду на карте
        private void RenderWater(int startX, int endX, int startY, int endY)
        {
            GL.BindTexture(TextureTarget.Texture2D, _textureRegistry.WaterTextureId);
            GL.Begin(PrimitiveType.Quads);
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    float posX = x * TileSize;
                    float posY = y * TileSize;

                    bool frame = ((x + y) % 2) == 0; // Чередуем текстуру для эффекта анимации
                    float tl = frame ? 0f : 0.5f;
                    float tr = frame ? 0.5f : 1f;

                    GL.TexCoord2(tl, 0); GL.Vertex2(posX, posY);
                    GL.TexCoord2(tr, 0); GL.Vertex2(posX + TileSize, posY);
                    GL.TexCoord2(tr, 1); GL.Vertex2(posX + TileSize, posY + TileSize);
                    GL.TexCoord2(tl, 1); GL.Vertex2(posX, posY + TileSize);
                }
            }
            GL.End();
        }

        // Рендерит траву на карте
        private void RenderGrass(int startX, int endX, int startY, int endY, int grassStartX, int grassEndX, int grassStartY, int grassEndY, int playerId)
        {
            GL.BindTexture(TextureTarget.Texture2D, _textureRegistry.GrassTextureId);
            GL.Begin(PrimitiveType.Quads);
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    if (x < grassStartX || x >= grassEndX || y < grassStartY || y >= grassEndY)
                        continue;

                    int mapX = x - grassStartX;
                    int mapY = y - grassStartY;
                    int spriteIndex = GetTileIndex(mapX, mapY, playerId);

                    if (spriteIndex == WATER_TILE || IsObstacleBaseTile(mapX, mapY, playerId))
                        continue;

                    if (spriteIndex < 0 || spriteIndex >= _grassSprites.Length)
                        spriteIndex = 4; // Значение по умолчанию для некорректных индексов

                    var s = _grassSprites[spriteIndex];
                    float posX = x * TileSize;
                    float posY = y * TileSize;

                    GL.TexCoord2(s.texLeft, s.texTop); GL.Vertex2(posX, posY);
                    GL.TexCoord2(s.texRight, s.texTop); GL.Vertex2(posX + TileSize, posY);
                    GL.TexCoord2(s.texRight, s.texBottom); GL.Vertex2(posX + TileSize, posY + TileSize);
                    GL.TexCoord2(s.texLeft, s.texBottom); GL.Vertex2(posX, posY + TileSize);
                }
            }
            GL.End();
        }

        // Рендерит тропы на карте
        private void RenderTropa(int startX, int endX, int startY, int endY, int grassStartX, int grassEndX, int grassStartY, int grassEndY, int playerId)
        {
            GL.BindTexture(TextureTarget.Texture2D, _textureRegistry.TropaTextureId);
            GL.Begin(PrimitiveType.Quads);
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    if (x < grassStartX || x >= grassEndX || y < grassStartY || y >= grassEndY)
                        continue;

                    int mapX = x - grassStartX;
                    int mapY = y - grassStartY;
                    int spriteIndex = GetTileIndex(mapX, mapY, playerId);

                    if (spriteIndex < 21)
                        continue;

                    if (IsObstacleBaseTile(mapX, mapY, playerId))
                        continue;

                    int tropaIndex = spriteIndex - 21;
                    if (tropaIndex < 0 || tropaIndex >= _tropaSprites.Length)
                        continue;

                    var s = _tropaSprites[tropaIndex];
                    float posX = x * TileSize;
                    float posY = y * TileSize;

                    GL.TexCoord2(s.texLeft, s.texTop); GL.Vertex2(posX, posY);
                    GL.TexCoord2(s.texRight, s.texTop); GL.Vertex2(posX + TileSize, posY);
                    GL.TexCoord2(s.texRight, s.texBottom); GL.Vertex2(posX + TileSize, posY + TileSize);
                    GL.TexCoord2(s.texLeft, s.texBottom); GL.Vertex2(posX, posY + TileSize);
                }
            }
            GL.End();
        }

        // Базовый метод для рендеринга препятствий (кусты, бревна и т.д.)
        private void RenderObstacleBase(int textureId, int checkTileIndex, int startX, int endX, int startY, int endY, int grassStartX, int grassEndX, int grassStartY, int grassEndY, int playerId, float visualWidthTiles = 2, float visualHeightTiles = 2)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.Begin(PrimitiveType.Quads);
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    if (x < grassStartX || x >= grassEndX || y < grassStartY || y >= grassEndY)
                        continue;

                    int mapX = x - grassStartX;
                    int mapY = y - grassStartY;
                    int spriteIndex = GetTileIndex(mapX, mapY, playerId);

                    if (spriteIndex != checkTileIndex)
                        continue;

                    float obstaclePosX = x * TileSize;
                    float obstaclePosY = y * TileSize;
                    float obstacleWidth = TileSize * visualWidthTiles;
                    float obstacleHeight = TileSize * visualHeightTiles;

                    GL.TexCoord2(0f, 0f); GL.Vertex2(obstaclePosX, obstaclePosY);
                    GL.TexCoord2(1f, 0f); GL.Vertex2(obstaclePosX + obstacleWidth, obstaclePosY);
                    GL.TexCoord2(1f, 1f); GL.Vertex2(obstaclePosX + obstacleWidth, obstaclePosY + obstacleHeight);
                    GL.TexCoord2(0f, 1f); GL.Vertex2(obstaclePosX, obstaclePosY + obstacleHeight);
                }
            }
            GL.End();
        }

        // Рендерит кусты
        private void RenderBush(int startX, int endX, int startY, int endY, int grassStartX, int grassEndX, int grassStartY, int grassEndY, int playerId)
        {
            RenderObstacleBase(_textureRegistry.BushTextureId, BUSH_TOP_LEFT, startX, endX, startY, endY, grassStartX, grassEndX, grassStartY, grassEndY, playerId);
        }

        // Рендерит бревна
        private void RenderBrevno(int startX, int endX, int startY, int endY, int grassStartX, int grassEndX, int grassStartY, int grassEndY, int playerId)
        {
            RenderObstacleBase(_textureRegistry.BrevnoTextureId, BREVNO_TOP_LEFT, startX, endX, startY, endY, grassStartX, grassEndX, grassStartY, grassEndY, playerId);
        }

        // Рендерит бочки
        private void RenderBarrel(int startX, int endX, int startY, int endY, int grassStartX, int grassEndX, int grassStartY, int grassEndY, int playerId)
        {
            RenderObstacleBase(_textureRegistry.BarrelTextureId, BARREL_TOP_LEFT, startX, endX, startY, endY, grassStartX, grassEndX, grassStartY, grassEndY, playerId);
        }

        // Рендерит пруды
        private void RenderPond(int startX, int endX, int startY, int endY, int grassStartX, int grassEndX, int grassStartY, int grassEndY, int playerId)
        {
            RenderObstacleBase(_textureRegistry.PondTextureId, POND_TOP_LEFT, startX, endX, startY, endY, grassStartX, grassEndX, grassStartY, grassEndY, playerId);
        }

        // Базовый метод для рендеринга отдельных элементов препятствий
        private void RenderOnlyObstacle(int textureId, int checkTileIndex, float offsetX, float offsetY, float visualWidthPixels, float visualHeightPixels, int startX, int endX, int startY, int endY, int grassStartX, int grassEndX, int grassStartY, int grassEndY, int playerId, float baseWidthTiles = 2, float baseHeightTiles = 2)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.Begin(PrimitiveType.Quads);
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    if (x < grassStartX || x >= grassEndX || y < grassStartY || y >= grassEndY)
                        continue;

                    int mapX = x - grassStartX;
                    int mapY = y - grassStartY;
                    int spriteIndex = GetTileIndex(mapX, mapY, playerId);

                    if (spriteIndex != checkTileIndex)
                        continue;

                    float basePosX = x * TileSize;
                    float basePosY = y * TileSize;

                    float finalPosX = basePosX + offsetX;
                    float finalPosY = basePosY + offsetY;

                    float renderWidth = visualWidthPixels * (TileSize / 16f);
                    float renderHeight = visualHeightPixels * (TileSize / 16f);

                    GL.TexCoord2(0f, 0f); GL.Vertex2(finalPosX, finalPosY);
                    GL.TexCoord2(1f, 0f); GL.Vertex2(finalPosX + renderWidth, finalPosY);
                    GL.TexCoord2(1f, 1f); GL.Vertex2(finalPosX + renderWidth, finalPosY + renderHeight);
                    GL.TexCoord2(0f, 1f); GL.Vertex2(finalPosX, finalPosY + renderHeight);
                }
            }
            GL.End();
        }

        // Рендерит отдельные элементы кустов
        private void RenderOnlyBush(int startX, int endX, int startY, int endY, int grassStartX, int grassEndX, int grassStartY, int grassEndY, int playerId)
        {
            RenderOnlyObstacle(_textureRegistry.OnlyBushTextureId, BUSH_TOP_LEFT, _onlyBushOffsetX, _onlyBushOffsetY, 28, 21, startX, endX, startY, endY, grassStartX, grassEndX, grassStartY, grassEndY, playerId);
        }

        // Рендерит отдельные элементы бревен
        private void RenderOnlyBrevno(int startX, int endX, int startY, int endY, int grassStartX, int grassEndX, int grassStartY, int grassEndY, int playerId)
        {
            RenderOnlyObstacle(_textureRegistry.OnlyBrevnoTextureId, BREVNO_TOP_LEFT, _onlyBrevnoOffsetX, _onlyBrevnoOffsetY, 30, 19, startX, endX, startY, endY, grassStartX, grassEndX, grassStartY, grassEndY, playerId);
        }

        // Рендерит отдельные элементы бочек
        private void RenderOnlyBarrel(int startX, int endX, int startY, int endY, int grassStartX, int grassEndX, int grassStartY, int grassEndY, int playerId)
        {
            RenderOnlyObstacle(_textureRegistry.OnlyBarrelTextureId, BARREL_TOP_LEFT, _onlyBarrelOffsetX, _onlyBarrelOffsetY, 22, 21, startX, endX, startY, endY, grassStartX, grassEndX, grassStartY, grassEndY, playerId);
        }

        // Рендерит отдельные элементы прудов
        private void RenderOnlyPond(int startX, int endX, int startY, int endY, int grassStartX, int grassEndX, int grassStartY, int grassEndY, int playerId)
        {
            RenderOnlyObstacle(_textureRegistry.OnlyPondTextureId, POND_TOP_LEFT, _onlyPondOffsetX, _onlyPondOffsetY, 28, 28, startX, endX, startY, endY, grassStartX, grassEndX, grassStartY, grassEndY, playerId);
        }

        // Рендерит забор
        private void RenderFence(int grassStartX, int grassStartY)
        {
            float fenceTilesX = 106f / 16f;
            float fenceTilesY = 235f / 16f;
            float fenceWidth = fenceTilesX * TileSize;
            float fenceHeight = fenceTilesY * TileSize;

            float tileMapX = grassStartX + 2;
            float tileMapY = grassStartY + 1;
            float tileLeftX = tileMapX * TileSize;
            float tileTopY = tileMapY * TileSize;

            float offsetXInTiles = 0.1875f;
            float offsetYInTiles = 0.0607f;
            float px = tileLeftX + offsetXInTiles * TileSize;
            float py = tileTopY + offsetYInTiles * TileSize;

            GL.BindTexture(TextureTarget.Texture2D, _textureRegistry.FenceTextureId);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0f, 0f); GL.Vertex2(px, py);
            GL.TexCoord2(1f, 0f); GL.Vertex2(px + fenceWidth, py);
            GL.TexCoord2(1f, 1f); GL.Vertex2(px + fenceWidth, py + fenceHeight);
            GL.TexCoord2(0f, 1f); GL.Vertex2(px, py + fenceHeight);
            GL.End();
        }

        // Получает индекс тайла для заданной позиции и игрока
        private int GetTileIndex(int mapX, int mapY, int playerId)
        {
            int[,] mapTiles = playerId == 1 ? _map.Player1MapTiles : _map.Player2MapTiles;
            if (mapY >= 0 && mapY < mapTiles.GetLength(0) && mapX >= 0 && mapX < mapTiles.GetLength(1))
                return mapTiles[mapY, mapX];
            return 4; // Значение по умолчанию
        }

        // Проверяет, является ли тайл базовым для препятствия
        private bool IsObstacleBaseTile(int mapX, int mapY, int playerId)
        {
            return false; // Пока всегда возвращает false (заглушка)
        }
    }
}