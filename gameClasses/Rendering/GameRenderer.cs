using OpenTK.Graphics.OpenGL;
using Gardener.gameClasses.Logic;

namespace Gardener.gameClasses.Rendering
{
    // Управляет рендерингом всей игры, включая карту, спрайты, игроков и пользовательский интерфейс
    public class GameRenderer
    {
        // Блок: Данные для рендеринга
        private readonly MapRenderer _mapRenderer;
        private readonly SpriteRenderer _spriteRenderer;
        private readonly GameManager _gameManager;
        private readonly TextureRegistry _textureRegistry;
        private (float texLeft, float texRight, float texTop, float texBottom)[] _panelSprites; 
        internal const int TileSize = 48;

        // Конструктор: Инициализирует рендерер с картой и менеджером игры
        public GameRenderer(Map map, GameManager gameManager)
        {
            _gameManager = gameManager;
            _textureRegistry = new TextureRegistry(new TextureManager());
            _mapRenderer = new MapRenderer(_textureRegistry, map);
            _spriteRenderer = _gameManager._spriteRenderer;
            InitializePanelSprites();
        }

        // Инициализирует координаты спрайтов для панели игрока
        private void InitializePanelSprites()
        {
            _panelSprites = new (float, float, float, float)[4];
            int spriteWidth = 16, textureWidth = 64, textureHeight = 70;
            for (int i = 0; i < _panelSprites.Length; i++)
            {
                _panelSprites[i] = ((i * spriteWidth) / (float)textureWidth, ((i + 1) * spriteWidth) / (float)textureWidth, 0f, 1f);
            }
        }

        /// <summary>
        /// Выполняет рендеринг игры в зависимости от её состояния (стартовый экран, игровой процесс или экран окончания игры).
        /// </summary>
        /// <param name="width">Ширина экрана</param>
        /// <param name="height">Высота экрана</param>
        /// <param name="map">Карта игры</param>
        /// <param name="cameraX">Позиция камеры по X</param>
        /// <param name="cameraY">Позиция камеры по Y</param>
        /// <param name="playerId">ID игрока</param>
        /// <param name="playerState">Состояние игрока</param>
        /// <param name="numberAnimationScales">Масштабы анимации для чисел</param>
        public void Render(int width, int height, Map map, float cameraX, float cameraY, int playerId, PlayerState playerState, float[] numberAnimationScales)
        {
            if (_gameManager.IsStartScreenActive)
            {
                _gameManager._startScreen.Render(width, height, playerId); // Отрисовка стартового экрана
                return;
            }
            if (_gameManager.IsGameOver)
            {
                _gameManager._endScreen.Render(width, height, playerId); // Отрисовка экрана окончания игры
                return;
            }

            SetupCamera(width, height, cameraX, cameraY); // Настройка камеры
            int tilesX = map.Width / TileSize;
            int tilesY = map.Height / TileSize;
            int grassStartX = (tilesX - 11) / 2; // Вычисление начальных координат для травяной области
            int grassStartY = (tilesY - 17) / 2;
            _mapRenderer.Render(width, height, cameraX, cameraY, playerId); // Отрисовка карты
            _spriteRenderer.Render(width, height, grassStartX, grassStartY, playerId, _gameManager._inventoryManager, playerState, numberAnimationScales); // Отрисовка спрайтов
            if (playerId == 1) _gameManager._player1.Render(); // Отрисовка игрока 1
            else if (playerId == 2) _gameManager._player2.Render(); // Отрисовка игрока 2
            RenderPanel(width, height, playerId); // Отрисовка панели игрока
        }

        // Проверяет, находится ли курсор мыши над кнопкой меню
        public bool IsMouseOverMenuButton(int mouseX, int mouseY, int width, int height)
        {
            return _gameManager._endScreen.IsMouseOverMenuButton(mouseX, mouseY, width, height);
        }

        // Запускает анимацию кнопки меню
        public void TriggerMenuButtonAnimation()
        {
            _gameManager._endScreen.TriggerMenuButtonAnimation();
        }

        // Настраивает камеру для рендеринга с ортографической проекцией
        private void SetupCamera(int width, int height, float cameraX, float cameraY)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(cameraX, cameraX + width, cameraY + height, cameraY, -1, 1); // Установка ортографической проекции
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        // Отрисовывает панель игрока с текстурой, основанной на ID игрока и выборе персонажа
        private void RenderPanel(int width, int height, int playerId)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, width, height, 0, -1, 1); // Установка проекции для UI
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Enable(EnableCap.Texture2D); // Включение текстур
            GL.Enable(EnableCap.Blend); // Включение прозрачности

            // Выбор текстуры панели на основе игрока и выбора персонажа
            int textureId = playerId == 1 ?
                _gameManager._selectedCharacterPlayer1 switch
                {
                    1 => _textureRegistry.AppleCharacter1PanelId,
                    2 => _textureRegistry.AppleCharacter2PanelId,
                    3 => _textureRegistry.AppleCharacter3PanelId,
                    _ => _textureRegistry.AppleCharacter1PanelId
                } :
                _gameManager._selectedCharacterPlayer2 switch
                {
                    1 => _textureRegistry.PearCharacter1PanelId,
                    2 => _textureRegistry.PearCharacter2PanelId,
                    3 => _textureRegistry.PearCharacter3PanelId,
                    _ => _textureRegistry.PearCharacter1PanelId
                };
            int stage = playerId == 1 ? _spriteRenderer.ApplesStage : _spriteRenderer.PearsStage; // Получение текущего этапа

            const float panelWidth = 16f * 5; 
            const float panelHeight = 70f * 5; 
            const float margin = 10f; 
            float posX = margin;
            float posY = margin;

            GL.BindTexture(TextureTarget.Texture2D, textureId);
            stage = Math.Clamp(stage, 0, _panelSprites.Length - 1); // Ограничение этапа допустимым диапазоном
            var sprite = _panelSprites[stage];

            // Отрисовка панели
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(sprite.texLeft, sprite.texTop); GL.Vertex2(posX, posY);
            GL.TexCoord2(sprite.texRight, sprite.texTop); GL.Vertex2(posX + panelWidth, posY);
            GL.TexCoord2(sprite.texRight, sprite.texBottom); GL.Vertex2(posX + panelWidth, posY + panelHeight);
            GL.TexCoord2(sprite.texLeft, sprite.texBottom); GL.Vertex2(posX, posY + panelHeight);
            GL.End();

            GL.Disable(EnableCap.Blend); // Отключение прозрачности
            GL.Disable(EnableCap.Texture2D); // Отключение текстур
        }

        // Отрисовывает вертикальную линию посередине экрана
        public void RenderSplitLine(int screenWidth, int screenHeight)
        {
            GL.Viewport(0, 0, screenWidth, screenHeight); // Установка вьюпорта на весь экран
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, screenWidth, screenHeight, 0, -1, 1); // Установка проекции для UI
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Disable(EnableCap.Texture2D); // Отключение текстур

            const int LineWidth = 6;
            int half = screenWidth / 2; // Половина ширины экрана

            // Отрисовка чёрной вертикальной линии по центру
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.Black);
            GL.Vertex2(half - LineWidth / 2, 0);
            GL.Vertex2(half + LineWidth / 2, 0);
            GL.Vertex2(half + LineWidth / 2, screenHeight);
            GL.Vertex2(half - LineWidth / 2, screenHeight);
            GL.End();
            GL.Color3(Color.White); // Сброс цвета на белый
        }
    }
}