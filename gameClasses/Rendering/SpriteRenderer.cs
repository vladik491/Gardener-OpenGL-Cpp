using OpenTK.Graphics.OpenGL;
using Gardener.gameClasses.Problems;
using Gardener.gameClasses.PlayerManagement;
using Gardener.gameClasses.Audio;
using Gardener.gameClasses.Logic;

namespace Gardener.gameClasses.Rendering
{
    // Управляет рендерингом спрайтов: столы, ящики, фрукты, деревья и проблемы
    public class SpriteRenderer
    {
        // Блок: Ресурсы и зависимости рендеринга
        private readonly TextureRegistry _textureRegistry; 
        private readonly Map _map; 
        private readonly AudioManager _audioManager; 
        private readonly TreeRenderer _treeRenderer; 
        private readonly InventoryUIRenderer _inventoryUIRenderer; 

        public bool Player1HasAppleTree { get; private set; } // Флаг наличия яблони у игрока 1

        private (float texLeft, float texRight, float texTop, float texBottom)[] _boxOfApplesSprites; 
        private (float texLeft, float texRight, float texTop, float texBottom)[] _boxOfPearsSprites; 

        private const int TileSize = GameRenderer.TileSize; // Размер тайла

        // Блок: Константы позиций и смещений
        internal const float DefaultTableOffsetX = 0.428f * TileSize; 
        internal const float DefaultTableOffsetY = -0.328f * TileSize; 
        internal const float DefaultBoxOffsetX = 0.428f * TileSize; 
        internal const float DefaultBoxOffsetY = -0.228f * TileSize; 
        internal const float DefaultTreePosX = 4.87f; 
        internal const float DefaultTreePosY = -0.18f; 
        internal const float DefaultTreeOffsetY = 0.86f * TileSize; 
        internal const float DefaultFruitPosX = 5f;
        internal const float DefaultFruitPosY = 3.05f; 
        internal const float DefaultFruitShadowPosX = 4.87f; 
        internal const float DefaultFruitShadowPosY = 2.635f; 
        internal const float DefaultTable1PosX = 3; 
        internal const float DefaultTable1PosY = 14;
        internal const float DefaultTable2PosX = 3; 
        internal const float DefaultTable2PosY = 14; 
        internal const float DefaultBoxOfApplesPosX = 5.88f;
        internal const float DefaultBoxOfApplesPosY = 13.91f; 
        internal const float DefaultBoxOfPearsPosX = 5.88f; 
        internal const float DefaultBoxOfPearsPosY = 13.91f; 

        // Блок: Текущие позиции и смещения
        private float _tableOffsetX = DefaultTableOffsetX;
        private float _tableOffsetY = DefaultTableOffsetY; 
        private float _boxOffsetX = DefaultBoxOffsetX; 
        private float _boxOffsetY = DefaultBoxOffsetY; 
        private float _treePosX = DefaultTreePosX; 
        private float _treePosY = DefaultTreePosY; 
        private float _treeOffsetY = DefaultTreeOffsetY; 
        private float _fruitPosX = DefaultFruitPosX;
        private float _fruitPosY = DefaultFruitPosY; 
        private float _fruitShadowPosX = DefaultFruitShadowPosX;
        private float _fruitShadowPosY = DefaultFruitShadowPosY;
        private float _table1PosX = DefaultTable1PosX; 
        private float _table1PosY = DefaultTable1PosY; 
        private float _table2PosX = DefaultTable2PosX; 
        private float _table2PosY = DefaultTable2PosY; 
        private float _boxOfApplesPosX = DefaultBoxOfApplesPosX; 
        private float _boxOfApplesPosY = DefaultBoxOfApplesPosY; 
        private float _boxOfPearsPosX = DefaultBoxOfPearsPosX; 
        private float _boxOfPearsPosY = DefaultBoxOfPearsPosY; 

        // Блок: Состояния объектов
        private int _applesBoxState = 0;
        private int _pearsBoxState = 0; 
        private int _applesStage = 0; 
        private int _pearsStage = 0;
        public const int MaxBoxState = 1;
        private const int MaxStage = 3; 

        // Блок: Флаги состояния фруктов
        private bool _player1HoldingFruit; // Флаг: Игрок 1 держит фрукт
        private bool _player2HoldingFruit; 
        private bool _player1FruitOnGround; // Флаг: Фрукт игрока 1 на земле
        private bool _player2FruitOnGround; 

        public const float FruitScale = 4f;
        private const float EmptyIconSize = 32f * 2f; 
        private const float Margin = 10f; 

