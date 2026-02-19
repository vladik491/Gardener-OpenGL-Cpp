using Gardener.gameClasses.Audio;
using Gardener.gameClasses.Rendering;
using OpenTK.Graphics.OpenGL;

namespace Gardener.gameClasses.ScreenManagement
{
    // Экран начала игры с обратным отсчётом
    public class GameStartScreen : IScreen
    {
        // Блок: Зависимости и ресурсы
        private readonly TextureRegistry _textureRegistry; 
        private readonly AudioManager _audioManager; 
        private readonly int[] _countdownTextureIds; // Текстуры для отсчёта (3, 2, 1, GO!)

        // Блок: Состояние отсчёта
        private bool _isCountdownActive; 
        private float _countdownTimer; 
        private int _currentCountdownStep; // Текущий шаг отсчёта (0-3)

        // Блок: Константы
        private const float CountdownStepDuration = 1f;
        private const float FadeInDuration = 0.3f; 
        private const float FadeOutDuration = 0.3f; 

        // Свойство: Активен ли экран
        public bool IsActive => _isCountdownActive;

        // Конструктор: Инициализирует экран начала игры
        public GameStartScreen(TextureRegistry textureRegistry, AudioManager audioManager)
        {
            _textureRegistry = textureRegistry ?? throw new ArgumentNullException(nameof(textureRegistry));
            _audioManager = audioManager ?? throw new ArgumentNullException(nameof(audioManager));

            _isCountdownActive = true;
            _countdownTimer = 0f;
            _currentCountdownStep = 0;

            _countdownTextureIds = new int[4]
            {
                _textureRegistry.ThreeStartTextureId, // Текстура 3
                _textureRegistry.TwoStartTextureId,   // Текстура 2
                _textureRegistry.OneStartTextureId,   // Текстура 1
                _textureRegistry.GoStartTextureId     // Текстура GO!
            };

            _audioManager.Play("start"); // Проигрывание звука начала
        }

        // Обновляет состояние отсчёта
        public void Update(float deltaTime)
        {
            if (!_isCountdownActive) return;

            _countdownTimer += deltaTime;

            if (_countdownTimer >= CountdownStepDuration)
            {
                _countdownTimer = 0f;
                _currentCountdownStep++;
                if (_currentCountdownStep >= 4)
                {
                    _isCountdownActive = false;
                }
            }
        }

        /// <summary>
        /// Рендерит экран начала игры с анимацией отсчёта.
        /// </summary>
        /// <param name="width">Ширина экрана</param>
        /// <param name="height">Высота экрана</param>
        /// <param name="playerId">ID игрока</param>
        public void Render(int width, int height, int playerId)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, width, height, 0, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            // Рендеринг затемнённого фона (#2e2a5b с прозрачностью 50%)
            GL.Disable(EnableCap.Texture2D);
            GL.Color4(46f / 255f, 42f / 255f, 91f / 255f, 0.5f);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(0, 0);
            GL.Vertex2(width, 0);
            GL.Vertex2(width, height);
            GL.Vertex2(0, height);
            GL.End();
            GL.Color4(1f, 1f, 1f, 1f);

            // Рендеринг текущей цифры или GO!
            if (_currentCountdownStep < 4)
            {
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, _countdownTextureIds[_currentCountdownStep]);

                // Эффект затухания/появления
                float alpha = 1f;
                if (_countdownTimer < FadeInDuration)
                {
                    alpha = _countdownTimer / FadeInDuration;
                }
                else if (_countdownTimer > CountdownStepDuration - FadeOutDuration)
                {
                    alpha = (CountdownStepDuration - _countdownTimer) / FadeOutDuration;
                }

                float texWidth, texHeight;
                if (_currentCountdownStep < 3)
                {
                    texWidth = 21f * 8f;  // Размеры для цифр (3, 2, 1)
                    texHeight = 27f * 8f;
                }
                else
                {
                    texWidth = 30f * 8f;  // Размеры для GO!
                    texHeight = 18f * 8f;
                }

                float posX = (width - texWidth) / 2f;
                float posY = (height - texHeight) / 2f;

                GL.Color4(1f, 1f, 1f, alpha);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0f, 0f); GL.Vertex2(posX, posY);
                GL.TexCoord2(1f, 0f); GL.Vertex2(posX + texWidth, posY);
                GL.TexCoord2(1f, 1f); GL.Vertex2(posX + texWidth, posY + texHeight);
                GL.TexCoord2(0f, 1f); GL.Vertex2(posX, posY + texHeight);
                GL.End();
                GL.Color4(1f, 1f, 1f, 1f);
            }

            GL.Disable(EnableCap.Blend);
        }
    }
}