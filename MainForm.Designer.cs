using Gardener.mainMenuClasses.panelClasses;
using System.Windows.Forms;

namespace Gardener
{
    partial class MainForm
    {
        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pictureBoxTitle = new PictureBox();
            characterIconButton = new Button();
            playButton = new Button();
            settingsButton = new Button();
            exitButton = new Button();
            settingsPanel = new DoubleBufferedPanel();
            closeButton = new Button();
            musicToggleButton = new Button();
            soundToggleButton = new Button();
            musicLabel = new Label();
            soundLabel = new Label();
            characterSelectPanel = new DoubleBufferedPanel();
            playerSelectPanel = new Panel();
            player2Button = new Button();
            player1Button = new Button();
            playerIndicator3 = new PictureBox();
            playerIndicator2 = new PictureBox();
            playerIndicator1 = new PictureBox();
            character1PictureBox = new PictureBox();
            character2PictureBox = new PictureBox();
            character3PictureBox = new PictureBox();
            characterCloseButton = new Button();
            selectButton1 = new Button();
            selectButton2 = new Button();
            selectButton3 = new Button();
            playPanel = new DoubleBufferedPanel();
            staticImage = new PictureBox();
            runningCharacterGif = new PictureBox();
            startGameButton = new Button();
            player2Label = new Label();
            player1Label = new Label();
            controlLabel = new Label();
            playCloseButton = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBoxTitle).BeginInit();
            settingsPanel.SuspendLayout();
            characterSelectPanel.SuspendLayout();
            playerSelectPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)playerIndicator3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)playerIndicator2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)playerIndicator1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)character1PictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)character2PictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)character3PictureBox).BeginInit();
            playPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)staticImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)runningCharacterGif).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxTitle
            // 
            pictureBoxTitle.Anchor = AnchorStyles.Top;
            pictureBoxTitle.BackColor = Color.Transparent;
            pictureBoxTitle.Image = Properties.Resources.gardener;
            pictureBoxTitle.Location = new Point(377, 25);
            pictureBoxTitle.Name = "pictureBoxTitle";
            pictureBoxTitle.Size = new Size(890, 240);
            pictureBoxTitle.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBoxTitle.TabIndex = 0;
            pictureBoxTitle.TabStop = false;
            // 
            // characterIconButton
            // 
            characterIconButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            characterIconButton.BackColor = Color.Transparent;
            characterIconButton.BackgroundImage = Properties.Resources.chooseCharacter;
            characterIconButton.BackgroundImageLayout = ImageLayout.Stretch;
            characterIconButton.FlatAppearance.BorderSize = 0;
            characterIconButton.FlatStyle = FlatStyle.Flat;
            characterIconButton.Location = new Point(1400, 644);
            characterIconButton.Name = "characterIconButton";
            characterIconButton.Size = new Size(180, 180);
            characterIconButton.TabIndex = 1;
            characterIconButton.UseVisualStyleBackColor = false;
            // 
            // playButton
            // 
            playButton.Anchor = AnchorStyles.Top;
            playButton.BackColor = Color.Transparent;
            playButton.BackgroundImage = Properties.Resources.play;
            playButton.BackgroundImageLayout = ImageLayout.Stretch;
            playButton.FlatAppearance.BorderSize = 0;
            playButton.FlatStyle = FlatStyle.Flat;
            playButton.Location = new Point(548, 320);
            playButton.Name = "playButton";
            playButton.Size = new Size(603, 144);
            playButton.TabIndex = 2;
            playButton.UseVisualStyleBackColor = false;
            // 
            // settingsButton
            // 
            settingsButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            settingsButton.BackColor = Color.Transparent;
            settingsButton.BackgroundImage = Properties.Resources.settings;
            settingsButton.BackgroundImageLayout = ImageLayout.Stretch;
            settingsButton.FlatAppearance.BorderSize = 0;
            settingsButton.FlatStyle = FlatStyle.Flat;
            settingsButton.Location = new Point(548, 500);
            settingsButton.Name = "settingsButton";
            settingsButton.Size = new Size(603, 144);
            settingsButton.TabIndex = 3;
            settingsButton.UseVisualStyleBackColor = false;
            // 
            // exitButton
            // 
            exitButton.Anchor = AnchorStyles.Bottom;
            exitButton.BackColor = Color.Transparent;
            exitButton.BackgroundImage = Properties.Resources.exit;
            exitButton.BackgroundImageLayout = ImageLayout.Stretch;
            exitButton.FlatAppearance.BorderSize = 0;
            exitButton.FlatStyle = FlatStyle.Flat;
            exitButton.Location = new Point(548, 680);
            exitButton.Name = "exitButton";
            exitButton.Size = new Size(603, 144);
            exitButton.TabIndex = 4;
            exitButton.UseVisualStyleBackColor = false;
            // 
            // settingsPanel
            // 
            settingsPanel.Anchor = AnchorStyles.Top;
            settingsPanel.BackColor = Color.Transparent;
            settingsPanel.BackgroundImage = Properties.Resources.panel;
            settingsPanel.BackgroundImageLayout = ImageLayout.Stretch;
            settingsPanel.Controls.Add(closeButton);
            settingsPanel.Controls.Add(musicToggleButton);
            settingsPanel.Controls.Add(soundToggleButton);
            settingsPanel.Controls.Add(musicLabel);
            settingsPanel.Controls.Add(soundLabel);
            settingsPanel.Location = new Point(120, 30);
            settingsPanel.Name = "settingsPanel";
            settingsPanel.Size = new Size(1250, 820);
            settingsPanel.TabIndex = 5;
            settingsPanel.Visible = false;
            // 
            // closeButton
            // 
            closeButton.BackgroundImage = Properties.Resources.krest14;
            closeButton.BackgroundImageLayout = ImageLayout.Stretch;
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.Location = new Point(1050, 0);
            closeButton.Name = "closeButton";
            closeButton.Size = new Size(200, 200);
            closeButton.TabIndex = 5;
            closeButton.UseVisualStyleBackColor = true;
            closeButton.Visible = false;
            // 
            // musicToggleButton
            // 
            musicToggleButton.BackgroundImage = Properties.Resources.musicOnn;
            musicToggleButton.BackgroundImageLayout = ImageLayout.Stretch;
            musicToggleButton.FlatAppearance.BorderSize = 0;
            musicToggleButton.FlatStyle = FlatStyle.Flat;
            musicToggleButton.Location = new Point(170, 125);
            musicToggleButton.Name = "musicToggleButton";
            musicToggleButton.Size = new Size(250, 250);
            musicToggleButton.TabIndex = 8;
            musicToggleButton.UseVisualStyleBackColor = true;
            musicToggleButton.Visible = false;
            // 
            // soundToggleButton
            // 
            soundToggleButton.BackgroundImage = Properties.Resources.audioOnn;
            soundToggleButton.BackgroundImageLayout = ImageLayout.Stretch;
            soundToggleButton.FlatAppearance.BorderSize = 0;
            soundToggleButton.FlatStyle = FlatStyle.Flat;
            soundToggleButton.Location = new Point(170, 450);
            soundToggleButton.Name = "soundToggleButton";
            soundToggleButton.Size = new Size(250, 250);
            soundToggleButton.TabIndex = 9;
            soundToggleButton.UseVisualStyleBackColor = true;
            soundToggleButton.Visible = false;
            // 
            // musicLabel
            // 
            musicLabel.AutoSize = true;
            musicLabel.Font = new Font("Monocraft", 72F, FontStyle.Bold, GraphicsUnit.Point, 0);
            musicLabel.ForeColor = Color.White;
            musicLabel.Location = new Point(480, 175);
            musicLabel.Name = "musicLabel";
            musicLabel.Size = new Size(555, 133);
            musicLabel.TabIndex = 7;
            musicLabel.Text = "Музыка";
            musicLabel.Visible = false;
            // 
            // soundLabel
            // 
            soundLabel.AutoSize = true;
            soundLabel.Font = new Font("Monocraft", 72F, FontStyle.Bold, GraphicsUnit.Point, 0);
            soundLabel.ForeColor = Color.White;
            soundLabel.Location = new Point(480, 500);
            soundLabel.Name = "soundLabel";
            soundLabel.Size = new Size(389, 133);
            soundLabel.TabIndex = 6;
            soundLabel.Text = "Звук";
            soundLabel.Visible = false;
            // 
            // characterSelectPanel
            // 
            characterSelectPanel.Anchor = AnchorStyles.Top;
            characterSelectPanel.BackColor = Color.Transparent;
            characterSelectPanel.BackgroundImage = Properties.Resources.panel;
            characterSelectPanel.BackgroundImageLayout = ImageLayout.Stretch;
            characterSelectPanel.Controls.Add(playerSelectPanel);
            characterSelectPanel.Controls.Add(playerIndicator3);
            characterSelectPanel.Controls.Add(playerIndicator2);
            characterSelectPanel.Controls.Add(playerIndicator1);
            characterSelectPanel.Controls.Add(character1PictureBox);
            characterSelectPanel.Controls.Add(character2PictureBox);
            characterSelectPanel.Controls.Add(character3PictureBox);
            characterSelectPanel.Controls.Add(characterCloseButton);
            characterSelectPanel.Controls.Add(selectButton1);
            characterSelectPanel.Controls.Add(selectButton2);
            characterSelectPanel.Controls.Add(selectButton3);
            characterSelectPanel.Location = new Point(120, 30);
            characterSelectPanel.Name = "characterSelectPanel";
            characterSelectPanel.Size = new Size(1250, 820);
            characterSelectPanel.TabIndex = 6;
            characterSelectPanel.Visible = false;
            // 
            // playerSelectPanel
            // 
            playerSelectPanel.BackgroundImage = Properties.Resources.panel200;
            playerSelectPanel.BackgroundImageLayout = ImageLayout.Stretch;
            playerSelectPanel.Controls.Add(player2Button);
            playerSelectPanel.Controls.Add(player1Button);
            playerSelectPanel.Location = new Point(485, 335);
            playerSelectPanel.Name = "playerSelectPanel";
            playerSelectPanel.Size = new Size(280, 150);
            playerSelectPanel.TabIndex = 10;
            playerSelectPanel.Visible = false;
            // 
            // player2Button
            // 
            player2Button.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            player2Button.BackgroundImage = Properties.Resources.player2;
            player2Button.BackgroundImageLayout = ImageLayout.Stretch;
            player2Button.FlatAppearance.BorderSize = 0;
            player2Button.FlatStyle = FlatStyle.Flat;
            player2Button.Location = new Point(160, 37);
            player2Button.Name = "player2Button";
            player2Button.Size = new Size(75, 75);
            player2Button.TabIndex = 1;
            player2Button.UseVisualStyleBackColor = true;
            // 
            // player1Button
            // 
            player1Button.BackgroundImage = Properties.Resources.player1;
            player1Button.BackgroundImageLayout = ImageLayout.Stretch;
            player1Button.FlatAppearance.BorderSize = 0;
            player1Button.FlatStyle = FlatStyle.Flat;
            player1Button.Location = new Point(50, 37);
            player1Button.Name = "player1Button";
            player1Button.Size = new Size(75, 75);
            player1Button.TabIndex = 0;
            player1Button.UseVisualStyleBackColor = true;
            // 
            // playerIndicator3
            // 
            playerIndicator3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            playerIndicator3.Image = Properties.Resources.empty;
            playerIndicator3.Location = new Point(985, 186);
            playerIndicator3.Name = "playerIndicator3";
            playerIndicator3.Size = new Size(75, 75);
            playerIndicator3.SizeMode = PictureBoxSizeMode.StretchImage;
            playerIndicator3.TabIndex = 9;
            playerIndicator3.TabStop = false;
            // 
            // playerIndicator2
            // 
            playerIndicator2.Anchor = AnchorStyles.Top;
            playerIndicator2.Image = Properties.Resources.player2;
            playerIndicator2.Location = new Point(590, 199);
            playerIndicator2.Name = "playerIndicator2";
            playerIndicator2.Size = new Size(75, 75);
            playerIndicator2.SizeMode = PictureBoxSizeMode.StretchImage;
            playerIndicator2.TabIndex = 8;
            playerIndicator2.TabStop = false;
            // 
            // playerIndicator1
            // 
            playerIndicator1.Image = Properties.Resources.player1;
            playerIndicator1.Location = new Point(195, 85);
            playerIndicator1.Name = "playerIndicator1";
            playerIndicator1.Size = new Size(75, 75);
            playerIndicator1.SizeMode = PictureBoxSizeMode.StretchImage;
            playerIndicator1.TabIndex = 7;
            playerIndicator1.TabStop = false;
            // 
            // character1PictureBox
            // 
            character1PictureBox.Location = new Point(94, 175);
            character1PictureBox.Name = "character1PictureBox";
            character1PictureBox.Size = new Size(272, 459);
            character1PictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            character1PictureBox.TabIndex = 1;
            character1PictureBox.TabStop = false;
            character1PictureBox.Visible = false;
            // 
            // character2PictureBox
            // 
            character2PictureBox.Anchor = AnchorStyles.Top;
            character2PictureBox.Location = new Point(489, 294);
            character2PictureBox.Name = "character2PictureBox";
            character2PictureBox.Size = new Size(272, 340);
            character2PictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            character2PictureBox.TabIndex = 2;
            character2PictureBox.TabStop = false;
            character2PictureBox.Visible = false;
            // 
            // character3PictureBox
            // 
            character3PictureBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            character3PictureBox.Location = new Point(884, 277);
            character3PictureBox.Name = "character3PictureBox";
            character3PictureBox.Size = new Size(272, 357);
            character3PictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            character3PictureBox.TabIndex = 3;
            character3PictureBox.TabStop = false;
            character3PictureBox.Visible = false;
            // 
            // characterCloseButton
            // 
            characterCloseButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            characterCloseButton.BackgroundImage = Properties.Resources.krest14;
            characterCloseButton.BackgroundImageLayout = ImageLayout.Stretch;
            characterCloseButton.FlatAppearance.BorderSize = 0;
            characterCloseButton.FlatStyle = FlatStyle.Flat;
            characterCloseButton.Location = new Point(1050, 0);
            characterCloseButton.Name = "characterCloseButton";
            characterCloseButton.Size = new Size(200, 200);
            characterCloseButton.TabIndex = 0;
            characterCloseButton.UseVisualStyleBackColor = true;
            characterCloseButton.Visible = false;
            // 
            // selectButton1
            // 
            selectButton1.BackgroundImageLayout = ImageLayout.Stretch;
            selectButton1.FlatAppearance.BorderSize = 0;
            selectButton1.FlatStyle = FlatStyle.Flat;
            selectButton1.Location = new Point(65, 675);
            selectButton1.Name = "selectButton1";
            selectButton1.Size = new Size(330, 88);
            selectButton1.TabIndex = 4;
            selectButton1.UseVisualStyleBackColor = true;
            selectButton1.Visible = false;
            // 
            // selectButton2
            // 
            selectButton2.Anchor = AnchorStyles.Top;
            selectButton2.BackgroundImageLayout = ImageLayout.Stretch;
            selectButton2.FlatAppearance.BorderSize = 0;
            selectButton2.FlatStyle = FlatStyle.Flat;
            selectButton2.Location = new Point(460, 675);
            selectButton2.Name = "selectButton2";
            selectButton2.Size = new Size(330, 88);
            selectButton2.TabIndex = 5;
            selectButton2.UseVisualStyleBackColor = true;
            selectButton2.Visible = false;
            // 
            // selectButton3
            // 
            selectButton3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            selectButton3.BackgroundImageLayout = ImageLayout.Stretch;
            selectButton3.FlatAppearance.BorderSize = 0;
            selectButton3.FlatStyle = FlatStyle.Flat;
            selectButton3.Location = new Point(855, 675);
            selectButton3.Name = "selectButton3";
            selectButton3.Size = new Size(330, 88);
            selectButton3.TabIndex = 6;
            selectButton3.UseVisualStyleBackColor = true;
            selectButton3.Visible = false;
            // 
            // playPanel
            // 
            playPanel.Anchor = AnchorStyles.Top;
            playPanel.BackColor = Color.Transparent;
            playPanel.BackgroundImage = Properties.Resources.panel;
            playPanel.BackgroundImageLayout = ImageLayout.Stretch;
            playPanel.Controls.Add(staticImage);
            playPanel.Controls.Add(runningCharacterGif);
            playPanel.Controls.Add(startGameButton);
            playPanel.Controls.Add(player2Label);
            playPanel.Controls.Add(player1Label);
            playPanel.Controls.Add(controlLabel);
            playPanel.Controls.Add(playCloseButton);
            playPanel.Location = new Point(120, 30);
            playPanel.Name = "playPanel";
            playPanel.Size = new Size(1250, 820);
            playPanel.TabIndex = 7;
            playPanel.Visible = false;
            // 
            // staticImage
            // 
            staticImage.Image = Properties.Resources.boxAndFlower;
            staticImage.Location = new Point(1025, 618);
            staticImage.Name = "staticImage";
            staticImage.Size = new Size(154, 140);
            staticImage.SizeMode = PictureBoxSizeMode.AutoSize;
            staticImage.TabIndex = 6;
            staticImage.TabStop = false;
            // 
            // runningCharacterGif
            // 
            runningCharacterGif.Image = Properties.Resources.fullAnimationCharacter;
            runningCharacterGif.Location = new Point(85, 562);
            runningCharacterGif.Name = "runningCharacterGif";
            runningCharacterGif.Size = new Size(126, 196);
            runningCharacterGif.SizeMode = PictureBoxSizeMode.AutoSize;
            runningCharacterGif.TabIndex = 5;
            runningCharacterGif.TabStop = false;
            // 
            // startGameButton
            // 
            startGameButton.Anchor = AnchorStyles.Bottom;
            startGameButton.BackgroundImage = Properties.Resources.startTheGame;
            startGameButton.BackgroundImageLayout = ImageLayout.Stretch;
            startGameButton.FlatAppearance.BorderSize = 0;
            startGameButton.FlatStyle = FlatStyle.Flat;
            startGameButton.Location = new Point(285, 630);
            startGameButton.Name = "startGameButton";
            startGameButton.Size = new Size(680, 128);
            startGameButton.TabIndex = 4;
            startGameButton.UseVisualStyleBackColor = true;
            // 
            // player2Label
            // 
            player2Label.AutoSize = true;
            player2Label.Font = new Font("Monocraft", 19.7999973F, FontStyle.Bold, GraphicsUnit.Point, 0);
            player2Label.ForeColor = Color.White;
            player2Label.Location = new Point(269, 430);
            player2Label.Name = "player2Label";
            player2Label.Size = new Size(711, 148);
            player2Label.TabIndex = 3;
            player2Label.Text = "            2 игрок\r\n\r\n↑←↓→   — ходьба\r\nEnter — взаимодействие с миром";
            // 
            // player1Label
            // 
            player1Label.AutoSize = true;
            player1Label.Font = new Font("Monocraft", 19.7999973F, FontStyle.Bold, GraphicsUnit.Point, 0);
            player1Label.ForeColor = Color.White;
            player1Label.Location = new Point(269, 230);
            player1Label.Name = "player1Label";
            player1Label.Size = new Size(688, 148);
            player1Label.TabIndex = 2;
            player1Label.Text = "            1 игрок\r\n\r\nWASD — ходьба\r\nE    — взаимодействие с миром";
            // 
            // controlLabel
            // 
            controlLabel.AutoSize = true;
            controlLabel.Font = new Font("Monocraft", 72F, FontStyle.Bold, GraphicsUnit.Point, 0);
            controlLabel.ForeColor = Color.White;
            controlLabel.Location = new Point(160, 67);
            controlLabel.Name = "controlLabel";
            controlLabel.Size = new Size(887, 133);
            controlLabel.TabIndex = 1;
            controlLabel.Text = "УПРАВЛЕНИЕ";
            controlLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // playCloseButton
            // 
            playCloseButton.BackColor = Color.Transparent;
            playCloseButton.BackgroundImage = Properties.Resources.krest14;
            playCloseButton.BackgroundImageLayout = ImageLayout.Stretch;
            playCloseButton.FlatAppearance.BorderSize = 0;
            playCloseButton.FlatStyle = FlatStyle.Flat;
            playCloseButton.Location = new Point(1050, 0);
            playCloseButton.Name = "playCloseButton";
            playCloseButton.Size = new Size(200, 200);
            playCloseButton.TabIndex = 0;
            playCloseButton.UseVisualStyleBackColor = false;
            playCloseButton.Visible = false;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1600, 900);
            Controls.Add(playPanel);
            Controls.Add(characterSelectPanel);
            Controls.Add(settingsPanel);
            Controls.Add(exitButton);
            Controls.Add(settingsButton);
            Controls.Add(playButton);
            Controls.Add(pictureBoxTitle);
            Controls.Add(characterIconButton);
            Name = "MainForm";
            Text = "Gardener";
            ((System.ComponentModel.ISupportInitialize)pictureBoxTitle).EndInit();
            settingsPanel.ResumeLayout(false);
            settingsPanel.PerformLayout();
            characterSelectPanel.ResumeLayout(false);
            characterSelectPanel.PerformLayout();
            playerSelectPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)playerIndicator3).EndInit();
            ((System.ComponentModel.ISupportInitialize)playerIndicator2).EndInit();
            ((System.ComponentModel.ISupportInitialize)playerIndicator1).EndInit();
            ((System.ComponentModel.ISupportInitialize)character1PictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)character2PictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)character3PictureBox).EndInit();
            playPanel.ResumeLayout(false);
            playPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)staticImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)runningCharacterGif).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBoxTitle;
        private Button characterIconButton; // Обновляем поле
        private Button playButton;
        private Button settingsButton;
        private Button exitButton;
        private DoubleBufferedPanel settingsPanel;
        private DoubleBufferedPanel characterSelectPanel;
        private Button characterCloseButton;
        private PictureBox character1PictureBox;
        private PictureBox character2PictureBox;
        private PictureBox character3PictureBox;
        private Button selectButton1;
        private Button selectButton2;
        private Button selectButton3;
        private DoubleBufferedPanel playPanel;
        private Button playCloseButton;
        private Button closeButton;
        private Label soundLabel;
        private Label musicLabel;
        private Button musicToggleButton;
        private Button soundToggleButton;
        private PictureBox playerIndicator1;
        private PictureBox playerIndicator3;
        private PictureBox playerIndicator2;
        private Panel playerSelectPanel;
        private Button player1Button;
        private Button player2Button;
        private Label controlLabel;
        private Label player1Label;
        private Label player2Label;
        private Button startGameButton;
        private PictureBox runningCharacterGif;
        private PictureBox staticImage;
    }
}