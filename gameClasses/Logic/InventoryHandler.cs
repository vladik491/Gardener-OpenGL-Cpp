using Gardener.gameClasses.Rendering;
using Gardener.gameClasses.PlayerManagement;
using Gardener.gameClasses.Audio;

namespace Gardener.gameClasses.Logic
{
    // Обрабатывает логику инвентаря игроков
    public class InventoryHandler
    {
        // Блок: Зависимости
        private readonly InventoryManager _inventoryManager; 
        private readonly AudioManager _audioManager; 

        // Блок: Таймеры и анимации
        private float _itemReturnTimerPlayer1;
        private float _itemReturnTimerPlayer2;
        private readonly float[] _numberAnimationScalesPlayer1; 
        private readonly float[] _numberAnimationScalesPlayer2; 
        private readonly float[] _numberAnimationTimersPlayer1; 
        private readonly float[] _numberAnimationTimersPlayer2; 

        // Блок: Константы
        private const float AutoReturnDelay = 20f;
        private const float AnimationDuration = 0.5f;
        private const float InteractionRange = 1.15f * GameRenderer.TileSize;

        // Публичные свойства
        public float ItemReturnTimerPlayer1 => _itemReturnTimerPlayer1;
        public float ItemReturnTimerPlayer2 => _itemReturnTimerPlayer2;

        // Инициализирует обработчик инвентаря с заданными зависимостями
        public InventoryHandler(InventoryManager inventoryManager, AudioManager audioManager)
        {
            _inventoryManager = inventoryManager;
            _audioManager = audioManager;
            _itemReturnTimerPlayer1 = 0f;
            _itemReturnTimerPlayer2 = 0f;
            _numberAnimationScalesPlayer1 = new float[9];
            _numberAnimationScalesPlayer2 = new float[9];
            _numberAnimationTimersPlayer1 = new float[9];
            _numberAnimationTimersPlayer2 = new float[9];

            for (int i = 0; i < 9; i++)
            {
                _numberAnimationScalesPlayer1[i] = 1f;
                _numberAnimationScalesPlayer2[i] = 1f;
                _numberAnimationTimersPlayer1[i] = 0f;
                _numberAnimationTimersPlayer2[i] = 0f;
            }
        }

        // Возвращает масштабы анимации для игрока
        public float[] GetNumberAnimationScales(int playerId)
        {
            return playerId == 1 ? _numberAnimationScalesPlayer1 : _numberAnimationScalesPlayer2;
        }

        // Обновляет состояние инвентаря
        public void Update(float deltaTime, int playerId, bool interact, bool[] numberKeys, bool isHoldingFruit, Player player, GameManager gameManager)
        {
            bool isInventoryOpen = _inventoryManager.IsInventoryOpen(playerId);

            // Координаты стола
            int tilesX = gameManager._map.Width / GameRenderer.TileSize;
            int tilesY = gameManager._map.Height / GameRenderer.TileSize;
            int grassStartX = (tilesX - 11) / 2;
            int grassStartY = (tilesY - 17) / 2;
            float tableWidthScaled = 32f * (GameRenderer.TileSize / 16f);
            float tableHeightScaled = 17f * (GameRenderer.TileSize / 16f);
            float tablePosX = playerId == 1 ? SpriteRenderer.DefaultTable1PosX : SpriteRenderer.DefaultTable2PosX;
            float tablePosY = playerId == 1 ? SpriteRenderer.DefaultTable1PosY : SpriteRenderer.DefaultTable2PosY;
            float tableCenterX = (grassStartX + tablePosX) * GameRenderer.TileSize + SpriteRenderer.DefaultTableOffsetX + tableWidthScaled / 2f;
            float tableCenterY = (grassStartY + tablePosY) * GameRenderer.TileSize + SpriteRenderer.DefaultTableOffsetY + tableHeightScaled / 2f;

            // Центр игрока
            RectangleF playerBounds = player.CollisionBounds;
            float playerCenterX = playerBounds.Left + playerBounds.Width / 2f;
            float playerCenterY = playerBounds.Top + playerBounds.Height / 2f;

            bool isNearTable = CalculateDistance(playerCenterX, playerCenterY, tableCenterX, tableCenterY) <= InteractionRange;

            // Открытие/закрытие инвентаря
            if (isNearTable && interact)
            {
                if (!isInventoryOpen)
                {
                    _inventoryManager.OpenInventory(playerId);
                    _audioManager.Play("soundOpening");
                }
                else
                {
                    _inventoryManager.CloseInventory(playerId);
                    _audioManager.Play("soundOpening");
                }
            }

            // Автозакрытие инвентаря, если игрок отошёл
            if (!isNearTable && isInventoryOpen)
            {
                _inventoryManager.CloseInventory(playerId);
                _audioManager.Play("soundOpening");
            }

            // Обработка выбора предметов
            for (int i = 1; i <= 8; i++)
            {
                if (numberKeys[i] && _inventoryManager.IsInventoryOpen(playerId))
                {
                    int slot = i;
                    HandleItemSelection(playerId, slot, isHoldingFruit);
                    int animationIndex = i - 1;
                    if (playerId == 1)
                        _numberAnimationTimersPlayer1[animationIndex] = AnimationDuration;
                    else
                        _numberAnimationTimersPlayer2[animationIndex] = AnimationDuration;
                }
            }

            UpdateNumberAnimations(deltaTime, playerId);
            UpdateItemReturnTimer(deltaTime, playerId, isHoldingFruit);
        }

