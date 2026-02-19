namespace Gardener.mainMenuClasses.panelClasses
{
    // Панель для подготовки к игре
    public class PlayPanel : IPanel
    {
        // Блок: Элементы управления
        private readonly Panel panel; 
        private readonly CloseButton closeButton; 
        private readonly Label controlLabel; 
        private readonly Label player1Label; 
        private readonly Label player2Label; 
        private readonly StartGameButton startGameButton; 
        private readonly PictureBox runningCharacterGif; 
        private readonly PictureBox staticImage; 

        // Блок: Зависимости
        private readonly ButtonSoundPlayer soundPlayer; 

        // Конструктор: Инициализирует панель
        public PlayPanel(
            Panel panel,
            Button closeButton,
            Label controlLabel,
            Label player1Label,
            Label player2Label,
            Button startGameButton,
            PictureBox runningCharacterGif,
            PictureBox staticImage,
            ButtonSoundPlayer soundPlayer)
        {
            this.panel = panel;
            this.closeButton = new CloseButton(closeButton, panel, soundPlayer);
            this.controlLabel = controlLabel;
            this.player1Label = player1Label;
            this.player2Label = player2Label;
            this.startGameButton = new StartGameButton(startGameButton, StartGame, soundPlayer);
            this.runningCharacterGif = runningCharacterGif;
            this.staticImage = staticImage;
            this.soundPlayer = soundPlayer;

            // Настраиваем прозрачность
            this.controlLabel.BackColor = Color.Transparent;
            this.player1Label.BackColor = Color.Transparent;
            this.player2Label.BackColor = Color.Transparent;
            this.startGameButton.button.BackColor = Color.Transparent;
            this.startGameButton.button.FlatAppearance.MouseDownBackColor = Color.Transparent;
            this.startGameButton.button.FlatAppearance.MouseOverBackColor = Color.Transparent;
            this.runningCharacterGif.BackColor = Color.Transparent;
            this.staticImage.BackColor = Color.Transparent;

            // Убедимся, что элементы видны
            this.controlLabel.Visible = true;
            this.player1Label.Visible = true;
            this.player2Label.Visible = true;
            this.startGameButton.button.Visible = true;
            this.runningCharacterGif.Visible = true;
            this.staticImage.Visible = true;
            this.closeButton.Button.Visible = true;

            // Поднимаем элементы на передний план
            this.controlLabel.BringToFront();
            this.player1Label.BringToFront();
            this.player2Label.BringToFront();
            this.startGameButton.button.BringToFront();
            this.runningCharacterGif.BringToFront();
            this.staticImage.BringToFront();
            this.closeButton.Button.BringToFront();

            // Привязываем события
            this.closeButton.AttachEvents();
            this.startGameButton.AttachEvents();
        }

        public bool IsVisible => panel.Visible;

        // Показывает панель
        public void Show()
        {
            panel.Visible = true;
            controlLabel.Visible = true;
            player1Label.Visible = true;
            player2Label.Visible = true;
            startGameButton.button.Visible = true;
            runningCharacterGif.Visible = true;
            staticImage.Visible = true;
            closeButton.Button.Visible = true;
        }

        // Скрывает панель
        public void Hide()
        {
            panel.Visible = false;
        }

        // Запускает игру
        private void StartGame()
        {
            // Получаем MainForm
            MainForm mainForm = (MainForm)panel.FindForm();
            if (mainForm == null)
            {
                return;
            }

            // Сохраняем выбранных персонажей
            int selectedCharacterPlayer1 = mainForm.SelectedCharacterPlayer1;
            int selectedCharacterPlayer2 = mainForm.SelectedCharacterPlayer2;

            bool isMusicEnabled = Settings.Default.IsMusicEnabled;
            bool isSoundEnabled = Settings.Default.IsSoundEnabled;

            this.Hide();

            // Сохраняем позицию MainForm
            Point mainFormLocation = mainForm.Location;

            // Останавливаем процессы и скрываем MainForm
            mainForm.StopAllProcesses();
            mainForm.Hide();

            // Запускаем игровую форму
            GameForm gameForm = new GameForm(mainForm, selectedCharacterPlayer1, selectedCharacterPlayer2, isMusicEnabled, isSoundEnabled);
            gameForm.StartPosition = FormStartPosition.Manual;
            gameForm.Location = mainFormLocation;
            gameForm.Show();
        }
    }
}