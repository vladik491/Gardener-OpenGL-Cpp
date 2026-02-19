using WMPLib;

namespace Gardener.mainMenuClasses
{
    // Проигрыватель фоновой музыки
    public class BackgroundMusicPlayer : IDisposable
    {
        // Блок: Зависимости и состояние
        private WindowsMediaPlayer _player; // Плеер Windows Media
        private double _pausedPosition; // Позиция при паузе (в секундах)
        private bool _isEnabled;
        private readonly string _musicFilePath; 
        private bool _isInitialized; // Флаг инициализации плеера

        // Конструктор: Инициализирует проигрыватель музыки
        public BackgroundMusicPlayer(string musicFilePath)
        {
            if (string.IsNullOrEmpty(musicFilePath))
                throw new ArgumentNullException(nameof(musicFilePath));

            _musicFilePath = musicFilePath;
            _pausedPosition = 0;
            _isEnabled = true;
            _isInitialized = false;
        }

        // Инициализирует плеер
        private void InitializePlayer()
        {
            if (!_isInitialized)
            {
                _player = new WindowsMediaPlayer();
                _player.URL = _musicFilePath;
                _player.settings.setMode("loop", true);
                _player.settings.volume = 20;
                _isInitialized = true;
            }
        }

        // Свойство: Включён ли проигрыватель
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                if (_isEnabled)
                {
                    Play();
                }
                else
                {
                    Pause();
                }
            }
        }

        // Воспроизводит музыку
        public void Play()
        {
            if (_isEnabled)
            {
                InitializePlayer();
                if (_pausedPosition > 0)
                {
                    _player.controls.currentPosition = _pausedPosition;
                }
                _player.controls.play();
            }
        }

        // Приостанавливает музыку
        public void Pause()
        {
            if (_isInitialized && _player.playState == WMPPlayState.wmppsPlaying)
            {
                _pausedPosition = _player.controls.currentPosition;
                _player.controls.pause();
            }
        }

        // Останавливает музыку
        public void Stop()
        {
            if (_isInitialized)
            {
                _player.controls.stop();
                _pausedPosition = 0;
            }
        }

        // Сбрасывает позицию воспроизведения
        public void ResetPosition()
        {
            _pausedPosition = 0;
        }

        // Устанавливает громкость
        public void SetVolume(int volume)
        {
            if (volume < 0 || volume > 100)
                throw new ArgumentOutOfRangeException(nameof(volume), "Громкость должна быть в диапазоне от 0 до 100.");
            if (_isInitialized)
            {
                _player.settings.volume = volume;
            }
        }

        // Утилизирует ресурсы
        public void Dispose()
        {
            if (_isInitialized)
            {
                _player?.close();
            }
        }
    }
}