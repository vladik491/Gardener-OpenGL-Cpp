using NAudio.Wave;

namespace Gardener.gameClasses.Audio
{
    // Управляет звуками и музыкой в игре
    public class AudioManager : IDisposable
    {
        // Блок: Звуковые ресурсы
        private readonly Dictionary<string, WaveOutEvent> _audioPlayers; 
        private readonly Dictionary<string, AudioFileReader> _audioFiles; 
        private readonly Dictionary<string, float> _volumes;
        public event EventHandler VictorySoundFinished;

        // Блок: Состояния
        private bool _disposing = false; 
        private bool _isMusicEnabled = true; 
        private bool _isSoundEnabled = true;
        private bool _isVictoryPlaying = false;

        /// <summary>
        /// Получает или устанавливает, включена ли музыка.
        /// </summary>
        public bool IsMusicEnabled
        {
            get => _isMusicEnabled;
            set => _isMusicEnabled = value;
        }
        /// <summary>
        /// Так же, только со звуком
        /// </summary>
        public bool IsSoundEnabled
        {
            get => _isSoundEnabled;
            set => _isSoundEnabled = value;
        }

        // Конструктор: инициализирует звуковые ресурсы
        public AudioManager()
        {
            _audioPlayers = new Dictionary<string, WaveOutEvent>();
            _audioFiles = new Dictionary<string, AudioFileReader>();
            _volumes = new Dictionary<string, float>();

            _volumes["backgroundMusic"] = 0.35f;
            _volumes["soundError"] = 0.63f;
            _volumes["soundFailure"] = 0.63f;
            _volumes["soundLeica"] = 1f;
            _volumes["soundOfMud"] = 0.6f;
            _volumes["soundSpray"] = 1f;
            _volumes["theSoundOfAPrick"] = 1f;
            _volumes["theSoundOfFruitBeingLifted"] = 1f;
            _volumes["button"] = 1f;
            _volumes["soundOfVictory"] = 1f;
            _volumes["soundOpening"] = 1f;
            _volumes["start"] = 1f;
            _volumes["credits"] = 1f;

            LoadAudio("backgroundMusic", "Assets/Game/musicAndSound/backgroundMusic.mp3", loop: true);
            LoadAudio("soundError", "Assets/Game/musicAndSound/soundError.mp3");
            LoadAudio("soundFailure", "Assets/Game/musicAndSound/soundOfFailure.wav");
            LoadAudio("soundLeica", "Assets/Game/musicAndSound/soundLeica.wav");
            LoadAudio("soundOfMud", "Assets/Game/musicAndSound/soundOfMud.wav");
            LoadAudio("soundSpray", "Assets/Game/musicAndSound/soundSpray.wav");
            LoadAudio("theSoundOfAPrick", "Assets/Game/musicAndSound/theSoundOfAPrick.wav");
            LoadAudio("theSoundOfFruitBeingLifted", "Assets/Game/musicAndSound/theSoundOfFruitBeingLifted.wav");
            LoadAudio("button", "Assets/Game/musicAndSound/button.wav");
            LoadAudio("soundOfVictory", "Assets/Game/musicAndSound/soundOfVictory.mp3");
            LoadAudio("soundOpening", "Assets/Game/musicAndSound/soundOpening.wav");
            LoadAudio("start", "Assets/Game/musicAndSound/start.wav");
            LoadAudio("credits", "Assets/Game/musicAndSound/musicForCredits.mp3", loop: true);
        }

        protected virtual void OnVictorySoundFinished()
        {
            VictorySoundFinished?.Invoke(this, EventArgs.Empty);
        }

        // Перезапускает зацикленные звуки
        private void RestartLoop(object sender, StoppedEventArgs e)
        {
            if (!_disposing)
            {
                var player = (WaveOutEvent)sender;
                if (player.PlaybackState == PlaybackState.Stopped)
                {
                    var key = player == _audioPlayers["backgroundMusic"] ? "backgroundMusic" : "credits";
                    var reader = _audioFiles[key];
                    reader.Position = 0;
                    player.Play();
                }
            }
        }

