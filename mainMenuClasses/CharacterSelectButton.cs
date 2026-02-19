namespace Gardener.mainMenuClasses.panelClasses
{
    // Кнопка для перехода к панели выбора персонажа
    public class CharacterSelectButton : MenuButton
    {
        private CharacterSelectPanel _panel; 
        private readonly PanelManager _panelManager; 

        // Конструктор: Инициализирует кнопку
        public CharacterSelectButton(Button button, CharacterSelectPanel panel, PanelManager panelManager, ButtonSoundPlayer soundPlayer = null)
            : base(button, soundPlayer)
        {
            _panel = panel;
            _panelManager = panelManager;
        }

        // Устанавливает панель выбора персонажа
        public void SetPanel(CharacterSelectPanel panel)
        {
            _panel = panel;
        }

        // Обработчик клика: Переключает видимость панели
        protected override void OnClick(object sender, EventArgs e)
        {
            base.OnClick(sender, e);
            ((MainForm)button.FindForm()).UpdateCharacterSelectButtonPanel();

            // Переключаем видимость панели
            if (_panel.IsVisible)
            {
                _panel.Hide();
            }
            else
            {
                _panelManager.HideAllPanels();
                _panel.Show();
            }
        }
    }
}