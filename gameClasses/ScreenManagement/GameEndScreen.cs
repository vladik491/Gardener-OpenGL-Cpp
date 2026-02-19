using Gardener.gameClasses.Audio;
using Gardener.gameClasses.Rendering;
using NAudio.Wave;
using OpenTK.Graphics.OpenGL;
using TextRenderer = Gardener.gameClasses.Rendering.TextRenderer;

namespace Gardener.gameClasses.ScreenManagement
{
    // Экран окончания игры с титрами и анимацией победителя
    public class GameEndScreen : IScreen, IDisposable
    {
        // Блок: Зависимости и ресурсы
        private readonly TextureRegistry _textureRegistry; 
        private readonly AudioManager _audioManager;
        private readonly int _selectedCharacterPlayer1; 
        private readonly int _selectedCharacterPlayer2; 
        private readonly (float texLeft, float texRight, float texTop, float texBottom)[] _characterFrames; 
        private readonly TextRenderer _creditsRenderer; 

        // Блок: Состояние экрана
        private bool _isGameOver; 
        private int _winnerPlayerId; 
        private bool _victorySoundPlayed; 
        private float _menuButtonScale;
        private float _menuButtonAnimationTimer; 
        private float _characterAnimationTimer;
        private int _currentCharacterFrame; 
        private bool _creditsStarted; 
        private bool _creditsFinished; 
        private bool _isDisposed;
        private float _victorySoundDelayTimer;
        private bool _waitingForVictorySound;

        // Блок: Константы
        private const float ButtonAnimationDuration = 0.3f; 
        private const float ButtonAnimationScale = 0.95f;
        private const float FrameDuration = 0.1f; 
        private const int TotalFrames = 6; 
        private const string CreditsFontPath = "Monocraft1.ttf"; 
        private const float CreditsFontSize = 20f;
        private const float CreditsScrollSpeed = 65.0f;
        private const float VictorySoundDuration = 1.0f;
        private float _creditsInitialYOffset = 1080f; 

        // Свойство: Активен ли экран
        public bool IsActive => _isGameOver;

        // Конструктор: Инициализирует экран окончания игры
        public GameEndScreen(
            TextureRegistry textureRegistry,
            AudioManager audioManager,
            int selectedCharacterPlayer1,
            int selectedCharacterPlayer2)
        {
            _textureRegistry = textureRegistry ?? throw new ArgumentNullException(nameof(textureRegistry));
            _audioManager = audioManager ?? throw new ArgumentNullException(nameof(audioManager));
            _selectedCharacterPlayer1 = selectedCharacterPlayer1;
            _selectedCharacterPlayer2 = selectedCharacterPlayer2;

            // Подписываемся на событие завершения soundOfVictory
            _audioManager.VictorySoundFinished += (s, e) =>
            {
                StartCredits();
                _audioManager.Play("credits"); // Запускаем музыку титров
            };

            try
            {
                var fontProvider = new FontProvider(CreditsFontPath, CreditsFontSize);
                var textureGenerator = new SystemDrawingTextTextureGenerator(fontProvider);
                _creditsRenderer = new TextRenderer(textureGenerator);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GameEndScreen] Ошибка инициализации TextRenderer: {ex.Message}");
                _creditsRenderer = null;
            }

            _isGameOver = false;
            _winnerPlayerId = 0;
            _victorySoundPlayed = false;
            _menuButtonScale = 1f;
            _menuButtonAnimationTimer = 0f;
            _characterAnimationTimer = 0f;
            _currentCharacterFrame = 0;
            _creditsStarted = false;
            _creditsFinished = false;

            _characterFrames = new (float, float, float, float)[TotalFrames];
            const float spriteWidth = 18f;
            const float textureWidth = 108f;
            for (int i = 0; i < TotalFrames; i++)
            {
                float texLeft = (i * spriteWidth) / textureWidth;
                float texRight = ((i + 1) * spriteWidth) / textureWidth;
                _characterFrames[i] = (texLeft, texRight, 0f, 1f);
            }
        }

