namespace Gardener.mainMenuClasses.panelClasses
{
    // Кнопка для перехода к панели игры
    public class PlayButton : MenuButton
    {
        private PlayPanel _panel; 
        private readonly PanelManager _panelManager; 

        // Конструктор: Инициализирует кнопку
        public PlayButton(Button button, PlayPanel panel, PanelManager panelManager, ButtonSoundPlayer soundPlayer = null)
            : base(button, soundPlayer)
        {
            _panel = panel;
            _panelManager = panelManager;
        }

        // Устанавливает панель игры
        public void SetPanel(PlayPanel panel)
        {
            _panel = panel;
        }

        // Обработчик клика: Обновляет панель и показывает её
        protected override void OnClick(object sender, EventArgs e)
        {
            base.OnClick(sender, e);
            ((MainForm)button.FindForm()).UpdatePlayButtonPanel();
            _panelManager.HideAllPanels();
            _panel.Show();
        }
    }
}