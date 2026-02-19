using Gardener.gameClasses.PlayerManagement;
using Gardener.gameClasses.Physics;
using Gardener.gameClasses.Audio;
using Gardener.gameClasses.ScreenManagement;
using Gardener.gameClasses.Rendering;
using Gardener.gameClasses.Mechanics.Trees;

namespace Gardener.gameClasses.Logic
{
    // Управляет общей логикой игры
    public class GameManager
    {
        // Блок: Игровые объекты
        public readonly Player _player1; 
        public readonly Player _player2;
        public readonly InventoryManager _inventoryManager;
        private readonly CollisionManager _collisionManager; 
        private readonly AudioManager _audioManager; 
        public readonly GameStartScreen _startScreen; 
        public readonly GameEndScreen _endScreen;
        public readonly SpriteRenderer _spriteRenderer; 
        public readonly Map _map; 
        private readonly TextureRegistry _textureRegistry; 

        // Блок: Состояния игроков и обработчики
        private readonly PlayerState _player1State; 
        private readonly PlayerState _player2State;
        private readonly InventoryHandler _inventoryHandler;
        private readonly InteractionHandler _interactionHandler; 

        // Блок: Состояния и настройки
        private bool _backgroundMusicStopped; 
        public readonly int _selectedCharacterPlayer1; 
        public readonly int _selectedCharacterPlayer2;

        // Состояние игры
        public PlayerState Player1State => _player1State;
        public PlayerState Player2State => _player2State;
        public InventoryHandler InventoryHandler => _inventoryHandler;
        public InteractionHandler InteractionHandler => _interactionHandler;
        public bool IsGameOver => _endScreen.IsActive;
        public bool IsStartScreenActive => _startScreen.IsActive;

        // Инициализирует игру с заданной картой, персонажами и настройками звука
        public GameManager(Map map, int selectedCharacterPlayer1, int selectedCharacterPlayer2, bool isMusicEnabled, bool isSoundEnabled)
        {
            _map = map;
            _textureRegistry = new TextureRegistry(new TextureManager());
            _collisionManager = new CollisionManager();
            _audioManager = new AudioManager { IsMusicEnabled = isMusicEnabled, IsSoundEnabled = isSoundEnabled };
            _spriteRenderer = new SpriteRenderer(_textureRegistry, map, _audioManager);
            _player1 = new Player(new TextureManager(), map, selectedCharacterPlayer1, 1, _collisionManager);
            _player2 = new Player(new TextureManager(), map, selectedCharacterPlayer2, 2, _collisionManager);
            _inventoryManager = new InventoryManager(_textureRegistry, _audioManager);
            _startScreen = new GameStartScreen(_textureRegistry, _audioManager);
            _endScreen = new GameEndScreen(_textureRegistry, _audioManager, selectedCharacterPlayer1, selectedCharacterPlayer2);
            _backgroundMusicStopped = false;
            _selectedCharacterPlayer1 = selectedCharacterPlayer1;
            _selectedCharacterPlayer2 = selectedCharacterPlayer2;

            _player1State = new PlayerState(new AppleTreeDecorator(new Tree(null)));
            _player2State = new PlayerState(new PearTreeDecorator(new Tree(null)));
            _inventoryHandler = new InventoryHandler(_inventoryManager, _audioManager);
            _interactionHandler = new InteractionHandler(_spriteRenderer, _audioManager, _map);

            InitializeStaticColliders(map);
            _audioManager.Play("backgroundMusic");
        }

        // Инициализирует коллизии на карте
        private void InitializeStaticColliders(Map map)
        {
            _collisionManager.Clear();
            int tilesX = map.Width / GameRenderer.TileSize;
            int tilesY = map.Height / GameRenderer.TileSize;
            int grassStartX = (tilesX - 11) / 2;
            int grassStartY = (tilesY - 17) / 2;
            float scale = GameRenderer.TileSize / 16f;
            AddPlayerSpecificColliders(map, grassStartX, grassStartY, scale);
            AddCommonColliders(grassStartX, grassStartY, scale);
            AddFenceColliders(grassStartX, grassStartY);
            AddCustomColliders(grassStartX, grassStartY);
        }