        // Устанавливает состояние окончания игры
        public void SetGameOver(int winnerPlayerId)
        {
            if (_isGameOver) return;

            _isGameOver = true;
            _winnerPlayerId = winnerPlayerId;

            if (!_victorySoundPlayed)
            {
                _audioManager.PlayVictorySequence();
                _victorySoundPlayed = true;

                if (!_audioManager.IsMusicEnabled)
                {
                    // Если музыка отключена, начинаем отсчёт задержки
                    _waitingForVictorySound = true;
                    _victorySoundDelayTimer = VictorySoundDuration;
                }
            }
        }

        // Запускает прокрутку титров
        private void StartCredits()
        {
            if (_creditsRenderer == null || _creditsStarted) return;

            string creditsText = "Над проектом работали\n\n" +
                     "Главный гейм-дизайнер\nКалинин Владислав Михайлович\n\n" +
                     "Ведущий программист\nКалинин Владислав Михайлович\n\n" +
                     "Художник-концептолог\nКалинин Владислав Михайлович\n\n" +
                     "Аниматор\nКалинин Владислав Михайлович\n\n" +
                     "Звукорежиссёр\nКалинин Владислав Михайлович\n\n" +
                     "Сценарист\nКалинин Владислав Михайлович\n\n" +
                     "Главный тестировщик\nКалинина Юлия Михайловна\n\n" + 
                     "Дизайнер уровней\nКалинин Владислав Михайлович\n\n" +
                     "Программист геймплея\nКалинин Владислав Михайлович\n\n" +
                     "UI/UX-дизайнер\nКалинин Владислав Михайлович\n\n" +
                     "Продюсер\nКалинин Владислав Михайлович\n\n" +
                     "Главный критик\nРудченко Ярослав Дмитриевич\n\n" + 
                     "\nСпециальное спасибо\nГГТУ им. П. О. Сухого\n" +
                     "Богословскому Ивану Николаевичу\n" +
                     "Леонову Павлу Владимировичу\n\n\n" +
                     "Проект создан студией\n\"Two Rings\"\n\n";

            bool startedOk = _creditsRenderer.StartCredits(
                creditsText,
                CreditsScrollSpeed,
                _creditsInitialYOffset,
                Color.FromArgb(230, 230, 230),
                Color.Transparent
            );

            if (startedOk)
            {
                _creditsStarted = true;
            }
            else
            {
                Console.WriteLine("[GameEndScreen] Не удалось запустить титры.");
            }
        }

        // Обновляет состояние экрана
        public void Update(float deltaTime)
        {
            if (!_isGameOver || _isDisposed) return;

            // Обрабатываем задержку, если музыка отключена
            if (_waitingForVictorySound)
            {
                _victorySoundDelayTimer -= deltaTime;
                if (_victorySoundDelayTimer <= 0)
                {
                    _waitingForVictorySound = false;
                    StartCredits();
                }
            }

            if (_menuButtonAnimationTimer > 0)
            {
                _menuButtonAnimationTimer -= deltaTime;
                if (_menuButtonAnimationTimer <= 0)
                {
                    _menuButtonAnimationTimer = 0;
                    _menuButtonScale = 1f;
                }
                else
                {
                    float t = 1 - (_menuButtonAnimationTimer / ButtonAnimationDuration);
                    float pulse = (float)Math.Sin(t * Math.PI);
                    _menuButtonScale = 1f - pulse * (1f - ButtonAnimationScale);
                }
            }

            _characterAnimationTimer += deltaTime;
            if (_characterAnimationTimer >= FrameDuration)
            {
                _characterAnimationTimer -= FrameDuration;
                _currentCharacterFrame = (_currentCharacterFrame + 1) % TotalFrames;
            }

            _creditsRenderer?.UpdateCredits(deltaTime, _creditsInitialYOffset);

            if (!_creditsFinished && _creditsStarted && _creditsRenderer != null && _creditsRenderer.AreCreditsFinished())
            {
                _creditsFinished = true;
            }
        }

