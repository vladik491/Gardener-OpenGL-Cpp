using System.Diagnostics;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using PixelFormatGL = OpenTK.Graphics.OpenGL.PixelFormat;
using PixelFormatSD = System.Drawing.Imaging.PixelFormat;

namespace Gardener.gameClasses.Rendering
{
    // Интерфейс для генерации текстур текста
    public interface ITextTextureGenerator : IDisposable
    {
        // Генерирует текстуру текста
        int GenerateTexture(string text);
        int TextureWidth { get; }
        int TextureHeight { get; }
        Color TextColor { get; set; }
        Color BackgroundColor { get; set; }
    }

    // Поставщик шрифтов
    public class FontProvider : IDisposable
    {
        private Font _font; 
        public bool _isDisposed; 

        // Конструктор: Загружает шрифт из файла
        public FontProvider(string fontPath, float fontSize)
        {
            if (string.IsNullOrWhiteSpace(fontPath)) throw new ArgumentException("Font path cannot be null or empty.", nameof(fontPath));
            try
            {
                var pfc = new System.Drawing.Text.PrivateFontCollection();
                pfc.AddFontFile(fontPath);
                _font = new Font(pfc.Families[0], fontSize);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[FontProvider] Failed to load font '{fontPath}': {ex.Message}. Using default font.");
                _font = new Font(FontFamily.GenericSansSerif, fontSize);
            }
        }

        // Свойство: Доступ к шрифту
        public Font Font => _isDisposed ? throw new ObjectDisposedException(nameof(FontProvider)) : _font;

        // Утилизирует ресурсы
        public void Dispose()
        {
            if (_isDisposed) return;
            _font?.Dispose();
            _isDisposed = true;
        }
    }

    // Генератор текстур текста с использованием System.Drawing
    public class SystemDrawingTextTextureGenerator : ITextTextureGenerator
    {
        // Блок: Зависимости и состояние
        private readonly FontProvider _fontProvider; 
        private int _textureWidth; 
        private int _textureHeight; 
        private bool _isDisposed; 
        private Color _textColor = Color.White; 
        private Color _backgroundColor = Color.Transparent; 

        // Блок: Константы
        private const PixelFormatSD BitmapPixelFormat = PixelFormatSD.Format32bppArgb; // Формат пикселей для битмапа

        // Конструктор: Инициализирует генератор текстур
        public SystemDrawingTextTextureGenerator(FontProvider fontProvider)
        {
            _fontProvider = fontProvider ?? throw new ArgumentNullException(nameof(fontProvider));
        }

        // Свойство: Цвет текста и другие
        public Color TextColor { get => _textColor; set => _textColor = value; }
        public Color BackgroundColor { get => _backgroundColor; set => _backgroundColor = value; }
        public int TextureWidth => _textureWidth;
        public int TextureHeight => _textureHeight;

        /// <summary>
        /// Генерирует текстуру текста и возвращает её OpenGL ID.
        /// </summary>
        /// <param name="text">Текст для генерации</param>
        /// <returns>ID текстуры или -1 в случае ошибки</returns>
        public int GenerateTexture(string text)
        {
            if (_isDisposed) throw new ObjectDisposedException(nameof(SystemDrawingTextTextureGenerator));
            if (string.IsNullOrEmpty(text))
            {
                _textureWidth = 0;
                _textureHeight = 0;
                return -1;
            }

            SizeF textSize = MeasureText(text);
            if (textSize.Width <= 0 || textSize.Height <= 0)
            {
                _textureWidth = 0;
                _textureHeight = 0;
                return -1;
            }

            _textureWidth = (int)Math.Ceiling(textSize.Width);
            _textureHeight = (int)Math.Ceiling(textSize.Height) + 2;

            int textureId = -1;
            using (var bitmap = new Bitmap(_textureWidth, _textureHeight, BitmapPixelFormat))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                    graphics.Clear(_backgroundColor);
                    using (var textBrush = new SolidBrush(_textColor))
                    {
                        graphics.DrawString(text, _fontProvider.Font, textBrush, new PointF(0, 0));
                    }
                }

                BitmapData bitmapData = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly,
                    BitmapPixelFormat);