        // Добавляет коллизии для объектов игроков (кусты, бревна, бочки, пруд)
        private void AddPlayerSpecificColliders(Map map, int grassStartX, int grassStartY, float scale)
        {
            float onlyBushOffsetX = 0.12f * GameRenderer.TileSize;
            float onlyBushOffsetY = 0.32f * GameRenderer.TileSize;
            float onlyBushWidth = 26.8f * scale;
            float onlyBushHeight = 18 * scale;

            float onlyBrevnoOffsetX = 0.07f * GameRenderer.TileSize;
            float onlyBrevnoOffsetY = 0.57f * GameRenderer.TileSize;
            float onlyBrevnoWidth = 30 * scale;
            float onlyBrevnoHeight = 19 * scale;

            float onlyBarrelOffsetX = 0.32f * GameRenderer.TileSize;
            float onlyBarrelOffsetY = 0.24f * GameRenderer.TileSize;
            float onlyBarrelWidth = 21.5f * scale;
            float onlyBarrelHeight = 18 * scale;

            float onlyPondOffsetX = 0.11f * GameRenderer.TileSize;
            float onlyPondOffsetY = 0.12f * GameRenderer.TileSize;
            float onlyPondWidth = 27 * scale;
            float onlyPondHeight = 25 * scale;

            var processedBaseTilesPlayer1 = new HashSet<Point>();
            var processedBaseTilesPlayer2 = new HashSet<Point>();

            int[,] map1Tiles = map.Player1MapTiles;
            for (int my = 0; my < map1Tiles.GetLength(0); my++)
            {
                for (int mx = 0; mx < map1Tiles.GetLength(1); mx++)
                {
                    int tileIndex = map1Tiles[my, mx];
                    var location = new Point(mx, my);
                    if ((tileIndex == MapRenderer.BUSH_TOP_LEFT || tileIndex == MapRenderer.BREVNO_TOP_LEFT) && processedBaseTilesPlayer1.Add(location))
                    {
                        float worldX = (grassStartX + mx) * GameRenderer.TileSize;
                        float worldY = (grassStartY + my) * GameRenderer.TileSize;
                        if (tileIndex == MapRenderer.BUSH_TOP_LEFT)
                            _collisionManager.AddStaticObject(new CollidableObject(worldX + onlyBushOffsetX, worldY + onlyBushOffsetY, onlyBushWidth, onlyBushHeight, "Bush"), 1);
                        else if (tileIndex == MapRenderer.BREVNO_TOP_LEFT)
                            _collisionManager.AddStaticObject(new CollidableObject(worldX + onlyBrevnoOffsetX, worldY + onlyBrevnoOffsetY, onlyBrevnoWidth, onlyBrevnoHeight, "Brevno"), 1);
                    }
                }
            }

            int[,] map2Tiles = map.Player2MapTiles;
            for (int my = 0; my < map2Tiles.GetLength(0); my++)
            {
                for (int mx = 0; mx < map2Tiles.GetLength(1); mx++)
                {
                    int tileIndex = map2Tiles[my, mx];
                    var location = new Point(mx, my);
                    if ((tileIndex == MapRenderer.BARREL_TOP_LEFT || tileIndex == MapRenderer.POND_TOP_LEFT) && processedBaseTilesPlayer2.Add(location))
                    {
                        float worldX = (grassStartX + mx) * GameRenderer.TileSize;
                        float worldY = (grassStartY + my) * GameRenderer.TileSize;
                        if (tileIndex == MapRenderer.BARREL_TOP_LEFT)
                            _collisionManager.AddStaticObject(new CollidableObject(worldX + onlyBarrelOffsetX, worldY + onlyBarrelOffsetY, onlyBarrelWidth, onlyBarrelHeight, "Barrel"), 2);
                        else if (tileIndex == MapRenderer.POND_TOP_LEFT)
                            _collisionManager.AddStaticObject(new CollidableObject(worldX + onlyPondOffsetX, worldY + onlyPondOffsetY, onlyPondWidth, onlyPondHeight, "Pond"), 2);
                    }
                }
            }
        }

