namespace Gardener.mainMenuClasses.panelClasses
{
    // Кнопка запуска игры
    public class StartGameButton
    {
        // Блок: Элементы и зависимости
        public readonly Button button; 
        private readonly Action onClick; 
        private readonly ButtonSoundPlayer soundPlayer; 

        // Блок: Исходные параметры
        private readonly Size originalSize; 
        private readonly Point originalLocation; 

        // Конструктор: Инициализирует кнопку
        public StartGameButton(Button button, Action onClick, ButtonSoundPlayer soundPlayer)
        {
            this.button = button;
            this.onClick = onClick;
            this.soundPlayer = soundPlayer;
            this.originalSize = button.Size;
            this.originalLocation = button.Location;
        }

        // Подключает обработчики событий
        public void AttachEvents()
        {
            button.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    // Уменьшаем размер кнопки на 5%
                    int newWidth = (int)(originalSize.Width * 0.95);
                    int newHeight = (int)(originalSize.Height * 0.95);
                    button.Size = new Size(newWidth, newHeight);

                    // Центрируем кнопку
                    int deltaX = (originalSize.Width - newWidth) / 2;
                    int deltaY = (originalSize.Height - newHeight) / 2;
                    button.Location = new Point(originalLocation.X + deltaX, originalLocation.Y + deltaY);
                }
            };
            button.MouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    // Восстанавливаем размер и позицию
                    button.Size = originalSize;
                    button.Location = originalLocation;
                    soundPlayer?.Play();
                    onClick?.Invoke();
                }
            };
        }
    }
}