using Gardener.mainMenuClasses.panelClasses;

namespace Gardener.mainMenuClasses
{
    // Интерфейс для панелей главного меню
    public interface IPanel
    {
        // Показывает панель
        void Show();

        // Скрывает панель
        void Hide();

        // Свойство: Видима ли панель
        bool IsVisible { get; }
    }

    // Управляет панелями главного меню
    public class PanelManager
    {
        // Блок: Панели и зависимости
        private readonly List<IPanel> _panels;
        private IPanel _characterSelectPanel; 
        private IPanel _playPanel; 
        private IPanel _settingsPanel;

        // Конструктор: Инициализирует менеджер панелей
        public PanelManager()
        {
            _panels = new List<IPanel>();
        }

        // Регистрирует новую панель
        public void RegisterPanel(IPanel panel)
        {
            if (panel == null) throw new ArgumentNullException(nameof(panel));
            _panels.Add(panel);

            // Сохраняем ссылки на конкретные панели
            if (panel is CharacterSelectPanel)
            {
                _characterSelectPanel = panel;
            }
            else if (panel is PlayPanel)
            {
                _playPanel = panel;
            }
            else if (panel is SettingsPanel)
            {
                _settingsPanel = panel;
            }
        }

        /// <summary>
        /// Показывает указанную панель, скрывая остальные.
        /// </summary>
        /// <param name="panelToShow">Панель для показа</param>
        public void ShowPanel(IPanel panelToShow)
        {
            if (panelToShow == null) throw new ArgumentNullException(nameof(panelToShow));

            // Если открываем CharacterSelectPanel, сначала закрываем PlayPanel и SettingsPanel
            if (panelToShow == _characterSelectPanel)
            {
                if (_playPanel != null && _playPanel.IsVisible)
                {
                    _playPanel.Hide();
                }
                if (_settingsPanel != null && _settingsPanel.IsVisible)
                {
                    _settingsPanel.Hide();
                }
            }

            // Логика показа/скрытия панелей
            foreach (var panel in _panels)
            {
                if (panel == panelToShow)
                {
                    if (!panel.IsVisible)
                    {
                        panel.Show();
                    }
                    else
                    {
                        panel.Hide();
                    }
                }
                else
                {
                    panel.Hide();
                }
            }
        }

        // Скрывает все панели
        public void HideAllPanels()
        {
            foreach (var panel in _panels)
            {
                panel.Hide();
            }
        }
    }
}