        // Добавляет общие коллизии (столы, дерево, ящики)
        private void AddCommonColliders(int grassStartX, int grassStartY, float scale)
        {
            float tableWidth = 32f * scale;
            float tableHeight = 17f * scale;
            float trunkWidth = 6f * Player.Scale;
            float trunkHeight = 25f * Player.Scale;
            float treeCollisionOffsetY = 3f * Player.Scale;
            float boxWidth = 20f * scale;
            float boxHeight = 17f * scale;

            float table1WorldX = (grassStartX + SpriteRenderer.DefaultTable1PosX) * GameRenderer.TileSize + SpriteRenderer.DefaultTableOffsetX;
            float table1WorldY = (grassStartY + SpriteRenderer.DefaultTable1PosY) * GameRenderer.TileSize + SpriteRenderer.DefaultTableOffsetY;
            _collisionManager.AddCommonStaticObject(new CollidableObject(table1WorldX, table1WorldY, tableWidth, tableHeight, "Table1"));

            float table2WorldX = (grassStartX + SpriteRenderer.DefaultTable2PosX) * GameRenderer.TileSize + SpriteRenderer.DefaultTableOffsetX;
            float table2WorldY = (grassStartY + SpriteRenderer.DefaultTable2PosY) * GameRenderer.TileSize + SpriteRenderer.DefaultTableOffsetY;
            _collisionManager.AddCommonStaticObject(new CollidableObject(table2WorldX, table2WorldY, tableWidth, tableHeight, "Table2"));

            float treeWorldX = (grassStartX + SpriteRenderer.DefaultTreePosX) * GameRenderer.TileSize;
            float treeVisualHeightScaled = 39f * Player.Scale;
            float treeBaseWorldY = (grassStartY + SpriteRenderer.DefaultTreePosY) * GameRenderer.TileSize + SpriteRenderer.DefaultTreeOffsetY + treeVisualHeightScaled;
            float treeColliderActualY = treeBaseWorldY - trunkHeight - treeCollisionOffsetY;
            _collisionManager.AddCommonStaticObject(new CollidableObject(treeWorldX + (GameRenderer.TileSize - trunkWidth) / 2, treeColliderActualY, trunkWidth, trunkHeight, "TreeTrunk"));

            float boxApplesWorldX = (grassStartX + SpriteRenderer.DefaultBoxOfApplesPosX) * GameRenderer.TileSize + SpriteRenderer.DefaultBoxOffsetX;
            float boxApplesWorldY = (grassStartY + SpriteRenderer.DefaultBoxOfApplesPosY) * GameRenderer.TileSize + SpriteRenderer.DefaultBoxOffsetY;
            _collisionManager.AddCommonStaticObject(new CollidableObject(boxApplesWorldX, boxApplesWorldY, boxWidth, boxHeight, "BoxApples"));

            float boxPearsWorldX = (grassStartX + SpriteRenderer.DefaultBoxOfPearsPosX) * GameRenderer.TileSize + SpriteRenderer.DefaultBoxOffsetX;
            float boxPearsWorldY = (grassStartY + SpriteRenderer.DefaultBoxOfPearsPosY) * GameRenderer.TileSize + SpriteRenderer.DefaultBoxOffsetY;
            _collisionManager.AddCommonStaticObject(new CollidableObject(boxPearsWorldX, boxPearsWorldY, boxWidth, boxHeight, "BoxPears"));
        }

