using OpenTK.Graphics.OpenGL;
using Gardener.gameClasses.Logic;
using Gardener.gameClasses.Rendering;
using System.Drawing.Text;

namespace Gardener
{
    // Главная форма игры, управляет окном и рендерингом
    public partial class GameForm : Form
    {
        // Блок: Основные объекты игры
        private MainForm mainForm;
        private GameManager gameManager; 
        private GameRenderer renderer;
        private Map map;

        // Блок: Настройки игры
        private int selectedCharacterPlayer1;
        private int selectedCharacterPlayer2; 
        private bool isMusicEnabled; 
        private bool isSoundEnabled; 
        private bool _isReturningToMainMenu; 

        // Блок: Рендеринг и время
        private System.Windows.Forms.Timer renderTimer; 
        private DateTime lastFrameTime; 
        private const int TileSize = 48; 

        // Блок: Управление игроком 1 (WASD)
        private bool player1MoveUp, player1MoveDown, player1MoveLeft, player1MoveRight;
        private bool player1Interact; 
        private bool[] numberKeysPlayer1; 

        // Блок: Управление игроком 2 (стрелки)
        private bool player2MoveUp, player2MoveDown, player2MoveLeft, player2MoveRight;
        private bool player2Interact; 
        private bool[] numberKeysPlayer2; 

        // Блок: UI элементы
        private Font _customFont; // Шрифт для текста

        // Конструктор формы игры
        public GameForm(MainForm mainForm, int selectedCharacterPlayer1, int selectedCharacterPlayer2, bool isMusicEnabled, bool isSoundEnabled)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.selectedCharacterPlayer1 = selectedCharacterPlayer1;
            this.selectedCharacterPlayer2 = selectedCharacterPlayer2;
            this.isMusicEnabled = isMusicEnabled;
            this.isSoundEnabled = isSoundEnabled;
            map = new Map(3200, 1792);
            this.ClientSize = new Size(1600, 900);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = true;
            this.Text = "Gardener";

            LoadCustomFont();
            lblPlayer1Fruits.Font = new Font(_customFont.FontFamily, 15F, FontStyle.Regular);
            lblPlayer2Fruits.Font = new Font(_customFont.FontFamily, 15F, FontStyle.Regular);
            lblPlayer1Fruits.TextAlign = ContentAlignment.MiddleCenter;
            lblPlayer2Fruits.TextAlign = ContentAlignment.MiddleCenter;
            lblPlayer1Fruits.Visible = false;
            lblPlayer2Fruits.Visible = false;

            renderTimer = new System.Windows.Forms.Timer();
            renderTimer.Interval = 16; // ~60 FPS
            renderTimer.Tick += RenderTimer_Tick;
            renderTimer.Start();
            glControl.Load += GlControl_Load;
            glControl.Paint += GlControl_Paint;
            glControl.Resize += GlControl_Resize;
            glControl.KeyDown += GlControl_KeyDown;
            glControl.KeyUp += GlControl_KeyUp;
            glControl.MouseDown += GlControl_MouseDown;
            glControl.GotFocus += GlControl_GotFocus;
            glControl.LostFocus += GlControl_LostFocus;
            this.Activated += GameForm_Activated;
            this.FormClosing += GameForm_FormClosing;
            this.Shown += GameForm_Shown;

            numberKeysPlayer1 = new bool[9];
            numberKeysPlayer2 = new bool[9];
            _isReturningToMainMenu = false;
        }

        // Загружает шрифт для текста
        private void LoadCustomFont()
        {
            PrivateFontCollection fontCollection = new PrivateFontCollection();
            string fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Monocraft1.ttf");
            fontCollection.AddFontFile(fontPath);
            _customFont = new Font(fontCollection.Families[0], 15F, FontStyle.Regular);
        }

