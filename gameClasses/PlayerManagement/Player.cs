using OpenTK.Graphics.OpenGL;
using Gardener.gameClasses.Rendering;
using Gardener.gameClasses.Physics;

namespace Gardener.gameClasses.PlayerManagement
{
    // Представляет игрока в игре, реализует интерфейс IEntity
    public class Player : IEntity
    {
        // Константы: параметры отображения
        internal const float Scale = 3f; 

        // Блок: Основные данные игрока
        private readonly TextureManager _textureManager;
        private readonly int _characterId; 
        private readonly int _playerId;

        // Блок: Данные для рендеринга
        private readonly int[] _spriteSheetIds; 
        private readonly (int Width, int Height, int SheetWidth)[] _spriteData; 
        private readonly MovementHandler _movementHandler; 

        // Блок: Параметры коллизии
        private readonly float _collisionBoxOffsetX; 
        private readonly float _collisionBoxOffsetY; 
        private readonly float _collisionBoxWidth; 
        private readonly float _collisionBoxHeight; 

        // Свойства: позиция и границы коллизии
        public float PositionX => _movementHandler.PositionX;
        public float PositionY => _movementHandler.PositionY;

        public RectangleF CollisionBounds => new RectangleF(
            PositionX + _collisionBoxOffsetX,
            PositionY + _collisionBoxOffsetY,
            _collisionBoxWidth,
            _collisionBoxHeight);

        // Конструктор: инициализирует игрока
        public Player(TextureManager textureManager, Map map, int characterId, int playerId, CollisionManager collisionManager)
        {
            _textureManager = textureManager ?? throw new ArgumentNullException(nameof(textureManager));
            _characterId = characterId;
            _playerId = playerId;
            _spriteSheetIds = new int[8];

            // Устанавливает параметры коллизии
            (_collisionBoxOffsetX, _collisionBoxOffsetY, _collisionBoxWidth, _collisionBoxHeight) = GetCollisionBoxParameters(characterId);

            // Загружает конфигурацию персонажа
            var config = CharacterConfig.GetConfig(characterId);
            _spriteData = config.SpriteData;

            // Вычисляет начальные координаты
            float initialX = CalculateInitialX(map, playerId);
            float initialY = CalculateInitialY(map);

            // Инициализирует обработчик движения
            _movementHandler = new MovementHandler(
                map, _spriteData, initialX, initialY, Scale, playerId,
                collisionManager, _collisionBoxOffsetX, _collisionBoxOffsetY, _collisionBoxWidth, _collisionBoxHeight);

            // Загружает текстуры персонажа
            InitializeTextures(config.FolderName);
        }

        // Возвращает параметры коллизии в зависимости от ID персонажа
        private (float offsetX, float offsetY, float width, float height) GetCollisionBoxParameters(int characterId)
        {
            return characterId switch
            {
                1 => (2.5f * Scale, 22.3f * Scale, 10.67f * Scale, 5.33f * Scale), // blueCharacter
                2 => (2.5f * Scale, 15.7f * Scale, 10.67f * Scale, 5.33f * Scale), // blackCharacter
                3 => (2.5f * Scale, 16.2f * Scale, 10.67f * Scale, 5.33f * Scale), // japanCharacter
                _ => throw new ArgumentException($"Invalid characterId: {characterId}", nameof(characterId))
            };
        }

        // Вычисляет начальную координату X игрока
        private float CalculateInitialX(Map map, int playerId)
        {
            int tilesX = map.Width / MapRenderer.TileSize;
            int grassStartX = (tilesX - 11) / 2;
            float initialX = (grassStartX + 2f) * MapRenderer.TileSize;
            return initialX + (playerId == 0 ? -MapRenderer.TileSize * 1.5f : MapRenderer.TileSize * 1.5f);
        }

        // Вычисляет начальную координату Y игрока
        private float CalculateInitialY(Map map)
        {
            int tilesY = map.Height / MapRenderer.TileSize;
            int grassStartY = (tilesY - 17) / 2;
            return (grassStartY + 8f) * MapRenderer.TileSize;
        }

