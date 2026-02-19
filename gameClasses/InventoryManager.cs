using Gardener.gameClasses.Rendering;
using Gardener.gameClasses.Audio;

namespace Gardener.gameClasses
{
    // Управляет инвентарём игроков в игре
    public class InventoryManager
    {
        // Класс предмета в инвентаре
        public class InventoryItem
        {
            public int Slot { get; }
            public bool IsAvailable { get; set; }
            public int PlayerHolding { get; set; }
            public int TextureId { get; }

            public InventoryItem(int slot, int textureId)
            {
                Slot = slot;
                IsAvailable = true;
                PlayerHolding = 0;
                TextureId = textureId;
            }
        }

        // Блок: Хранилища
        private readonly List<InventoryItem> _items; 
        private readonly int[] _selectedItems; 
        private readonly AudioManager _audioManager; 

        // Блок: Состояния
        private bool _player1InventoryOpen = false; 
        private bool _player2InventoryOpen = false;


        /// <summary>
        /// Инициализирует инвентарь с предметами для ухода за деревьями.
        /// </summary>
        /// <param name="textureRegistry">Регистр текстур для предметов.</param>
        /// <param name="audioManager">Менеджер звуков для воспроизведения эффектов.</param>
        public InventoryManager(TextureRegistry textureRegistry, AudioManager audioManager)
        {
            _audioManager = audioManager;
            _items = new List<InventoryItem>
            {
                new InventoryItem(1, textureRegistry.WateringCanTextureId),
                new InventoryItem(2, textureRegistry.FertilizerLevel1TextureId),
                new InventoryItem(3, textureRegistry.FertilizerLevel2TextureId),
                new InventoryItem(4, textureRegistry.FertilizerLevel3TextureId),
                new InventoryItem(5, textureRegistry.InsectSprayTextureId),
                new InventoryItem(6, textureRegistry.FungusSprayTextureId),
                new InventoryItem(7, textureRegistry.VirusSprayTextureId),
                new InventoryItem(8, textureRegistry.SyringeTextureId)
            };
            _selectedItems = new int[3];
        }

        // Возвращает список предметов
        public List<InventoryItem> GetItems() => _items;

        // Возвращает выбранный предмет игрока
        public int GetSelectedItem(int playerId) => _selectedItems[playerId];

        // Выбирает предмет для игрока
        public void SelectItem(int slot, int playerId)
        {
            var item = _items.Find(i => i.Slot == slot);
            if (item != null && item.IsAvailable)
            {
                item.IsAvailable = false;
                item.PlayerHolding = playerId;
                _selectedItems[playerId] = slot;
            }
        }

        // Возвращает предмет в инвентарь
        public void ReturnItem(int slot, int playerId)
        {
            var item = _items.Find(i => i.Slot == slot);
            if (item != null && item.PlayerHolding == playerId)
            {
                item.IsAvailable = true;
                item.PlayerHolding = 0;
                _selectedItems[playerId] = 0;
                _audioManager.Play("button");
            }
        }

        // Проверяет, можно ли выбрать предмет
        public bool CanSelectItem(int slot, int playerId)
        {
            var item = _items.Find(i => i.Slot == slot);
            return item != null && item.IsAvailable && item.PlayerHolding != (playerId == 1 ? 2 : 1);
        }

        // Открывает инвентарь игрока
        public void OpenInventory(int playerId)
        {
            if (playerId == 1)
                _player1InventoryOpen = true;
            else if (playerId == 2)
                _player2InventoryOpen = true;
        }

        // Закрывает инвентарь игрока
        public void CloseInventory(int playerId)
        {
            if (playerId == 1)
                _player1InventoryOpen = false;
            else if (playerId == 2)
                _player2InventoryOpen = false;
        }

        // Проверяет, открыт ли инвентарь
        public bool IsInventoryOpen(int playerId)
        {
            return playerId == 1 ? _player1InventoryOpen : _player2InventoryOpen;
        }
    }
}