        // Добавляет коллизии для забора
        private void AddFenceColliders(int grassStartX, int grassStartY)
        {
            float pixelToWorld = GameRenderer.TileSize / 16f;
            float fenceTilesX = 106f / 16f;
            float fenceTilesY = 235f / 16f;
            float fenceWidth = fenceTilesX * GameRenderer.TileSize;
            float fenceHeight = fenceTilesY * GameRenderer.TileSize;
            float fenceOffsetXInTiles = 2.186f;
            float fenceOffsetYInTiles = 1.05f;
            float fenceWorldX = grassStartX * GameRenderer.TileSize + (fenceOffsetXInTiles * GameRenderer.TileSize);
            float fenceWorldY = grassStartY * GameRenderer.TileSize + (fenceOffsetYInTiles * GameRenderer.TileSize);

            float leftSideWidth = 10 * pixelToWorld;
            float leftSideHeight = 235 * pixelToWorld;
            _collisionManager.AddCommonStaticObject(new CollidableObject(fenceWorldX, fenceWorldY, leftSideWidth, leftSideHeight, "FenceLeftSide"));

            float rightSideX = fenceWorldX + (96 * pixelToWorld);
            float rightSideWidth = 10 * pixelToWorld;
            float rightSideHeight = 235 * pixelToWorld;
            _collisionManager.AddCommonStaticObject(new CollidableObject(rightSideX, fenceWorldY, rightSideWidth, rightSideHeight, "FenceRightSide"));

            float topRailX = fenceWorldX + (10 * pixelToWorld);
            float topRailWidth = 86 * pixelToWorld;
            float topRailHeight = 13 * pixelToWorld;
            _collisionManager.AddCommonStaticObject(new CollidableObject(topRailX, fenceWorldY, topRailWidth, topRailHeight, "FenceTopRail"));

            float bottomRailX = fenceWorldX + (10 * pixelToWorld);
            float bottomRailY = fenceWorldY + (221 * pixelToWorld);
            float bottomRailWidth = 86 * pixelToWorld;
            float bottomRailHeight = 13 * pixelToWorld;
            _collisionManager.AddCommonStaticObject(new CollidableObject(bottomRailX, bottomRailY, bottomRailWidth, bottomRailHeight, "FenceBottomRail"));
        }

        // Добавляет пользовательские коллизии
        private void AddCustomColliders(int grassStartX, int grassStartY)
        {
            float pixelToWorld = GameRenderer.TileSize / 16f;
            float customColliderWidth32x14 = 32 * pixelToWorld;
            float customColliderHeight32x14 = 12 * pixelToWorld;
            float customColliderWidth10x23 = 10 * pixelToWorld;
            float customColliderHeight10x23 = 23 * pixelToWorld;

            float p1Collider1X = (grassStartX + 2.79f) * GameRenderer.TileSize;
            float p1Collider1Y = (grassStartY + 10.65f) * GameRenderer.TileSize;
            _collisionManager.AddStaticObject(new CollidableObject(p1Collider1X, p1Collider1Y, customColliderWidth32x14, customColliderHeight32x14, "P1CustomCollider1_32x14"), 1);

            float p1Collider2X = (grassStartX + 6.22f) * GameRenderer.TileSize;
            float p1Collider2Y = (grassStartY + 10.65f) * GameRenderer.TileSize;
            _collisionManager.AddStaticObject(new CollidableObject(p1Collider2X, p1Collider2Y, customColliderWidth32x14, customColliderHeight32x14, "P1CustomCollider2_32x14"), 1);

            float p1Collider3X = (grassStartX + 5.57f) * GameRenderer.TileSize;
            float p1Collider3Y = (grassStartY + 13.668f) * GameRenderer.TileSize;
            _collisionManager.AddStaticObject(new CollidableObject(p1Collider3X, p1Collider3Y, customColliderWidth10x23, customColliderHeight10x23, "P1CustomCollider3_10x23"), 1);

            float p2Collider1X = (grassStartX + 2.79f) * GameRenderer.TileSize;
            float p2Collider1Y = (grassStartY + 10.65f) * GameRenderer.TileSize;
            _collisionManager.AddStaticObject(new CollidableObject(p2Collider1X, p2Collider1Y, customColliderWidth32x14, customColliderHeight32x14, "P2CustomCollider1_32x14"), 2);

            float p2Collider2X = (grassStartX + 6.22f) * GameRenderer.TileSize;
            float p2Collider2Y = (grassStartY + 10.65f) * GameRenderer.TileSize;
            _collisionManager.AddStaticObject(new CollidableObject(p2Collider2X, p2Collider2Y, customColliderWidth32x14, customColliderHeight32x14, "P2CustomCollider2_32x14"), 2);

            float p2Collider3X = (grassStartX + 5.57f) * GameRenderer.TileSize;
            float p2Collider3Y = (grassStartY + 13.668f) * GameRenderer.TileSize;
            _collisionManager.AddStaticObject(new CollidableObject(p2Collider3X, p2Collider3Y, customColliderWidth10x23, customColliderHeight10x23, "P2CustomCollider3_10x23"), 2);
        }