        // Загружает текстуры для анимаций персонажа
        private void InitializeTextures(string characterFolder)
        {
            string[] directions = { "downMovement", "downRightMovement", "rightMovement", "upRightMovement",
                                    "upMovement", "upLeftMovement", "leftMovement", "downLeftMovement" };

            for (int i = 0; i < 8; i++)
            {
                string texturePath = $"Assets/Game/characters/{characterFolder}/{directions[i]}.png";
                _spriteSheetIds[i] = _textureManager.LoadTexture(texturePath);
            }
        }

        /// <summary>
        /// Обновляет состояние игрока (движение, анимации).
        /// </summary>
        /// <param name="deltaTime">Время с последнего обновления</param>
        /// <param name="moveUp">Движение вверх</param>
        /// <param name="moveDown">Движение вниз</param>
        /// <param name="moveLeft">Движение влево</param>
        /// <param name="moveRight">Движение вправо</param>
        public void Update(float deltaTime, bool moveUp, bool moveDown, bool moveLeft, bool moveRight)
        {
            _movementHandler.Update(deltaTime, moveUp, moveDown, moveLeft, moveRight);
        }

        // Рендерит игрока на экране (анимация и текстуры)
        public void Render()
        {
            int direction = _movementHandler.CurrentDirection;
            if (direction < 0 || direction >= _spriteSheetIds.Length)
            {
                direction = 0;
            }

            int spriteSheetId = _spriteSheetIds[direction];
            if (spriteSheetId == 0)
            {
                return;
            }

            // Настраивает OpenGL для рендеринга текстур
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.BindTexture(TextureTarget.Texture2D, spriteSheetId);

            // Получает данные текущего спрайта
            var (frameWidth, frameHeight, sheetWidth) = _spriteData[direction];
            int frameIndex = (int)_movementHandler.AnimationFrame;

            if (sheetWidth <= 0 || frameWidth <= 0)
            {
                return;
            }

            // Вычисляет кадры анимации
            int maxFrames = sheetWidth / frameWidth;
            if (maxFrames <= 0)
            {
                maxFrames = 1;
            }
            frameIndex %= maxFrames;

            // Определяет координаты текстуры
            float texLeft = frameIndex * frameWidth / (float)sheetWidth;
            float texRight = (frameIndex + 1) * frameWidth / (float)sheetWidth;
            float texTop = 0f;
            float texBottom = 1f;

            // Масштабирует спрайт
            float scaledWidth = frameWidth * Scale;
            float scaledHeight = frameHeight * Scale;

            // Рисует спрайт игрока
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(texLeft, texTop);
            GL.Vertex2(PositionX, PositionY);
            GL.TexCoord2(texRight, texTop);
            GL.Vertex2(PositionX + scaledWidth, PositionY);
            GL.TexCoord2(texRight, texBottom);
            GL.Vertex2(PositionX + scaledWidth, PositionY + scaledHeight);
            GL.TexCoord2(texLeft, texBottom);
            GL.Vertex2(PositionX, PositionY + scaledHeight);
            GL.End();

            // Отключает настройки OpenGL
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);

            // Закомментировано отладочное отображение коллизий
            /*
            bool debugRenderCollision = true;
            if (debugRenderCollision)
            {
                GL.Disable(EnableCap.Texture2D);
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                GL.LineWidth(2f);
                GL.Color3(Color.Red);

                GL.Begin(PrimitiveType.Quads);
                RectangleF bounds = CollisionBounds;
                GL.Vertex2(bounds.Left, bounds.Top);
                GL.Vertex2(bounds.Right, bounds.Top);
                GL.Vertex2(bounds.Right, bounds.Bottom);
                GL.Vertex2(bounds.Left, bounds.Bottom);
                GL.End();

                GL.Color3(Color.White);
                GL.LineWidth(1f);
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }
            */
        }
    }
}