using System.Drawing.Text;
using Gardener.mainMenuClasses;
using Gardener.mainMenuClasses.panelClasses;

namespace Gardener
{
    // Главная форма приложения
    public partial class MainForm : Form
    {
        // Блок: Кнопки и панели
        private CharacterSelectButton _characterSelectButton;
        private PlayButton _playButton; 
        private SettingsButton _settingsButton;
        private ExitButton _exitButton; 
        private SettingsPanel _settingsPanel; 
        private CharacterSelectPanel _characterSelectPanel;
        private PlayPanel _playPanel;
        private PanelManager _panelManager;

        // Блок: Зависимости
        private BackgroundMusicPlayer _musicPlayer;
        private ButtonSoundPlayer _soundPlayer;
        private PrivateFontCollection _fontCollection; 

        // Блок: Состояние
        private int _selectedCharacter;
        private bool _isMusicEnabled;
        private int _selectedCharacterPlayer1; 
        private int _selectedCharacterPlayer2; 

        // Блок: Ресурсы
        private Image _musicOnn;
        private Image _musicOff; 
        private Image _audioOnn;
        private Image _audioOff; 
        private Image _closeButtonImage;
        private Image _jewCharacterImage; 
        private Image _blackCharacterImage; 
        private Image _japanCharacterImage;
        private Image _selectedImage; 
        private Image _chooseImage;

        // Свойство: Выбранный персонаж игрока 1
        public int SelectedCharacterPlayer1 => _selectedCharacterPlayer1;

        // Свойство: Выбранный персонаж игрока 2
        public int SelectedCharacterPlayer2 => _selectedCharacterPlayer2;

        // Конструктор: Инициализирует главную форму
        public MainForm()
        {
            InitializeComponent();

            // Настройка формы
            this.ClientSize = new Size(1600, 900);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.CenterToScreen();

            // Установка фона
            this.BackgroundImage = Image.FromFile("Assets/GM/background.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;

            // Включение двойной буферизации
            this.DoubleBuffered = true;

            // Инициализация шрифтов
            _fontCollection = new PrivateFontCollection();
            _fontCollection.AddFontFile("Monocraft1.ttf");

            // Инициализация проигрывателей
            _musicPlayer = new BackgroundMusicPlayer("Assets/GM/MusicAudio/musicGM.mp3");
            _soundPlayer = new ButtonSoundPlayer("Assets/GM/MusicAudio/button.wav");

            // Инициализация менеджера панелей
            _panelManager = new PanelManager();

            // Загружаем сохранённые настройки для двух игроков
            _selectedCharacterPlayer1 = Settings.Default.SelectedCharacterPlayer1;
            _selectedCharacterPlayer2 = Settings.Default.SelectedCharacterPlayer2;
            if (_selectedCharacterPlayer1 == 0)
            {
                _selectedCharacterPlayer1 = 1;
                Settings.Default.SelectedCharacterPlayer1 = 1;
            }
            if (_selectedCharacterPlayer2 == 0)
            {
                _selectedCharacterPlayer2 = 2;
                Settings.Default.SelectedCharacterPlayer2 = 2;
            }
            Settings.Default.Save();

            // Асинхронная инициализация ресурсов
            InitializeResourcesAsync();

            this.Shown += MainForm_Shown;
        }

        // Обработчик события показа формы: Запускает музыку
        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (_isMusicEnabled)
            {
                _musicPlayer.Play();
            }
            else
            {
                _musicPlayer.Stop();
            }
        }

        // Включает двойную буферизацию для элемента управления и его дочерних элементов
        private void EnableDoubleBuffering(Control control)
        {
            control.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(control, true);
            foreach (Control child in control.Controls)
            {
                EnableDoubleBuffering(child);
            }
        }