        // Обновляет состояние игры
        public void Update(
            float deltaTime,
            bool player1MoveUp, bool player1MoveDown, bool player1MoveLeft, bool player1MoveRight,
            bool player2MoveUp, bool player2MoveDown, bool player2MoveLeft, bool player2MoveRight,
            bool player1Interact, bool player2Interact,
            bool[] numberKeysPlayer1, bool[] numberKeysPlayer2)
        {
            if (_endScreen.IsActive)
            {
                if (!_backgroundMusicStopped)
                {
                    _audioManager.Stop("backgroundMusic");
                    _backgroundMusicStopped = true;
                }
                _endScreen.Update(deltaTime);
                return;
            }
            if (_startScreen.IsActive)
            {
                _startScreen.Update(deltaTime);
                return;
            }

            _player1.Update(deltaTime, player1MoveUp, player1MoveDown, player1MoveLeft, player1MoveRight);
            _player2.Update(deltaTime, player2MoveUp, player2MoveDown, player2MoveLeft, player2MoveRight);

            _player1State.Update(deltaTime, _spriteRenderer.ApplesBoxState, SpriteRenderer.MaxBoxState, _audioManager.Play);
            _player2State.Update(deltaTime, _spriteRenderer.PearsBoxState, SpriteRenderer.MaxBoxState, _audioManager.Play);

            bool player1HoldingFruit = _spriteRenderer.IsPlayerHoldingFruit(1);
            bool player2HoldingFruit = _spriteRenderer.IsPlayerHoldingFruit(2);
            _inventoryHandler.Update(deltaTime, 1, player1Interact, numberKeysPlayer1, player1HoldingFruit, _player1, this);
            _inventoryHandler.Update(deltaTime, 2, player2Interact, numberKeysPlayer2, player2HoldingFruit, _player2, this);

            _interactionHandler.Update(deltaTime, _player1, _player2, player1Interact, player2Interact, _player1State, _player2State, _inventoryManager);

            _spriteRenderer.Update(deltaTime, _player1, _player2, player1Interact, player2Interact, _player1State, _player2State, _inventoryManager);

            if (_spriteRenderer.ApplesBoxState >= SpriteRenderer.MaxBoxState && !_endScreen.IsActive) _endScreen.SetGameOver(1);
            else if (_spriteRenderer.PearsBoxState >= SpriteRenderer.MaxBoxState && !_endScreen.IsActive) _endScreen.SetGameOver(2);
        }

        // Останавливает и освобождает аудио
        public void StopAndDisposeAudio()
        {
            _audioManager.Dispose();
        }
    }
}