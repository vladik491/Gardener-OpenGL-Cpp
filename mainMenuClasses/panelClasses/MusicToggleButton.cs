namespace Gardener.mainMenuClasses.panelClasses
{
    // Кнопка переключения музыки
    public class MusicToggleButton : MenuButton
    {
        private readonly BackgroundMusicPlayer musicPlayer;

        // Свойство: Доступ к кнопке
        public Button Button => base.button;

        // Конструктор: Инициализирует кнопку
        public MusicToggleButton(Button button, BackgroundMusicPlayer musicPlayer, ButtonSoundPlayer soundPlayer = null)
            : base(button, soundPlayer)
        {
            this.musicPlayer = musicPlayer;
            UpdateButtonImage();
        }

        // Обработчик клика: Переключает музыку и сохраняет настройки
        protected override void OnClick(object sender, EventArgs e)
        {
            base.OnClick(sender, e);
            musicPlayer.IsEnabled = !musicPlayer.IsEnabled;
            Settings.Default.IsMusicEnabled = musicPlayer.IsEnabled;
            Settings.Default.Save();
            UpdateButtonImage();
        }

        // Обновляет изображение кнопки в зависимости от состояния музыки
        private void UpdateButtonImage()
        {
            button.BackgroundImage = musicPlayer.IsEnabled ? Properties.Resources.musicOnn : Properties.Resources.musicOff;
            button.Invalidate();
            button.Refresh();
        }
    }
}