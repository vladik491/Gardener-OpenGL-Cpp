using System.Media;

namespace Gardener.mainMenuClasses
{
    // Проигрыватель звука для кнопок
    public class ButtonSoundPlayer : IDisposable
    {
        private readonly SoundPlayer _soundPlayer; 
        public bool IsEnabled { get; set; } = true;

        // Конструктор: Инициализирует проигрыватель
        public ButtonSoundPlayer(string soundFilePath)
        {
            if (string.IsNullOrEmpty(soundFilePath))
                throw new ArgumentNullException(nameof(soundFilePath));

            _soundPlayer = new SoundPlayer(soundFilePath);
        }

        // Воспроизводит звук
        public void Play()
        {
            if (IsEnabled)
            {
                _soundPlayer.Play();
            }
        }

        // Останавливает звук
        public void Stop()
        {
            _soundPlayer.Stop();
        }

        // Утилизирует ресурсы
        public void Dispose()
        {
            _soundPlayer?.Dispose();
        }
    }
}