        // Загружает аудиофайл
        private void LoadAudio(string key, string path, bool loop = false)
        {
            try
            {
                var reader = new AudioFileReader(path)
                {
                    Volume = _volumes.ContainsKey(key) ? _volumes[key] : 1f
                };
                var waveOut = new WaveOutEvent();
                waveOut.Init(reader);

                if (loop)
                    waveOut.PlaybackStopped += RestartLoop;

                _audioFiles[key] = reader;
                _audioPlayers[key] = waveOut;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AudioManager] Ошибка загрузки '{key}': {ex.Message}");
            }
        }

        /// <summary>
        /// Воспроизводит звук по ключу
        /// </summary>
        /// <param name="key">Ключ звука</param>
        public void Play(string key)
        {
            if (!_audioPlayers.ContainsKey(key))
            {
                Console.WriteLine($"[AudioManager] Ключ '{key}' не найден!");
                return;
            }

            bool isMusic = key == "backgroundMusic" || key == "soundOfVictory" || key == "start" || key == "credits";
            if (isMusic && !_isMusicEnabled) return;
            if (!isMusic && !_isSoundEnabled) return;

            var player = _audioPlayers[key];
            var reader = _audioFiles[key];

            if (key == "backgroundMusic" || key == "credits")
            {
                if (player.PlaybackState == PlaybackState.Stopped)
                {
                    reader.Position = 0;
                    player.Play();
                }
            }
            else
            {
                reader.Position = 0;
                player.Stop();
                player.Play();
                if (key == "soundOfVictory")
                    _isVictoryPlaying = true;
            }
        }

        // Проигрывает последовательность победы
        public void PlayVictorySequence()
        {
            if (_audioPlayers.TryGetValue("backgroundMusic", out var bgPlayer))
            {
                bgPlayer.Stop();
                if (_audioFiles.TryGetValue("backgroundMusic", out var bgReader))
                    bgReader.Position = 0;
                bgPlayer.PlaybackStopped -= RestartLoop;
            }

            Play("soundOfVictory");

            if (_audioPlayers.TryGetValue("soundOfVictory", out var victoryPlayer))
            {
                EventHandler<StoppedEventArgs> handler = null;
                handler = (s, e) =>
                {
                    victoryPlayer.PlaybackStopped -= handler;
                    _isVictoryPlaying = false;
                    if (!_disposing)
                    {
                        OnVictorySoundFinished(); // Уведомляем о завершении
                    }
                };
                victoryPlayer.PlaybackStopped += handler;
            }
        }

        // Останавливает звук
        public void Stop(string key)
        {
            if (key == "soundOfVictory" && _isVictoryPlaying) return;

            if (_audioPlayers.TryGetValue(key, out var player))
                player.Stop();
        }

        // Останавливает все звуки
        public void StopAllSounds()
        {
            foreach (var kvp in _audioPlayers)
            {
                if (kvp.Key == "soundOfVictory" && _isVictoryPlaying) continue;
                kvp.Value.Stop();
            }
        }

        // Получает плеер по ключу
        public bool GetPlayer(string key, out WaveOutEvent player)
        {
            return _audioPlayers.TryGetValue(key, out player);
        }

        // Устанавливает громкость
        public void SetVolume(string key, float volume)
        {
            if (!_audioFiles.ContainsKey(key)) return;

            float clamped = Math.Clamp(volume, 0f, 1f);
            _volumes[key] = clamped;
            _audioFiles[key].Volume = clamped;
        }

        // Проверяет, играет ли звук
        public bool IsPlaying(string key)
        {
            if (_audioPlayers.TryGetValue(key, out var player))
                return player.PlaybackState == PlaybackState.Playing;
            return false;
        }

        // Освобождает ресурсы
        public void Dispose()
        {
            _disposing = true;
            foreach (var player in _audioPlayers.Values)
            {
                player.Stop();
                player.Dispose();
            }
            foreach (var reader in _audioFiles.Values)
                reader.Dispose();
            _audioPlayers.Clear();
            _audioFiles.Clear();
            _volumes.Clear();
        }
    }
}