        // Свойства для доступа к состояниям
        public int ApplesBoxState => _applesBoxState;
        public int PearsBoxState => _pearsBoxState;
        public int ApplesStage => _applesStage;
        public int PearsStage => _pearsStage;

        // Конструктор: Инициализирует рендерер спрайтов
        public SpriteRenderer(TextureRegistry textureRegistry, Map map, AudioManager audioManager)
        {
            _textureRegistry = textureRegistry ?? throw new ArgumentNullException(nameof(textureRegistry));
            _map = map ?? throw new ArgumentNullException(nameof(map));
            _audioManager = audioManager ?? throw new ArgumentNullException(nameof(audioManager));

            Player1HasAppleTree = true; // Игрок 1 всегда получает яблоню

            _treeRenderer = new TreeRenderer(_textureRegistry);
            _inventoryUIRenderer = new InventoryUIRenderer(_textureRegistry);

            _player1HoldingFruit = false;
            _player2HoldingFruit = false;
            _player1FruitOnGround = false;
            _player2FruitOnGround = false;

            InitializeSprites();
        }

        // Инициализирует спрайты для ящиков
        private void InitializeSprites()
        {
            _boxOfApplesSprites = new (float, float, float, float)[11];
            int boxSpriteWidth = 20, boxTextureWidth = 220, boxTextureHeight = 17;
            for (int i = 0; i < _boxOfApplesSprites.Length; i++)
            {
                int sx = i;
                _boxOfApplesSprites[i] = ((sx * boxSpriteWidth) / (float)boxTextureWidth, ((sx + 1) * boxSpriteWidth) / (float)boxTextureWidth, 0f, 1f);
            }
            _boxOfPearsSprites = new (float, float, float, float)[11];
            for (int i = 0; i < _boxOfPearsSprites.Length; i++)
            {
                int sx = i;
                _boxOfPearsSprites[i] = ((sx * boxSpriteWidth) / (float)boxTextureWidth, ((sx + 1) * boxSpriteWidth) / (float)boxTextureWidth, 0f, 1f);
            }
        }

        // Проверяет, держит ли игрок фрукт
        public bool IsPlayerHoldingFruit(int playerId)
        {
            return playerId == 1 ? _player1HoldingFruit : _player2HoldingFruit;
        }

        // Устанавливает флаг удержания фрукта
        public void SetHoldingFruit(int playerId, bool value)
        {
            if (playerId == 1)
                _player1HoldingFruit = value;
            else
                _player2HoldingFruit = value;
        }

        // Увеличивает состояние ящика
        public void IncrementBoxState(int playerId)
        {
            if (playerId == 1)
                _applesBoxState++;
            else
                _pearsBoxState++;
        }

        // Обновляет состояния рендерера
        public void Update(float deltaTime, Player player1, Player player2, bool player1Interact, bool player2Interact, PlayerState player1State, PlayerState player2State, InventoryManager inventoryManager)
        {
            if (player1 == null || player2 == null || player1State == null || player2State == null || inventoryManager == null) return;

            _player1FruitOnGround = player1State.FruitOnGround;
            _player2FruitOnGround = player2State.FruitOnGround;
            _applesStage = player1State.FruitStage;
            _pearsStage = player2State.FruitStage;
        }

        /// <summary>
        /// Рендерит спрайты: столы, ящики, дерево, фрукты, проблемы и интерфейс инвентаря.
        /// </summary>
        /// <param name="width">Ширина экрана</param>
        /// <param name="height">Высота экрана</param>
        /// <param name="grassStartX">Начальная координата травы по X</param>
        /// <param name="grassStartY">Начальная координата травы по Y</param>
        /// <param name="playerId">ID игрока</param>
        /// <param name="inventoryManager">Менеджер инвентаря</param>
        /// <param name="playerState">Состояние игрока</param>
        /// <param name="numberAnimationScales">Масштабы анимации чисел</param>
        public void Render(int width, int height, int grassStartX, int grassStartY, int playerId, InventoryManager inventoryManager, PlayerState playerState, float[] numberAnimationScales)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            _treeRenderer.RenderShadow(grassStartX, grassStartY); // Рендерим тень

