namespace Gardener.mainMenuClasses.panelClasses
{
    // Кнопка для перехода к панели настроек
    public class SettingsButton : MenuButton
    {
        private SettingsPanel _panel;
        private readonly PanelManager _panelManager;

        // Конструктор: Инициализирует кнопку
        public SettingsButton(Button button, SettingsPanel panel, PanelManager panelManager, ButtonSoundPlayer soundPlayer = null)
            : base(button, soundPlayer)
        {
            _panel = panel;
            _panelManager = panelManager;
        }

        // Устанавливает панель настроек
        public void SetPanel(SettingsPanel panel)
        {
            _panel = panel;
        }

        // Обработчик клика: Обновляет панель и показывает её
        protected override void OnClick(object sender, EventArgs e)
        {
            base.OnClick(sender, e);
            ((MainForm)button.FindForm()).UpdateSettingsButtonPanel();
            _panelManager.HideAllPanels();
            _panel.Show();
        }
    }
}