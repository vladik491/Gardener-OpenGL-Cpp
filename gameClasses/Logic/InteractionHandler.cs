using Gardener.gameClasses.Rendering;
using Gardener.gameClasses.PlayerManagement;
using Gardener.gameClasses.Audio;
using Gardener.gameClasses.Problems;

namespace Gardener.gameClasses.Logic
{
    // Обрабатывает взаимодействия игроков с объектами
    public class InteractionHandler
    {
        // Блок: Зависимости
        private readonly SpriteRenderer _spriteRenderer; 
        private readonly AudioManager _audioManager;
        private readonly Map _map; 

        // Блок: Константы
        private const int TileSize = GameRenderer.TileSize;
        private const float InteractionRange = 2.0f * TileSize;

        // Инициализирует обработчик с заданными зависимостями
        public InteractionHandler(SpriteRenderer spriteRenderer, AudioManager audioManager, Map map)
        {
            _spriteRenderer = spriteRenderer;
            _audioManager = audioManager;
            _map = map;
        }

        // Обрабатывает взаимодействия игроков
        public void Update(float deltaTime, Player player1, Player player2, bool player1Interact, bool player2Interact, PlayerState player1State, PlayerState player2State, InventoryManager inventoryManager)
        {
            if (player1 == null || player2 == null || player1State == null || player2State == null || inventoryManager == null) return;

            int tilesX = _map.Width / TileSize;
            int tilesY = _map.Height / TileSize;
            int grassStartX = (tilesX - 11) / 2;
            int grassStartY = (tilesY - 17) / 2;

            // Координаты объектов для взаимодействия
            float treeInteractX = (grassStartX + SpriteRenderer.DefaultTreePosX) * TileSize + TileSize / 2f;
            float treeVisualHeightScaled = 39f * Player.Scale;
            float treeInteractY = (grassStartY + SpriteRenderer.DefaultTreePosY) * TileSize + SpriteRenderer.DefaultTreeOffsetY + treeVisualHeightScaled / 2f;

            float boxWidthScaled = 20f * (TileSize / 16f);
            float boxHeightScaled = 17f * (TileSize / 16f);
            float boxApplesCenterX = (grassStartX + SpriteRenderer.DefaultBoxOfApplesPosX) * TileSize + SpriteRenderer.DefaultBoxOffsetX + boxWidthScaled / 2f;
            float boxApplesCenterY = (grassStartY + SpriteRenderer.DefaultBoxOfApplesPosY) * TileSize + SpriteRenderer.DefaultBoxOffsetY + boxHeightScaled / 2f;
            float boxPearsCenterX = (grassStartX + SpriteRenderer.DefaultBoxOfPearsPosX) * TileSize + SpriteRenderer.DefaultBoxOffsetX + boxWidthScaled / 2f;
            float boxPearsCenterY = (grassStartY + SpriteRenderer.DefaultBoxOfPearsPosY) * TileSize + SpriteRenderer.DefaultBoxOffsetY + boxHeightScaled / 2f;

            float fruitWorldX = (grassStartX + SpriteRenderer.DefaultFruitPosX) * TileSize;
            float fruitWorldY = (grassStartY + SpriteRenderer.DefaultFruitPosY) * TileSize;
            float fruitWidthScaled = 7 * SpriteRenderer.FruitScale;
            float fruitHeightScaled = 7 * SpriteRenderer.FruitScale;
            float fruitCenterX = fruitWorldX + fruitWidthScaled / 2f;
            float fruitCenterY = fruitWorldY + fruitHeightScaled / 2f;

            // Центры игроков
            RectangleF p1Bounds = player1.CollisionBounds;
            RectangleF p2Bounds = player2.CollisionBounds;
            float p1CenterX = p1Bounds.Left + p1Bounds.Width / 2f;
            float p1CenterY = p1Bounds.Top + p1Bounds.Height / 2f;
            float p2CenterX = p2Bounds.Left + p2Bounds.Width / 2f;
            float p2CenterY = p2Bounds.Top + p2Bounds.Height / 2f;

            // Проверка расстояний
            bool player1NearTree = CalculateDistance(p1CenterX, p1CenterY, treeInteractX, treeInteractY) <= InteractionRange;
            bool player2NearTree = CalculateDistance(p2CenterX, p2CenterY, treeInteractX, treeInteractY) <= InteractionRange;
            bool player1NearBox = CalculateDistance(p1CenterX, p1CenterY, boxApplesCenterX, boxApplesCenterY) <= InteractionRange;
            bool player2NearBox = CalculateDistance(p2CenterX, p2CenterY, boxPearsCenterX, boxPearsCenterY) <= InteractionRange;
            bool player1NearFruit = CalculateDistance(p1CenterX, p1CenterY, fruitCenterX, fruitCenterY) <= InteractionRange;
            bool player2NearFruit = CalculateDistance(p2CenterX, p2CenterY, fruitCenterX, fruitCenterY) <= InteractionRange;

            // Взаимодействие игрока 1
            if (player1Interact)
            {
                int selectedItem1 = inventoryManager.GetSelectedItem(1);
                bool isHoldingFruit1 = _spriteRenderer.IsPlayerHoldingFruit(1);
                if (player1NearTree && !isHoldingFruit1 && selectedItem1 != 0 && player1State.Problem != null && IsCorrectItemForProblem(selectedItem1, player1State.Problem.Type))
                {
                    PlaySoundForItem(selectedItem1);
                    player1State.AddFruitStage();
                    player1State.ResetProblem();
                }
                else if (player1NearFruit && player1State.FruitOnGround && !isHoldingFruit1 && selectedItem1 == 0)
                {
                    _spriteRenderer.SetHoldingFruit(1, true);
                    player1State.SetFruitOnGround(false);
                    player1State.ResetFruitStage();
                    _audioManager.Play("theSoundOfFruitBeingLifted");
                }
                else if (player1NearBox && isHoldingFruit1)
                {
                    _audioManager.Play("soundOpening");
                    if (_spriteRenderer.ApplesBoxState < SpriteRenderer.MaxBoxState)
                    {
                        _spriteRenderer.IncrementBoxState(1);
                        if (_spriteRenderer.ApplesBoxState == SpriteRenderer.MaxBoxState)
                            _audioManager.Play("soundOfVictory");
                    }
                    _spriteRenderer.SetHoldingFruit(1, false);
                }
            }

            // Взаимодействие игрока 2
            if (player2Interact)
            {
                int selectedItem2 = inventoryManager.GetSelectedItem(2);
                bool isHoldingFruit2 = _spriteRenderer.IsPlayerHoldingFruit(2);
                if (player2NearTree && !isHoldingFruit2 && selectedItem2 != 0 && player2State.Problem != null && IsCorrectItemForProblem(selectedItem2, player2State.Problem.Type))
                {
                    PlaySoundForItem(selectedItem2);
                    player2State.AddFruitStage();
                    player2State.ResetProblem();
                }
                else if (player2NearFruit && player2State.FruitOnGround && !isHoldingFruit2 && selectedItem2 == 0)
                {
                    _spriteRenderer.SetHoldingFruit(2, true);
                    player2State.SetFruitOnGround(false);
                    player2State.ResetFruitStage();
                    _audioManager.Play("theSoundOfFruitBeingLifted");
                }
                else if (player2NearBox && isHoldingFruit2)
                {
                    _audioManager.Play("soundOpening");
                    if (_spriteRenderer.PearsBoxState < SpriteRenderer.MaxBoxState)
                    {
                        _spriteRenderer.IncrementBoxState(2);
                        if (_spriteRenderer.PearsBoxState == SpriteRenderer.MaxBoxState)
                            _audioManager.Play("soundOfVictory");
                    }
                    _spriteRenderer.SetHoldingFruit(2, false);
                }
            }
        }

        // Вычисляет расстояние между точками
        private float CalculateDistance(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        // Проверяет, подходит ли предмет для решения проблемы
        private bool IsCorrectItemForProblem(int slot, ProblemType problemType)
        {
            return (slot, problemType) switch
            {
                (1, ProblemType.Watering) => true,
                (2, ProblemType.Fertilizing) => true,
                (3, ProblemType.Fertilizing) => true,
                (4, ProblemType.Fertilizing) => true,
                (5, ProblemType.Pests) => true,
                (6, ProblemType.Fungus) => true,
                (7, ProblemType.Virus) => true,
                (8, ProblemType.Tending) => true,
                _ => false
            };
        }

        // Проигрывает звук для выбранного предмета
        private void PlaySoundForItem(int slot)
        {
            if (slot == 1)
                _audioManager.Play("soundLeica");
            else if (slot >= 2 && slot <= 4)
                _audioManager.Play("soundOfMud");
            else if (slot >= 5 && slot <= 7)
                _audioManager.Play("soundSpray");
            else if (slot == 8)
                _audioManager.Play("theSoundOfAPrick");
        }
    }
}