        /// <summary>
        /// Рендерит экран окончания игры: победителя, анимацию персонажа и титры.
        /// </summary>
        /// <param name="width">Ширина экрана</param>
        /// <param name="height">Высота экрана</param>
        /// <param name="playerId">ID игрока</param>
        public void Render(int width, int height, int playerId)
        {
            if (!_isGameOver || _isDisposed) return;

            if (_creditsInitialYOffset != height)
            {
                _creditsInitialYOffset = height;
            }

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, width, height, 0, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            // Рендеринг затемнённого фона
            GL.Disable(EnableCap.Texture2D);
            GL.Color4(46f / 255f, 42f / 255f, 91f / 255f, 0.5f);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(0, 0);
            GL.Vertex2(width, 0);
            GL.Vertex2(width, height);
            GL.Vertex2(0, height);
            GL.End();
            GL.Color4(1f, 1f, 1f, 1f);

            GL.Enable(EnableCap.Texture2D);

            if (playerId == 1)
            {
                float margin = 40f;

                float winnerWidth = 55f * 10f;
                float winnerHeight = 9f * 10f;
                float charHeight = _winnerPlayerId == 1
                    ? _selectedCharacterPlayer1 switch
                    {
                        1 => 28f * 11f,
                        2 => 21f * 14f,
                        3 => 22f * 14f,
                        _ => 28f * 11f
                    }
                    : _selectedCharacterPlayer2 switch
                    {
                        1 => 28f * 11f,
                        2 => 21f * 14f,
                        3 => 22f * 14f,
                        _ => 28f * 11f
                    };
                float charWidth = 18f * 12f;
                float playerTextWidth = 37f * 10f;
                float playerTextHeight = 9f * 10f;
                float buttonWidth = 76f * 8f;
                float buttonHeight = 25f * 8f;

                float winnerPosX = (width - winnerWidth) / 2f;
                float winnerPosY = margin;
                float charPosX = (width - charWidth) / 2f;
                float charPosY = winnerPosY + winnerHeight + margin * 1.5f;
                float playerTextPosX = (width - playerTextWidth) / 2f;
                float buttonPosY = height - buttonHeight - margin;
                float playerTextPosY = buttonPosY - playerTextHeight - margin * 0.75f;
                float buttonPosX = (width - buttonWidth) / 2f;

                // Рендеринг надписи "Winner"
                GL.BindTexture(TextureTarget.Texture2D, _textureRegistry.WinnerTextureId);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0f, 0f); GL.Vertex2(winnerPosX, winnerPosY);
                GL.TexCoord2(1f, 0f); GL.Vertex2(winnerPosX + winnerWidth, winnerPosY);
                GL.TexCoord2(1f, 1f); GL.Vertex2(winnerPosX + winnerWidth, winnerPosY + winnerHeight);
                GL.TexCoord2(0f, 1f); GL.Vertex2(winnerPosX, winnerPosY + winnerHeight);
                GL.End();

                // Рендеринг персонажа победителя
                int characterTextureId = _winnerPlayerId == 1
                    ? _selectedCharacterPlayer1 switch
                    {
                        1 => _textureRegistry.DownRightJewTextureId,
                        2 => _textureRegistry.DownRightBlackTextureId,
                        3 => _textureRegistry.DownRightJapanTextureId,
                        _ => _textureRegistry.DownRightJewTextureId
                    }
                    : _selectedCharacterPlayer2 switch
                    {
                        1 => _textureRegistry.DownRightJewTextureId,
                        2 => _textureRegistry.DownRightBlackTextureId,
                        3 => _textureRegistry.DownRightJapanTextureId,
                        _ => _textureRegistry.DownRightJewTextureId
                    };

                GL.BindTexture(TextureTarget.Texture2D, characterTextureId);
                var frame = _characterFrames[_currentCharacterFrame];
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(frame.texLeft, frame.texTop); GL.Vertex2(charPosX, charPosY);
                GL.TexCoord2(frame.texRight, frame.texTop); GL.Vertex2(charPosX + charWidth, charPosY);
                GL.TexCoord2(frame.texRight, frame.texBottom); GL.Vertex2(charPosX + charWidth, charPosY + charHeight);
                GL.TexCoord2(frame.texLeft, frame.texBottom); GL.Vertex2(charPosX, charPosY + charHeight);
                GL.End();

                // Рендеринг текста игрока (Player 1 или Player 2)
                int playerTextTextureId = _winnerPlayerId == 1 ? _textureRegistry.Player1TextureId : _textureRegistry.Player2TextureId;
                GL.BindTexture(TextureTarget.Texture2D, playerTextTextureId);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0f, 0f); GL.Vertex2(playerTextPosX, playerTextPosY);
                GL.TexCoord2(1f, 0f); GL.Vertex2(playerTextPosX + playerTextWidth, playerTextPosY);
                GL.TexCoord2(1f, 1f); GL.Vertex2(playerTextPosX + playerTextWidth, playerTextPosY + playerTextHeight);
                GL.TexCoord2(0f, 1f); GL.Vertex2(playerTextPosX, playerTextPosY + playerTextHeight);
                GL.End();

