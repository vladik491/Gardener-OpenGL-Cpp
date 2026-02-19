namespace Gardener.mainMenuClasses.panelClasses
{
    // Кнопка переключения звука
    public class SoundToggleButton : MenuButton
    {
        private readonly ButtonSoundPlayer soundPlayer;

        // Свойство: Доступ к кнопке
        public Button Button => base.button;

        // Конструктор: Инициализирует кнопку
        public SoundToggleButton(Button button, ButtonSoundPlayer soundPlayer = null)
            : base(button, soundPlayer)
        {
            this.soundPlayer = soundPlayer;
            UpdateButtonImage();
        }

        // Обработчик клика: Переключает звук и сохраняет настройки
        protected override void OnClick(object sender, EventArgs e)
        {
            base.OnClick(sender, e);
            soundPlayer.IsEnabled = !soundPlayer.IsEnabled;
            Settings.Default.IsSoundEnabled = soundPlayer.IsEnabled;
            Task.Run(() => Settings.Default.Save());
            UpdateButtonImage();
        }

        // Обновляет изображение кнопки в зависимости от состояния звука
        private void UpdateButtonImage()
        {
            button.BackgroundImage = soundPlayer.IsEnabled ? Properties.Resources.audioOnn : Properties.Resources.audioOff;
            button.Invalidate();
            button.Refresh();
        }
    }
}