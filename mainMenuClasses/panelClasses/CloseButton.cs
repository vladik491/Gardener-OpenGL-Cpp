namespace Gardener.mainMenuClasses.panelClasses
{
    // Кнопка закрытия панели
    public class CloseButton : MenuButton
    {
        private readonly Panel panel;

        // Свойство: Доступ к кнопке
        public Button Button => button;

        public bool Visible { get; internal set; }

        // Конструктор: Инициализирует кнопку закрытия
        public CloseButton(Button button, Panel panel, ButtonSoundPlayer soundPlayer = null)
            : base(button, soundPlayer)
        {
            this.panel = panel;
        }

        // Обработчик клика: Скрывает панель
        protected override void OnClick(object sender, EventArgs e)
        {
            base.OnClick(sender, e);
            panel.Visible = false;
        }
    }
}