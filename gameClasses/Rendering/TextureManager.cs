using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace Gardener.gameClasses.Rendering
{
    // Управляет загрузкой и освобождением текстур в игре
    public class TextureManager
    {
        private readonly Dictionary<string, int> textures = new Dictionary<string, int>(); // Хранит загруженные текстуры по пути файла

        /// <summary>
        /// Загружает текстуру из файла и возвращает её ID.
        /// </summary>
        /// <param name="path">Путь к файлу текстуры</param>
        /// <returns>ID текстуры в OpenGL</returns>
        public int LoadTexture(string path)
        {
            // Если текстура уже загружена, возвращаем её ID
            if (textures.ContainsKey(path))
                return textures[path];


            // Создаём новую текстуру
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);
            using (var img = new Bitmap(path))
            {
                var data = img.LockBits(
                    new Rectangle(0, 0, img.Width, img.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb
                );
                GL.TexImage2D(
                    TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                    img.Width, img.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                    PixelType.UnsignedByte,
                    data.Scan0
                );
                img.UnlockBits(data);
            }
            // Настраиваем параметры фильтрации текстуры
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            textures[path] = id; // Сохраняем ID в словаре
            return id;
        }

        // Освобождает все загруженные текстуры
        public void Dispose()
        {
            foreach (var textureId in textures.Values)
                GL.DeleteTexture(textureId);
            textures.Clear();
        }
    }
}