                // Рендеринг кнопки "Главное меню" с анимацией
                GL.BindTexture(TextureTarget.Texture2D, _textureRegistry.GlavnoeMenuTextureId);
                GL.PushMatrix();
                float buttonCenterX = buttonPosX + buttonWidth / 2f;
                float buttonCenterY = buttonPosY + buttonHeight / 2f;
                GL.Translate(buttonCenterX, buttonCenterY, 0);
                GL.Scale(_menuButtonScale, _menuButtonScale, 1f);
                GL.Translate(-buttonCenterX, -buttonCenterY, 0);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0f, 0f); GL.Vertex2(buttonPosX, buttonPosY);
                GL.TexCoord2(1f, 0f); GL.Vertex2(buttonPosX + buttonWidth, buttonPosY);
                GL.TexCoord2(1f, 1f); GL.Vertex2(buttonPosX + buttonWidth, buttonPosY + buttonHeight);
                GL.TexCoord2(0f, 1f); GL.Vertex2(buttonPosX, buttonPosY + buttonHeight);
                GL.End();
                GL.PopMatrix();
            }
            else if (playerId == 2)
            {
                if (!_creditsFinished)
                {
                    // Отрисовываем титры, пока они не закончились
                    _creditsRenderer?.RenderCredits(0, width);
                }
                else
                {
                    // После завершения титров отображаем "Спасибо, что играли :)"
                    string thankYouText = "Спасибо, что играли :)";
                    float textPosX = width / 2f;
                    float textPosY = height / 2f;
                    _creditsRenderer?.RenderText(
                        thankYouText,
                        textPosX,
                        textPosY,
                        Color.FromArgb(230, 230, 230),
                        Color.Transparent,
                        TextAlignment.Center
                    );
                }
            }

            GL.Disable(EnableCap.Blend);
            GL.Color4(1f, 1f, 1f, 1f);
        }

        // Проверяет, находится ли курсор мыши над кнопкой меню
        public bool IsMouseOverMenuButton(int mouseX, int mouseY, int width, int height)
        {
            if (!_isGameOver || _isDisposed) return false;

            float margin = 40f;
            float buttonWidth = 76f * 8f;
            float buttonHeight = 25f * 8f;
            float buttonPosX = (width - buttonWidth) / 2f;
            float buttonPosY = height - buttonHeight - margin;

            return mouseX >= buttonPosX && mouseX <= buttonPosX + buttonWidth &&
                   mouseY >= buttonPosY && mouseY <= buttonPosY + buttonHeight;
        }

        // Запускает анимацию кнопки меню
        public void TriggerMenuButtonAnimation()
        {
            if (_isDisposed) return;

            _menuButtonAnimationTimer = ButtonAnimationDuration;
            _menuButtonScale = 1f;

            _audioManager.Play("button");
            _audioManager.Stop("credits");

            if (_audioManager.GetPlayer("button", out var buttonPlayer))
            {
                EventHandler<StoppedEventArgs> handler = null;
                handler = (s, e) =>
                {
                    buttonPlayer.PlaybackStopped -= handler;
                };
                buttonPlayer.PlaybackStopped += handler;
            }
        }

        // Утилизирует ресурсы
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Внутренний метод утилизации
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                _creditsRenderer?.Dispose();
            }

            _isDisposed = true;
        }

        // Финализатор
        ~GameEndScreen()
        {
            Dispose(false);
        }
    }
}