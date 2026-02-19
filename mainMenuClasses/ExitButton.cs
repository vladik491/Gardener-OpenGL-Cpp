namespace Gardener.mainMenuClasses
{
    // Кнопка выхода из приложения
    public class ExitButton : MenuButton
    {
        private readonly PanelManager panelManager; // Менеджер панелей

        // Конструктор: Инициализирует кнопку выхода
        public ExitButton(Button button, PanelManager panelManager, ButtonSoundPlayer soundPlayer = null)
            : base(button, soundPlayer)
        {
            this.panelManager = panelManager;
        }

        // Обработчик клика: Скрывает панели и закрывает приложение
        protected override void OnClick(object sender, EventArgs e)
        {
            base.OnClick(sender, e);
            if (panelManager != null)
            {
                panelManager.HideAllPanels();
            }
            Application.Exit();
        }
    }
}