        // Освобождает ресурсы
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
                _customFont?.Dispose();
            }
            base.Dispose(disposing);
        }

        // Устанавливает фокус на glControl при активации
        private void GameForm_Activated(object sender, EventArgs e)
        {
            glControl.Focus();
        }

        // Устанавливает фокус при показе формы
        private void GameForm_Shown(object sender, EventArgs e)
        {
            this.Activate();
            glControl.Focus();
            if (!glControl.Focused)
            {
                System.Threading.Thread.Sleep(100);
                glControl.Focus();
            }
        }

        // Инициализирует OpenGL и логику игры
        private void GlControl_Load(object sender, EventArgs e)
        {
            glControl.MakeCurrent();
            gameManager = new GameManager(map, selectedCharacterPlayer1, selectedCharacterPlayer2, isMusicEnabled, isSoundEnabled);
            renderer = new GameRenderer(map, gameManager);
            glControl.Focus();
        }

        // Обновляет положение меток при изменении размера окна
        private void GlControl_Resize(object sender, EventArgs e)
        {
            const float panelWidth = 16f * 5;
            const float panelHeight = 70f * 3.6f;
            const float margin = 10f;
            int halfWidth = glControl.ClientSize.Width / 2;

            int labelXOffsetPlayer1 = (int)(margin + (panelWidth - lblPlayer1Fruits.Width) / 2);
            lblPlayer1Fruits.Location = new Point(labelXOffsetPlayer1, (int)(margin + panelHeight - 5));
            int labelXOffsetPlayer2 = (int)(halfWidth + margin + (panelWidth - lblPlayer2Fruits.Width) / 2);
            lblPlayer2Fruits.Location = new Point(labelXOffsetPlayer2, (int)(margin + panelHeight - 5));

            glControl.Invalidate();
        }

        // Обновляет UI (счётчик фруктов)
        private void UpdateUI()
        {
            if (gameManager == null || gameManager._spriteRenderer == null) return;
            bool shouldShowLabels = !gameManager.IsStartScreenActive && !gameManager.IsGameOver;
            lblPlayer1Fruits.Visible = shouldShowLabels;
            lblPlayer2Fruits.Visible = shouldShowLabels;
            lblPlayer1Fruits.Text = $"{gameManager._spriteRenderer.ApplesBoxState}";
            lblPlayer2Fruits.Text = $"{gameManager._spriteRenderer.PearsBoxState}";
        }

        // Обновляет логику и вызывает перерисовку
        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            float deltaTime = lastFrameTime == default ? 0 : (float)(now - lastFrameTime).TotalSeconds;
            lastFrameTime = now;
            gameManager.Update(deltaTime,
                player1MoveUp, player1MoveDown, player1MoveLeft, player1MoveRight,
                player2MoveUp, player2MoveDown, player2MoveLeft, player2MoveRight,
                player1Interact, player2Interact, numberKeysPlayer1, numberKeysPlayer2);
            player1Interact = false;
            player2Interact = false;
            for (int i = 1; i <= 8; i++)
            {
                numberKeysPlayer1[i] = false;
                numberKeysPlayer2[i] = false;
            }
            UpdateUI();
            glControl.Invalidate();
        }

        // Рендерит поле с разделённым экраном
        private void GlControl_Paint(object sender, PaintEventArgs e)
        {
            glControl.MakeCurrent();
            GL.Clear(ClearBufferMask.ColorBufferBit);
            int w = glControl.ClientSize.Width;
            int h = glControl.ClientSize.Height;
            if (w <= 0 || h <= 0) return;
            int half = w / 2;
            int tilesX = map.Width / TileSize;
            int tilesY = map.Height / TileSize;
            int totalGrassTilesX = 11;
            int totalGrassTilesY = 17;
            int grassStartX = (tilesX - totalGrassTilesX) / 2;
            int grassStartY = (tilesY - totalGrassTilesY) / 2;
            float frameWidth = totalGrassTilesX * TileSize;
            float frameHeight = totalGrassTilesY * TileSize;
            float cameraX = grassStartX * TileSize - (half - frameWidth) / 2;
            float cameraY = grassStartY * TileSize - (h - frameHeight) / 2;

            var player1State = gameManager.Player1State;
            var player2State = gameManager.Player2State;
            var numberAnimationScalesPlayer1 = gameManager.InventoryHandler.GetNumberAnimationScales(1);
            var numberAnimationScalesPlayer2 = gameManager.InventoryHandler.GetNumberAnimationScales(2);

            GL.Viewport(0, 0, half, h);
            renderer.Render(half, h, map, cameraX, cameraY, 1, player1State, numberAnimationScalesPlayer1);

            GL.Viewport(half, 0, w - half, h);
            renderer.Render(w - half, h, map, cameraX, cameraY, 2, player2State, numberAnimationScalesPlayer2);

            renderer.RenderSplitLine(w, h);
            glControl.SwapBuffers();
        }

        // Обрабатывает клик мыши для возврата в меню после игры
        private async void GlControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && gameManager.IsGameOver)
            {
                int w = glControl.ClientSize.Width;
                int h = glControl.ClientSize.Height;
                int half = w / 2;
                if (e.X < half && renderer.IsMouseOverMenuButton(e.X, e.Y, half, h))
                {
                    renderer.TriggerMenuButtonAnimation();
                    await Task.Delay(300);
                    CleanupAndClose(true);
                }
            }
        }

        // Обрабатывает клавиши для игрока 2 (стрелки и Enter)
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Up) { player2MoveUp = true; return true; }
            else if (keyData == Keys.Down) { player2MoveDown = true; return true; }
            else if (keyData == Keys.Left) { player2MoveLeft = true; return true; }
            else if (keyData == Keys.Right) { player2MoveRight = true; return true; }
            else if (keyData == Keys.Enter) { player2Interact = true; return true; }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // Обрабатывает нажатия клавиш для игроков
        private void GlControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) player1MoveUp = true;
            else if (e.KeyCode == Keys.S) player1MoveDown = true;
            else if (e.KeyCode == Keys.A) player1MoveLeft = true;
            else if (e.KeyCode == Keys.D) player1MoveRight = true;
            else if (e.KeyCode == Keys.E) player1Interact = true;
            else if (e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D8) numberKeysPlayer1[e.KeyCode - Keys.D0] = true;
            else if (e.KeyCode >= Keys.NumPad1 && e.KeyCode <= Keys.NumPad8) numberKeysPlayer2[e.KeyCode - Keys.NumPad0] = true;
            e.Handled = true;
        }

        // Обрабатывает отпускание клавиш
        private void GlControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) player1MoveUp = false;
            else if (e.KeyCode == Keys.S) player1MoveDown = false;
            else if (e.KeyCode == Keys.A) player1MoveLeft = false;
            else if (e.KeyCode == Keys.D) player1MoveRight = false;
            else if (e.KeyCode == Keys.Up) player2MoveUp = false;
            else if (e.KeyCode == Keys.Down) player2MoveDown = false;
            else if (e.KeyCode == Keys.Left) player2MoveLeft = false;
            else if (e.KeyCode == Keys.Right) player2MoveRight = false;
            e.Handled = true;
        }

        private void GlControl_GotFocus(object sender, EventArgs e) { Console.WriteLine("glControl: Получен фокус"); }
        private void GlControl_LostFocus(object sender, EventArgs e) { Console.WriteLine("glControl: Потерян фокус"); }

        // Очищает ресурсы и закрывает форму
        private void CleanupAndClose(bool returnToMainMenu)
        {
            _isReturningToMainMenu = returnToMainMenu;
            gameManager.StopAndDisposeAudio();
            renderTimer?.Stop();
            renderTimer?.Dispose();
            renderer = null;
            if (returnToMainMenu)
            {
                mainForm.Show();
                mainForm.ResumeMusic();
            }
            this.Close();
        }

        // Закрывает приложение, если не возвращаемся в меню
        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isReturningToMainMenu) mainForm.Close();
        }
    }
}