namespace Gardener.mainMenuClasses
{
    // Базовый класс для кнопок меню с анимацией нажатия
    public class MenuButton
    {
        // Блок: Зависимости и состояние
        protected readonly Button button; 
        protected readonly ButtonSoundPlayer soundPlayer; 
        protected Size originalSize; 
        protected Point originalLocation; 

        // Событие клика по кнопке
        public event EventHandler Click;

        // Конструктор: Инициализирует кнопку
        public MenuButton(Button button, ButtonSoundPlayer soundPlayer = null)
        {
            this.button = button;
            this.soundPlayer = soundPlayer;
            button.BackColor = Color.Transparent;
            button.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button.FlatAppearance.MouseOverBackColor = Color.Transparent;

            this.originalSize = button.Size;
            this.originalLocation = button.Location;
        }

        // Подключает обработчики событий к кнопке
        public void AttachEvents()
        {
            button.MouseDown += OnMouseDown;
            button.MouseUp += OnMouseUp;
            button.Click += OnClick;
        }

        // Обработчик нажатия мыши: Уменьшает размер кнопки
        protected virtual void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Уменьшаем размер кнопки на 5%
                int newWidth = (int)(originalSize.Width * 0.95f);
                int newHeight = (int)(originalSize.Height * 0.95f);
                button.Size = new Size(newWidth, newHeight);

                // Центрируем кнопку
                button.Location = new Point(
                    originalLocation.X + (originalSize.Width - newWidth) / 2,
                    originalLocation.Y + (originalSize.Height - newHeight) / 2
                );
            }
        }

        // Обработчик отпускания мыши: Восстанавливает размер и вызывает событие клика
        protected virtual void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Восстанавливаем размер
                button.Size = originalSize;
                button.Location = originalLocation;

                // Вызываем событие Click
                Click?.Invoke(this, EventArgs.Empty);
            }
        }

        // Обработчик клика: Проигрывает звук
        protected virtual void OnClick(object sender, EventArgs e)
        {
            soundPlayer?.Play();
        }
    }
}