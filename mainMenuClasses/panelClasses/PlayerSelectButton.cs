namespace Gardener.mainMenuClasses.panelClasses
{
    // Кнопка выбора игрока
    public class PlayerSelectButton
    {
        // Блок: Элементы и зависимости
        public readonly Button button; 
        private readonly int playerIndex; 
        private readonly Action<int> onClick; 
        private readonly ButtonSoundPlayer soundPlayer;

        // Блок: Исходные параметры
        private readonly Size originalSize; 

        // Конструктор: Инициализирует кнопку
        public PlayerSelectButton(Button button, int playerIndex, Action<int> onClick, ButtonSoundPlayer soundPlayer)
        {
            this.button = button;
            this.playerIndex = playerIndex;
            this.onClick = onClick;
            this.soundPlayer = soundPlayer;
            this.originalSize = button.Size;
        }

        // Подключает обработчики событий
        public void AttachEvents()
        {
            button.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    button.Size = new Size((int)(originalSize.Width * 0.95), (int)(originalSize.Height * 0.95));
                }
            };
            button.MouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    button.Size = originalSize;
                    soundPlayer?.Play();
                    onClick?.Invoke(playerIndex);
                }
            };
        }
    }
}