            if (playerId == 1)
            {
                RenderTable(_textureRegistry.Table1TextureId, _table1PosX, _table1PosY, width, height, grassStartX, grassStartY); // Стол для игрока 1
                RenderBox(_textureRegistry.BoxOfApplesTextureId, _boxOfApplesPosX, _boxOfApplesPosY, _applesBoxState, _boxOfApplesSprites, width, height, grassStartX, grassStartY); // Ящик с яблоками
            }
            else
            {
                RenderTable(_textureRegistry.Table2TextureId, _table2PosX, _table2PosY, width, height, grassStartX, grassStartY); // Стол для игрока 2
                RenderBox(_textureRegistry.BoxOfPearsTextureId, _boxOfPearsPosX, _boxOfPearsPosY, _pearsBoxState, _boxOfPearsSprites, width, height, grassStartX, grassStartY); // Ящик с грушами
            }

            _treeRenderer.RenderTree(grassStartX, grassStartY); // Рендерим дерево

            // Рендерим фрукт, если он на земле
            bool playerFruitOnGround = (playerId == 1) ? _player1FruitOnGround : _player2FruitOnGround;
            bool playerIsHoldingFruit = (playerId == 1) ? _player1HoldingFruit : _player2HoldingFruit;
            if (playerFruitOnGround && !playerIsHoldingFruit)
            {
                int fruitTextureId = (playerId == 1) ? _textureRegistry.AppleTextureId : _textureRegistry.PearTextureId;
                int fruitWidth = 7;
                int fruitHeight = (playerId == 1) ? 7 : 9;
                _treeRenderer.RenderFruit(fruitTextureId, grassStartX, grassStartY, fruitWidth, fruitHeight);
            }

            // Рендерим иконку проблемы
            Problem playerProblem = playerState.Problem;
            int playerBoxState = (playerId == 1) ? _applesBoxState : _pearsBoxState;
            RenderProblemIcon(grassStartX, grassStartY, playerProblem, playerBoxState);

            // Рендерим интерфейс инвентаря
            GL.MatrixMode(MatrixMode.Projection); GL.PushMatrix(); GL.MatrixMode(MatrixMode.Modelview); GL.PushMatrix();
            bool isInventoryOpen = (playerId == 1) ? inventoryManager.IsInventoryOpen(1) : inventoryManager.IsInventoryOpen(2);
            if (isInventoryOpen)
            {
                int viewportOffsetX = (playerId == 1) ? 0 : width / 2;
                _inventoryUIRenderer.RenderInventoryPanel(width, height, inventoryManager, numberAnimationScales, playerId, viewportOffsetX);
            }
            RenderSelectedItem(width, height, playerId, inventoryManager);
            GL.MatrixMode(MatrixMode.Projection); GL.PopMatrix(); GL.MatrixMode(MatrixMode.Modelview); GL.PopMatrix();

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
        }

