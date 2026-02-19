namespace Gardener.mainMenuClasses.panelClasses
{
    // Кнопка выбора персонажа
    public class SelectButton : MenuButton
    {
        private readonly Action<int> _selectAction; 
        private readonly int _characterIndex; 

        // Свойство: Доступ к кнопке
        public Button button => base.button;

        public bool Visible { get; internal set; }

        // Свойство: Фоновое изображение кнопки
        public Image BackgroundImage
        {
            get => button.BackgroundImage;
            set => button.BackgroundImage = value;
        }

        // Конструктор: Инициализирует кнопку выбора
        public SelectButton(Button button, int characterIndex, Action<int> selectAction, ButtonSoundPlayer soundPlayer = null)
            : base(button, soundPlayer)
        {
            _characterIndex = characterIndex;
            _selectAction = selectAction ?? throw new ArgumentNullException(nameof(selectAction));
        }

        // Обработчик клика: Вызывает действие выбора персонажа
        protected override void OnClick(object sender, EventArgs e)
        {
            base.OnClick(sender, e);
            _selectAction(_characterIndex);
        }
    }
}