                try
                {
                    textureId = GL.GenTexture();
                    if (textureId <= 0)
                    {
                        Debug.WriteLine("[SystemDrawingTextTextureGenerator] Failed to generate OpenGL texture ID.");
                        _textureWidth = 0;
                        _textureHeight = 0;
                        return -1;
                    }

                    GL.BindTexture(TextureTarget.Texture2D, textureId);
                    GL.TexImage2D(
                        TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                        bitmapData.Width, bitmapData.Height, 0,
                        PixelFormatGL.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);

                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[SystemDrawingTextTextureGenerator] Error during texture creation: {ex.Message}");
                    if (textureId > 0) GL.DeleteTexture(textureId);
                    textureId = -1;
                    _textureWidth = 0;
                    _textureHeight = 0;
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                    GL.BindTexture(TextureTarget.Texture2D, 0);
                }
            }

            return textureId;
        }

        // Измеряет размер текста
        private SizeF MeasureText(string text)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(SystemDrawingTextTextureGenerator), "Невозможно измерить текст, так как объект был утилизирован.");
            }
            if (_fontProvider._isDisposed)
            {
                throw new ObjectDisposedException(nameof(FontProvider), "Невозможно измерить текст, так как FontProvider был удален.");
            }
            using (var bitmap = new Bitmap(1, 1, BitmapPixelFormat))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                return graphics.MeasureString(text, _fontProvider.Font);
            }
        }

        // Утилизирует ресурсы
        public void Dispose()
        {
            if (_isDisposed) return;
            _fontProvider?.Dispose();
            _isDisposed = true;
        }
    }

    // Рендерер текста
    public class TextRenderer : IDisposable
    {
        // Блок: Зависимости и состояние
        private readonly ITextTextureGenerator _textureGenerator; 
        private int _staticTextureId = -1; 
        private int _staticTextureWidth; 
        private int _staticTextureHeight; 

        // Блок: Титры
        private readonly List<(int textureId, int width, int height, float yOffset)> _creditsLines; // Список строк титров
        private float _scrollPositionY; 
        private float _scrollSpeed; 
        private bool _isCreditsActive;
        private float _creditsContainerHeight;
        private float _totalCreditsHeight;

        // Блок: Текст "Спасибо, что играли :)"
        private int _thankYouTextureId = -1; 
        private int _thankYouTextureWidth;
        private int _thankYouTextureHeight;

        private bool _isDisposed; // Флаг утилизации

        // Конструктор: Инициализирует рендерер текста
        public TextRenderer(ITextTextureGenerator textureGenerator)
        {
            _textureGenerator = textureGenerator ?? throw new ArgumentNullException(nameof(textureGenerator));
            _creditsLines = new List<(int, int, int, float)>();
        }

        // Обновляет статический текст
        public bool UpdateText(string text)
        {
            if (_isDisposed) throw new ObjectDisposedException(nameof(TextRenderer));

            DeleteTexture(ref _staticTextureId);

            _staticTextureId = _textureGenerator.GenerateTexture(text);
            if (_staticTextureId == -1)
            {
                _staticTextureWidth = 0;
                _staticTextureHeight = 0;
                return false;
            }

            _staticTextureWidth = _textureGenerator.TextureWidth;
            _staticTextureHeight = _textureGenerator.TextureHeight;
            return true;
        }

        // Рендерит статический текст
        public void Render(float x, float y, Color? tint = null)
        {
            RenderInternal(_staticTextureId, _staticTextureWidth, _staticTextureHeight, x, y, tint);
        }

        /// <summary>
        /// Запускает прокрутку титров.
        /// </summary>
        /// <param name="creditsContent">Текст титров</param>
        /// <param name="speed">Скорость прокрутки</param>
        /// <param name="startPositionOffsetY">Начальное смещение по Y</param>
        /// <param name="textColor">Цвет текста (опционально)</param>
        /// <param name="backgroundColor">Цвет фона (опционально)</param>
        /// <returns>True, если титры успешно запущены</returns>
        public bool StartCredits(string creditsContent, float speed, float startPositionOffsetY, Color? textColor = null, Color? backgroundColor = null)
        {
            if (_isDisposed) throw new ObjectDisposedException(nameof(TextRenderer));

            StopCredits();

            if (string.IsNullOrEmpty(creditsContent))
            {
                _isCreditsActive = false;
                return false;
            }

            _scrollSpeed = speed;
            _scrollPositionY = startPositionOffsetY;
            _creditsContainerHeight = startPositionOffsetY;

            Color originalTextColor = _textureGenerator.TextColor;
            Color originalBgColor = _textureGenerator.BackgroundColor;
            _textureGenerator.TextColor = textColor ?? Color.White;
            _textureGenerator.BackgroundColor = backgroundColor ?? Color.Transparent;

            // Разбиваем текст на строки и создаём текстуру для каждой строки
            string[] lines = creditsContent.Split(new[] { "\n" }, StringSplitOptions.None);
            float currentYOffset = 0;
            _creditsLines.Clear();

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine))
                {
                    currentYOffset += 20f; // Отступ для пустых строк
                    continue;
                }

                int textureId = _textureGenerator.GenerateTexture(trimmedLine);
                if (textureId == -1)
                {
                    Debug.WriteLine($"[TextRenderer] Failed to generate texture for line: {trimmedLine}");
                    continue;
                }

                int width = _textureGenerator.TextureWidth;
                int height = _textureGenerator.TextureHeight;
                _creditsLines.Add((textureId, width, height, currentYOffset));
                currentYOffset += height + 5f; // Отступ между строками
            }

            _totalCreditsHeight = currentYOffset;

            // Генерируем текстуру для "Спасибо, что играли :)"
            _thankYouTextureId = _textureGenerator.GenerateTexture("Спасибо, что играли :)");
            if (_thankYouTextureId != -1)
            {
                _thankYouTextureWidth = _textureGenerator.TextureWidth;
                _thankYouTextureHeight = _textureGenerator.TextureHeight;
            }
            else
            {
                _thankYouTextureWidth = 0;
                _thankYouTextureHeight = 0;
            }

            _textureGenerator.TextColor = originalTextColor;
            _textureGenerator.BackgroundColor = originalBgColor;

            if (_creditsLines.Count == 0)
            {
                _isCreditsActive = false;
                return false;
            }

            _isCreditsActive = true;
            return true;
        }

        // Обновляет состояние титров
        public void UpdateCredits(float deltaTime, float containerHeight)
        {
            if (!_isCreditsActive || _isDisposed) return;

            _creditsContainerHeight = containerHeight;
            _scrollPositionY -= _scrollSpeed * deltaTime;
        }

        // Проверяет, закончились ли титры
        public bool AreCreditsFinished()
        {
            if (!_isCreditsActive || _isDisposed) return true;

            // Титры закончены, если нижняя часть последней строки ушла за верх контейнера
            float bottomOfCredits = _scrollPositionY + _totalCreditsHeight;
            return bottomOfCredits < 0;
        }

        // Рендерит титры
        public void RenderCredits(float containerX, float containerWidth, Color? tint = null)
        {
            if (!_isCreditsActive || _creditsLines.Count == 0) return;

            foreach (var (textureId, width, height, yOffset) in _creditsLines)
            {
                float renderY = _scrollPositionY + yOffset;
                // Пропускаем строки вне области видимости
                if (renderY + height < 0 || renderY > _creditsContainerHeight)
                    continue;

                float renderX = containerX + (containerWidth - width) / 2.0f;
                RenderInternal(textureId, width, height, renderX, renderY, tint);
            }
        }

        // Рендерит текст с заданным выравниванием
        public void RenderText(string text, float x, float y, Color textColor, Color backgroundColor, TextAlignment alignment)
        {
            if (_isDisposed) return;

            // Используем заранее сгенерированную текстуру для "Спасибо, что играли :)"
            if (text == "Спасибо, что играли :)" && _thankYouTextureId != -1)
            {
                float renderX = x;
                if (alignment == TextAlignment.Center)
                {
                    renderX = x - _thankYouTextureWidth / 2.0f;
                }
                else if (alignment == TextAlignment.Right)
                {
                    renderX = x - _thankYouTextureWidth;
                }

                RenderInternal(_thankYouTextureId, _thankYouTextureWidth, _thankYouTextureHeight, renderX, y, textColor);
                return;
            }

            // Генерируем текстуру на лету для другого текста
            Color originalTextColor = _textureGenerator.TextColor;
            Color originalBgColor = _textureGenerator.BackgroundColor;
            _textureGenerator.TextColor = textColor;
            _textureGenerator.BackgroundColor = backgroundColor;

            int tempTextureId = _textureGenerator.GenerateTexture(text);
            int tempWidth = _textureGenerator.TextureWidth;
            int tempHeight = _textureGenerator.TextureHeight;

            if (tempTextureId != -1)
            {
                float renderX = x;
                if (alignment == TextAlignment.Center)
                {
                    renderX = x - tempWidth / 2.0f;
                }
                else if (alignment == TextAlignment.Right)
                {
                    renderX = x - tempWidth;
                }

                RenderInternal(tempTextureId, tempWidth, tempHeight, renderX, y, textColor);
                DeleteTexture(ref tempTextureId);
            }

            _textureGenerator.TextColor = originalTextColor;
            _textureGenerator.BackgroundColor = originalBgColor;
        }

        // Останавливает титры
        public void StopCredits()
        {
            if (_isDisposed) return;
            _isCreditsActive = false;
            foreach (var (textureId, _, _, _) in _creditsLines)
            {
                int tempId = textureId;
                DeleteTexture(ref tempId);
            }
            _creditsLines.Clear();
            _totalCreditsHeight = 0;
        }

        // Внутренний метод рендеринга текстуры
        private void RenderInternal(int textureId, int texWidth, int texHeight, float x, float y, Color? tint)
        {
            if (_isDisposed || textureId == -1 || texWidth <= 0 || texHeight <= 0) return;

            Color renderColor = tint ?? Color.White;

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            GL.Color4(renderColor.R / 255f, renderColor.G / 255f, renderColor.B / 255f, renderColor.A / 255f);

            float x2 = x + texWidth;
            float y2 = y + texHeight;

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0); GL.Vertex2(x, y);
            GL.TexCoord2(1, 0); GL.Vertex2(x2, y);
            GL.TexCoord2(1, 1); GL.Vertex2(x2, y2);
            GL.TexCoord2(0, 1); GL.Vertex2(x, y2);
            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Blend);
            GL.Color4(Color.White);
        }

        // Удаляет текстуру
        private void DeleteTexture(ref int textureId)
        {
            if (textureId == -1) return;
            try
            {
                GL.DeleteTexture(textureId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TextRenderer] Error deleting texture {textureId}: {ex.Message}");
            }
            textureId = -1;
        }

        // Утилизирует ресурсы
        public void Dispose()
        {
            if (_isDisposed) return;

            _textureGenerator?.Dispose();
            DeleteTexture(ref _staticTextureId);
            StopCredits();
            DeleteTexture(ref _thankYouTextureId);

            _isDisposed = true;
        }
    }
}