        // Рендерит стол
        private void RenderTable(int textureId, float posX, float posY, int width, int height, int grassStartX, int grassStartY)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            float tablePosX = (grassStartX + posX) * TileSize + _tableOffsetX;
            float tablePosY = (grassStartY + posY) * TileSize + _tableOffsetY;
            float tableWidth = 32f * (TileSize / 16f);
            float tableHeight = 17f * (TileSize / 16f);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0f, 0f); GL.Vertex2(tablePosX, tablePosY);
            GL.TexCoord2(1f, 0f); GL.Vertex2(tablePosX + tableWidth, tablePosY);
            GL.TexCoord2(1f, 1f); GL.Vertex2(tablePosX + tableWidth, tablePosY + tableHeight);
            GL.TexCoord2(0f, 1f); GL.Vertex2(tablePosX, tablePosY + tableHeight);
            GL.End();
        }

        // Рендерит ящик
        private void RenderBox(int textureId, float posX, float posY, int state, (float texLeft, float texRight, float texTop, float texBottom)[] sprites, int width, int height, int grassStartX, int grassStartY)
        {
            state = Math.Clamp(state, 0, sprites.Length - 1);
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            float boxPosX = (grassStartX + posX) * TileSize + _boxOffsetX;
            float boxPosY = (grassStartY + posY) * TileSize + _boxOffsetY;
            float boxWidth = 20f * (TileSize / 16f);
            float boxHeight = 17f * (TileSize / 16f);
            var sprite = sprites[state];
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(sprite.texLeft, sprite.texTop); GL.Vertex2(boxPosX, boxPosY);
            GL.TexCoord2(sprite.texRight, sprite.texTop); GL.Vertex2(boxPosX + boxWidth, boxPosY);
            GL.TexCoord2(sprite.texRight, sprite.texBottom); GL.Vertex2(boxPosX + boxWidth, boxPosY + boxHeight);
            GL.TexCoord2(sprite.texLeft, sprite.texBottom); GL.Vertex2(boxPosX, boxPosY + boxHeight);
            GL.End();
        }

        // Рендерит иконку проблемы над деревом
        private void RenderProblemIcon(int grassStartX, int grassStartY, Problem problem, int boxState)
        {
            if (problem == null) return;
            int textureId;
            if (problem.Type == ProblemType.Fertilizing)
            {
                if (boxState < 3) textureId = _textureRegistry.Level1FertilizerIconId;
                else if (boxState < 6) textureId = _textureRegistry.Level2FertilizerIconId;
                else textureId = _textureRegistry.Level3FertilizerIconId;
            }
            else
            {
                textureId = problem.Type switch
                {
                    ProblemType.Watering => _textureRegistry.DropIconId,
                    ProblemType.Fungus => _textureRegistry.FungusIconId,
                    ProblemType.Pests => _textureRegistry.CrossedOutInsectIconId,
                    ProblemType.Virus => _textureRegistry.VirusIconId,
                    ProblemType.Tending => _textureRegistry.SyringeIconId,
                    _ => _textureRegistry.DropIconId
                };
            }
            float treeTopVisY = (grassStartY + _treePosY) * TileSize + _treeOffsetY;
            float treeVisCenterX = (grassStartX + _treePosX) * TileSize + TileSize / 2f;
            float iconWidth = 20f * 3;
            float iconHeight = 20f * 3;
            float iconPosX = treeVisCenterX - iconWidth / 2f;
            float iconPosY = treeTopVisY - iconHeight - 5;
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0f, 0f); GL.Vertex2(iconPosX, iconPosY);
            GL.TexCoord2(1f, 0f); GL.Vertex2(iconPosX + iconWidth, iconPosY);
            GL.TexCoord2(1f, 1f); GL.Vertex2(iconPosX + iconWidth, iconPosY + iconHeight);
            GL.TexCoord2(0f, 1f); GL.Vertex2(iconPosX, iconPosY + iconHeight);
            GL.End();
        }

        // Рендерит выбранный предмет в интерфейсе
        private void RenderSelectedItem(int width, int height, int playerId, InventoryManager inventoryManager)
        {
            GL.MatrixMode(MatrixMode.Projection); GL.LoadIdentity(); GL.Ortho(0, width, height, 0, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview); GL.LoadIdentity();
            GL.Enable(EnableCap.Texture2D); GL.Enable(EnableCap.Blend);
            float panelWidth = 16f * 5;
            float panelHeight = 70f * 5;
            float iconPosX = Margin + (panelWidth - EmptyIconSize) / 2;
            float iconPosY = Margin + panelHeight + 10;
            int selectedItemSlot = inventoryManager.GetSelectedItem(playerId);
            bool holdingFruit = (playerId == 1) ? _player1HoldingFruit : _player2HoldingFruit;
            int textureId;
            float texLeft = 0f, texRight = 1f;
            if (holdingFruit)
            {
                textureId = (playerId == 1) ? _textureRegistry.AppleIconTextureId : _textureRegistry.PearIconTextureId;
                texLeft = 0f; texRight = 1f;
            }
            else if (selectedItemSlot == 0)
            {
                textureId = _textureRegistry.EmptyIconTextureId;
                texLeft = 0f; texRight = 1f;
            }
            else
            {
                var items = inventoryManager.GetItems();
                if (selectedItemSlot > 0 && selectedItemSlot <= items.Count)
                {
                    textureId = items[selectedItemSlot - 1].TextureId;
                    texLeft = 0f; texRight = 0.5f;
                }
                else
                {
                    textureId = _textureRegistry.EmptyIconTextureId;
                    texLeft = 0f; texRight = 1f;
                }
            }
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(texLeft, 0f); GL.Vertex2(iconPosX, iconPosY);
            GL.TexCoord2(texRight, 0f); GL.Vertex2(iconPosX + EmptyIconSize, iconPosY);
            GL.TexCoord2(texRight, 1f); GL.Vertex2(iconPosX + EmptyIconSize, iconPosY + EmptyIconSize);
            GL.TexCoord2(texLeft, 1f); GL.Vertex2(iconPosX, iconPosY + EmptyIconSize);
            GL.End();
        }

        // Увеличивает стадию фрукта
        public void AddFruitStage(int playerId)
        {
            if (playerId == 1)
            {
                if (_applesStage < MaxStage) _applesStage++;
            }
            else
            {
                if (_pearsStage < MaxStage) _pearsStage++;
            }
        }
    }
}