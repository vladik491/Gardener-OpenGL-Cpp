namespace Gardener.mainMenuClasses.panelClasses
{
    // Панель настроек игры
    public class SettingsPanel : IPanel
    {
        // Блок: Элементы управления
        private readonly Panel panel; 
        private readonly CloseButton closeButton; 
        private readonly MusicToggleButton musicToggleButton; 
        private readonly SoundToggleButton soundToggleButton; 
        private readonly Label musicLabel; 
        private readonly Label soundLabel; 

        // Блок: Зависимости
        private readonly BackgroundMusicPlayer musicPlayer; 
        private readonly ButtonSoundPlayer soundPlayer; 

        // Конструктор: Инициализирует панель настроек
        public SettingsPanel(
            Panel panel,
            Button closeButton,
            Button musicToggleButton,
            Button soundToggleButton,
            Label musicLabel,
            Label soundLabel,
            BackgroundMusicPlayer musicPlayer,
            ButtonSoundPlayer soundPlayer)
        {
            this.panel = panel;
            this.musicLabel = musicLabel;
            this.soundLabel = soundLabel;
            this.musicPlayer = musicPlayer;
            this.soundPlayer = soundPlayer;

            // Настройка кнопок
            musicToggleButton.BackColor = Color.Transparent;
            musicToggleButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
            musicToggleButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
            soundToggleButton.BackColor = Color.Transparent;
            soundToggleButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
            soundToggleButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
            musicToggleButton.Visible = true;
            soundToggleButton.Visible = true;
            musicLabel.Visible = true;
            soundLabel.Visible = true;

            this.closeButton = new CloseButton(closeButton, panel, soundPlayer);
            this.musicToggleButton = new MusicToggleButton(musicToggleButton, musicPlayer, soundPlayer);
            this.soundToggleButton = new SoundToggleButton(soundToggleButton, soundPlayer);

            this.closeButton.AttachEvents();
            this.musicToggleButton.AttachEvents();
            this.soundToggleButton.AttachEvents();

            // Устанавливаем начальную видимость
            this.musicToggleButton.Button.Visible = true;
            this.soundToggleButton.Button.Visible = true;
        }

        public bool IsVisible => panel.Visible;

        // Показывает панель
        public void Show()
        {
            panel.Visible = true;
            musicToggleButton.Button.Visible = true;
            soundToggleButton.Button.Visible = true;
            musicLabel.Visible = true;
            soundLabel.Visible = true;
            closeButton.Button.Visible = true;
        }

        // Скрывает панель
        public void Hide()
        {
            panel.Visible = false;
        }
    }
}