        // Вычисляет расстояние между точками
        private float CalculateDistance(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        // Обрабатывает выбор предмета
        private void HandleItemSelection(int playerId, int slot, bool isHoldingFruit)
        {
            if (isHoldingFruit) return;

            int currentSelectedItem = _inventoryManager.GetSelectedItem(playerId);
            if (currentSelectedItem == slot)
            {
                _inventoryManager.ReturnItem(slot, playerId);
            }
            else if (currentSelectedItem == 0 && _inventoryManager.CanSelectItem(slot, playerId))
            {
                _inventoryManager.SelectItem(slot, playerId);
                _audioManager.Play("button");
            }
            else if (currentSelectedItem != 0 && _inventoryManager.CanSelectItem(slot, playerId))
            {
                _inventoryManager.ReturnItem(currentSelectedItem, playerId);
                _inventoryManager.SelectItem(slot, playerId);
                _audioManager.Play("button");
            }
        }

        // Обновляет таймер автоматического возврата предмета
        private void UpdateItemReturnTimer(float deltaTime, int playerId, bool isHoldingFruit)
        {
            int selectedItem = _inventoryManager.GetSelectedItem(playerId);
            if (playerId == 1)
            {
                if (selectedItem != 0 && !isHoldingFruit)
                {
                    _itemReturnTimerPlayer1 += deltaTime;
                    if (_itemReturnTimerPlayer1 >= AutoReturnDelay)
                    {
                        _inventoryManager.ReturnItem(selectedItem, playerId);
                        _itemReturnTimerPlayer1 = 0f;
                    }
                }
                else
                {
                    _itemReturnTimerPlayer1 = 0f;
                }
            }
            else if (playerId == 2)
            {
                if (selectedItem != 0 && !isHoldingFruit)
                {
                    _itemReturnTimerPlayer2 += deltaTime;
                    if (_itemReturnTimerPlayer2 >= AutoReturnDelay)
                    {
                        _inventoryManager.ReturnItem(selectedItem, playerId);
                        _itemReturnTimerPlayer2 = 0f;
                    }
                }
                else
                {
                    _itemReturnTimerPlayer2 = 0f;
                }
            }
        }

        // Обновляет анимации цифр
        private void UpdateNumberAnimations(float deltaTime, int playerId)
        {
            float[] timers = playerId == 1 ? _numberAnimationTimersPlayer1 : _numberAnimationTimersPlayer2;
            float[] scales = playerId == 1 ? _numberAnimationScalesPlayer1 : _numberAnimationScalesPlayer2;

            for (int i = 0; i < 8; i++)
            {
                if (timers[i] > 0)
                {
                    timers[i] -= deltaTime;
                    if (timers[i] <= 0)
                    {
                        timers[i] = 0;
                        scales[i] = 1f;
                    }
                    else
                    {
                        float t = (AnimationDuration - timers[i]) / AnimationDuration;
                        scales[i] = 1f + (float)Math.Sin(t * Math.PI * 2) * 0.2f;
                    }
                }
            }
        }
    }
}