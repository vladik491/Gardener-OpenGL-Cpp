namespace Gardener.mainMenuClasses.panelClasses
{
    // Панель выбора персонажей
    public class CharacterSelectPanel : IPanel
    {
        // Блок: Элементы управления
        private readonly Panel panel; 
        private readonly CloseButton closeButton; 
        private readonly PictureBox character1PictureBox; 
        private readonly PictureBox character2PictureBox; 
        private readonly PictureBox character3PictureBox; 
        private readonly SelectButton selectButton1; 
        private readonly SelectButton selectButton2; 
        private readonly SelectButton selectButton3; 
        private readonly PictureBox playerIndicator1; 
        private readonly PictureBox playerIndicator2; 
        private readonly PictureBox playerIndicator3; 
        private readonly Panel playerSelectPanel; 
        private readonly PlayerSelectButton player1Button; 
        private readonly PlayerSelectButton player2Button; 

        // Блок: Зависимости
        private readonly ButtonSoundPlayer soundPlayer;
        private readonly Action<int, int> updateSelectedCharacter; 

        // Блок: Состояние
        private int currentCharacterIndex;
        private bool isPlayerSelectPanelOpen; 
        private int lastOpenedButtonIndex; 

        // Конструктор: Инициализирует панель выбора персонажей
        public CharacterSelectPanel(
            Panel panel,
            Button closeButton,
            PictureBox character1PictureBox,
            PictureBox character2PictureBox,
            PictureBox character3PictureBox,
            Button selectButton1,
            Button selectButton2,
            Button selectButton3,
            PictureBox playerIndicator1,
            PictureBox playerIndicator2,
            PictureBox playerIndicator3,
            Panel playerSelectPanel,
            Button player1Button,
            Button player2Button,
            ButtonSoundPlayer soundPlayer,
            Action<int, int> updateSelectedCharacter)
        {
            this.panel = panel;
            this.closeButton = new CloseButton(closeButton, panel, soundPlayer);
            this.character1PictureBox = character1PictureBox;
            this.character2PictureBox = character2PictureBox;
            this.character3PictureBox = character3PictureBox;
            this.selectButton1 = new SelectButton(selectButton1, 1, OpenPlayerSelectPanel, soundPlayer);
            this.selectButton2 = new SelectButton(selectButton2, 2, OpenPlayerSelectPanel, soundPlayer);
            this.selectButton3 = new SelectButton(selectButton3, 3, OpenPlayerSelectPanel, soundPlayer);
            this.playerIndicator1 = playerIndicator1;
            this.playerIndicator2 = playerIndicator2;
            this.playerIndicator3 = playerIndicator3;
            this.playerSelectPanel = playerSelectPanel;
            this.player1Button = new PlayerSelectButton(player1Button, 1, SelectPlayer, soundPlayer);
            this.player2Button = new PlayerSelectButton(player2Button, 2, SelectPlayer, soundPlayer);
            this.soundPlayer = soundPlayer;
            this.updateSelectedCharacter = updateSelectedCharacter;

            // Инициализируем состояние
            this.isPlayerSelectPanelOpen = false;
            this.lastOpenedButtonIndex = 0;

            // Настраиваем прозрачность
            this.character1PictureBox.BackColor = Color.Transparent;
            this.character2PictureBox.BackColor = Color.Transparent;
            this.character3PictureBox.BackColor = Color.Transparent;
            this.playerIndicator1.BackColor = Color.Transparent;
            this.playerIndicator2.BackColor = Color.Transparent;
            this.playerIndicator3.BackColor = Color.Transparent;
            this.playerSelectPanel.BackColor = Color.Transparent;
            this.player1Button.button.BackColor = Color.Transparent;
            this.player1Button.button.FlatAppearance.MouseDownBackColor = Color.Transparent;
            this.player1Button.button.FlatAppearance.MouseOverBackColor = Color.Transparent;
            this.player2Button.button.BackColor = Color.Transparent;
            this.player2Button.button.FlatAppearance.MouseDownBackColor = Color.Transparent;
            this.player2Button.button.FlatAppearance.MouseOverBackColor = Color.Transparent;

            // Убедимся, что элементы видны
            this.character1PictureBox.Visible = true;
            this.character2PictureBox.Visible = true;
            this.character3PictureBox.Visible = true;
            this.selectButton1.button.Visible = true;
            this.selectButton2.button.Visible = true;
            this.selectButton3.button.Visible = true;
            this.playerIndicator1.Visible = true;
            this.playerIndicator2.Visible = true;
            this.playerIndicator3.Visible = true;
            this.closeButton.Button.Visible = true;

            // Поднимаем элементы на передний план
            this.character1PictureBox.BringToFront();
            this.character2PictureBox.BringToFront();
            this.character3PictureBox.BringToFront();
            this.selectButton1.button.BringToFront();
            this.selectButton2.button.BringToFront();
            this.selectButton3.button.BringToFront();
            this.playerIndicator1.BringToFront();
            this.playerIndicator2.BringToFront();
            this.playerIndicator3.BringToFront();
            this.closeButton.Button.BringToFront();

            // Привязываем события
            this.closeButton.AttachEvents();
            this.selectButton1.AttachEvents();
            this.selectButton2.AttachEvents();
            this.selectButton3.AttachEvents();
            this.player1Button.AttachEvents();
            this.player2Button.AttachEvents();

            // Настраиваем перетаскивание
            SetupDragAndDrop(playerIndicator1);
            SetupDragAndDrop(playerIndicator2);
            SetupDragAndDrop(playerIndicator3);
        }

        public bool IsVisible => panel.Visible;

        // Показывает панель
        public void Show()
        {
            panel.Visible = true;
            character1PictureBox.Visible = true;
            character2PictureBox.Visible = true;
            character3PictureBox.Visible = true;
            selectButton1.button.Visible = true;
            selectButton2.button.Visible = true;
            selectButton3.button.Visible = true;
            playerIndicator1.Visible = true;
            playerIndicator2.Visible = true;
            playerIndicator3.Visible = true;
            closeButton.Button.Visible = true;

            UpdateButtons(((MainForm)panel.FindForm()).SelectedCharacterPlayer1, ((MainForm)panel.FindForm()).SelectedCharacterPlayer2);
        }

        // Скрывает панель
        public void Hide()
        {
            panel.Visible = false;
            playerSelectPanel.Visible = false;
            isPlayerSelectPanelOpen = false;
        }

        // Обновляет состояние кнопок и индикаторов
        public void UpdateButtons(int selectedCharacterPlayer1, int selectedCharacterPlayer2)
        {
            // Обновляем изображения индикаторов
            playerIndicator1.Image = GetIndicatorImage(1, selectedCharacterPlayer1, selectedCharacterPlayer2);
            playerIndicator2.Image = GetIndicatorImage(2, selectedCharacterPlayer1, selectedCharacterPlayer2);
            playerIndicator3.Image = GetIndicatorImage(3, selectedCharacterPlayer1, selectedCharacterPlayer2);

            // Обновляем состояние кнопок
            selectButton1.button.Enabled = !IsCharacterSelected(1, selectedCharacterPlayer1, selectedCharacterPlayer2);
            selectButton2.button.Enabled = !IsCharacterSelected(2, selectedCharacterPlayer1, selectedCharacterPlayer2);
            selectButton3.button.Enabled = !IsCharacterSelected(3, selectedCharacterPlayer1, selectedCharacterPlayer2);

            selectButton1.BackgroundImage = IsCharacterSelected(1, selectedCharacterPlayer1, selectedCharacterPlayer2) ? Properties.Resources.selected : Properties.Resources.choose;
            selectButton2.BackgroundImage = IsCharacterSelected(2, selectedCharacterPlayer1, selectedCharacterPlayer2) ? Properties.Resources.selected : Properties.Resources.choose;
            selectButton3.BackgroundImage = IsCharacterSelected(3, selectedCharacterPlayer1, selectedCharacterPlayer2) ? Properties.Resources.selected : Properties.Resources.choose;
        }

        // Возвращает изображение индикатора для персонажа
        private Image GetIndicatorImage(int characterIndex, int selectedCharacterPlayer1, int selectedCharacterPlayer2)
        {
            if (characterIndex == selectedCharacterPlayer1)
                return Properties.Resources.player1;
            if (characterIndex == selectedCharacterPlayer2)
                return Properties.Resources.player2;
            return Properties.Resources.empty;
        }

        // Проверяет, выбран ли персонаж
        private bool IsCharacterSelected(int characterIndex, int selectedCharacterPlayer1, int selectedCharacterPlayer2)
        {
            return characterIndex == selectedCharacterPlayer1 || characterIndex == selectedCharacterPlayer2;
        }

        // Открывает панель выбора игрока
        private void OpenPlayerSelectPanel(int characterIndex)
        {
            // Если панель уже открыта и нажата та же кнопка, закрываем её
            if (isPlayerSelectPanelOpen && lastOpenedButtonIndex == characterIndex)
            {
                playerSelectPanel.Visible = false;
                isPlayerSelectPanelOpen = false;
                lastOpenedButtonIndex = 0;
            }
            else
            {
                // Открываем панель
                currentCharacterIndex = characterIndex;
                lastOpenedButtonIndex = characterIndex;
                playerSelectPanel.Visible = true;
                playerSelectPanel.BringToFront();
                isPlayerSelectPanelOpen = true;
            }
        }

        // Выбирает игрока для персонажа
        private void SelectPlayer(int playerIndex)
        {
            updateSelectedCharacter(currentCharacterIndex, playerIndex);
            playerSelectPanel.Visible = false;
            isPlayerSelectPanelOpen = false;
            lastOpenedButtonIndex = 0;
        }

        // Настраивает перетаскивание индикаторов
        private void SetupDragAndDrop(PictureBox pictureBox)
        {
            pictureBox.AllowDrop = true;
            pictureBox.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left && pictureBox.Image != Properties.Resources.empty && !isPlayerSelectPanelOpen)
                {
                    pictureBox.DoDragDrop(pictureBox, DragDropEffects.Move);
                }
            };
            pictureBox.DragEnter += (s, e) =>
            {
                if (e.Data.GetDataPresent(typeof(PictureBox)))
                {
                    e.Effect = DragDropEffects.Move;
                }
            };
            pictureBox.DragDrop += (s, e) =>
            {
                var source = (PictureBox)e.Data.GetData(typeof(PictureBox));
                var target = (PictureBox)s;

                if (source != target)
                {
                    int sourceCharacterIndex = GetCharacterIndex(source);
                    int targetCharacterIndex = GetCharacterIndex(target);

                    int selectedCharacterPlayer1 = ((MainForm)panel.FindForm()).SelectedCharacterPlayer1;
                    int selectedCharacterPlayer2 = ((MainForm)panel.FindForm()).SelectedCharacterPlayer2;

                    if (!IsCharacterSelected(targetCharacterIndex, selectedCharacterPlayer1, selectedCharacterPlayer2))
                    {
                        int playerIndex = 0;
                        if (sourceCharacterIndex == selectedCharacterPlayer1)
                        {
                            playerIndex = 1;
                        }
                        else if (sourceCharacterIndex == selectedCharacterPlayer2)
                        {
                            playerIndex = 2;
                        }

                        if (playerIndex != 0)
                        {
                            updateSelectedCharacter(targetCharacterIndex, playerIndex);
                            soundPlayer?.Play();
                        }
                    }
                }
            };
        }

        // Возвращает индекс персонажа по индикатору
        private int GetCharacterIndex(PictureBox pictureBox)
        {
            if (pictureBox == playerIndicator1) return 1;
            if (pictureBox == playerIndicator2) return 2;
            if (pictureBox == playerIndicator3) return 3;
            return 0;
        }
    }
}