        // Асинхронно инициализирует ресурсы
        private async void InitializeResourcesAsync()
        {
            await Task.Run(() =>
            {
                _musicOnn = Properties.Resources.musicOnn;
                _musicOff = Properties.Resources.musicOff;
                _audioOnn = Properties.Resources.audioOnn;
                _audioOff = Properties.Resources.audioOff;
                _closeButtonImage = Properties.Resources.krest14;
                _jewCharacterImage = Properties.Resources.jewCharacter;
                _blackCharacterImage = Properties.Resources.blackCharacter;
                _japanCharacterImage = Properties.Resources.japanCharacter;
                _selectedImage = Properties.Resources.selected;
                _chooseImage = Properties.Resources.choose;
            });

            this.BeginInvoke(new Action(() =>
            {
                EnableDoubleBuffering(this);

                // Установка шрифтов для меток
                musicLabel.Font = new Font(_fontCollection.Families[0], 72F, FontStyle.Bold, GraphicsUnit.Point, 0);
                soundLabel.Font = new Font(_fontCollection.Families[0], 72F, FontStyle.Bold, GraphicsUnit.Point, 0);
                controlLabel.Font = new Font(_fontCollection.Families[0], 72F, FontStyle.Bold, GraphicsUnit.Point, 0);
                player1Label.Font = new Font(_fontCollection.Families[0], 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
                player2Label.Font = new Font(_fontCollection.Families[0], 20F, FontStyle.Bold, GraphicsUnit.Point, 0);

                // Инициализация настроек при первом запуске
                if (Settings.Default.IsFirstRun)
                {
                    Settings.Default.IsMusicEnabled = true;
                    Settings.Default.IsSoundEnabled = true;
                    Settings.Default.IsFirstRun = false;
                    Settings.Default.Save();
                }

                // Применение сохранённых настроек
                _isMusicEnabled = Settings.Default.IsMusicEnabled;
                _musicPlayer.IsEnabled = _isMusicEnabled;
                _soundPlayer.IsEnabled = Settings.Default.IsSoundEnabled;

                // Установка изображений для персонажей и кнопок
                character1PictureBox.Image = _jewCharacterImage;
                character2PictureBox.Image = _blackCharacterImage;
                character3PictureBox.Image = _japanCharacterImage;
                characterCloseButton.BackgroundImage = _closeButtonImage;
                playCloseButton.BackgroundImage = _closeButtonImage;

                // Инициализация кнопок
                _characterSelectButton = new CharacterSelectButton(characterIconButton, null, _panelManager, _soundPlayer);
                _playButton = new PlayButton(playButton, null, _panelManager, _soundPlayer);
                _settingsButton = new SettingsButton(settingsButton, null, _panelManager, _soundPlayer);
                _exitButton = new ExitButton(exitButton, _panelManager, _soundPlayer);

                // Привязка событий к кнопкам
                _characterSelectButton.AttachEvents();
                _playButton.AttachEvents();
                _settingsButton.AttachEvents();
                _exitButton.AttachEvents();
            }));
        }

        // Инициализирует панель настроек
        private void InitializeSettingsPanel()
        {
            if (_settingsPanel == null)
            {
                _settingsPanel = new SettingsPanel(
                    settingsPanel,
                    closeButton,
                    musicToggleButton,
                    soundToggleButton,
                    musicLabel,
                    soundLabel,
                    _musicPlayer,
                    _soundPlayer
                );
                _panelManager.RegisterPanel(_settingsPanel);
                _settingsPanel.Show();
                _settingsPanel.Hide();
            }
        }

        // Инициализирует панель выбора персонажа
        private void InitializeCharacterSelectPanel()
        {
            if (_characterSelectPanel == null)
            {
                _characterSelectPanel = new CharacterSelectPanel(
                    characterSelectPanel,
                    characterCloseButton,
                    character1PictureBox,
                    character2PictureBox,
                    character3PictureBox,
                    selectButton1,
                    selectButton2,
                    selectButton3,
                    playerIndicator1,
                    playerIndicator2,
                    playerIndicator3,
                    playerSelectPanel,
                    player1Button,
                    player2Button,
                    _soundPlayer,
                    UpdateSelectedCharacter
                );
                _panelManager.RegisterPanel(_characterSelectPanel);
                _characterSelectPanel.UpdateButtons(_selectedCharacterPlayer1, _selectedCharacterPlayer2);
                _characterSelectPanel.Show();
                _characterSelectPanel.Hide();
            }
        }

        // Инициализирует панель игры
        private void InitializePlayPanel()
        {
            if (_playPanel == null)
            {
                _playPanel = new PlayPanel(
                    playPanel,
                    playCloseButton,
                    controlLabel,
                    player1Label,
                    player2Label,
                    startGameButton,
                    runningCharacterGif,
                    staticImage,
                    _soundPlayer
                );
                _panelManager.RegisterPanel(_playPanel);
                _playPanel.Show();
                _playPanel.Hide();
            }
        }

        // Обновляет панель выбора персонажа для кнопки
        public void UpdateCharacterSelectButtonPanel()
        {
            InitializeCharacterSelectPanel();
            _characterSelectButton.SetPanel(_characterSelectPanel);
        }

        // Обновляет панель игры для кнопки
        public void UpdatePlayButtonPanel()
        {
            InitializePlayPanel();
            _playButton.SetPanel(_playPanel);
        }

        // Обновляет панель настроек для кнопки
        public void UpdateSettingsButtonPanel()
        {
            InitializeSettingsPanel();
            _settingsButton.SetPanel(_settingsPanel);
        }

        // Обновляет выбор персонажа
        private void UpdateSelectedCharacter(int characterIndex, int playerIndex)
        {
            int previousCharacterPlayer1 = _selectedCharacterPlayer1;
            int previousCharacterPlayer2 = _selectedCharacterPlayer2;

            if (playerIndex == 1)
            {
                if (characterIndex == _selectedCharacterPlayer2)
                    return;

                _selectedCharacterPlayer1 = characterIndex;
                Settings.Default.SelectedCharacterPlayer1 = characterIndex;
            }
            else if (playerIndex == 2)
            {
                if (characterIndex == _selectedCharacterPlayer1)
                    return;

                _selectedCharacterPlayer2 = characterIndex;
                Settings.Default.SelectedCharacterPlayer2 = characterIndex;
            }

            Settings.Default.Save();

            if (_characterSelectPanel != null)
            {
                _characterSelectPanel.UpdateButtons(_selectedCharacterPlayer1, _selectedCharacterPlayer2);
            }
        }

        // Останавливает все процессы
        public void StopAllProcesses()
        {
            try
            {
                _musicPlayer?.Stop();
                _soundPlayer?.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при остановке процессов: {ex.Message}\n\nStackTrace: {ex.StackTrace}");
            }
        }

        // Возобновляет музыку
        public void ResumeMusic()
        {
            if (_isMusicEnabled)
            {
                _musicPlayer.Play();
            }
        }

        // Сбрасывает состояние в главное меню
        public void ResetToMainMenu()
        {
            _panelManager.HideAllPanels();
            _isMusicEnabled = Settings.Default.IsMusicEnabled;

            if (_isMusicEnabled)
            {
                _musicPlayer.Play();
            }
        }

        // Обработчик закрытия формы: Очищает ресурсы
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _musicPlayer?.Stop();
                _musicPlayer?.Dispose();
                _soundPlayer?.Dispose();
                _fontCollection?.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при закрытии MainForm: {ex.Message}\n\nStackTrace: {ex.StackTrace}");
            }
        }
    }
}