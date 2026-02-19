using OpenTK.Graphics.OpenGL;

namespace Gardener.gameClasses.Rendering
{
    // Рендерит панель инвентаря игрока на экране
    public class InventoryUIRenderer
    {
        // Блок: Зависимости
        private readonly TextureRegistry _textureRegistry; // Реестр текстур для доступа к изображениям

        // Блок: Константы размеров
        private const float ItemIconWidth = 64f; 
        private const float ItemIconHeight = 64f; 
        private const float NumberIconSize = 32f; 
        private const float Margin = 10f;

        // Конструктор: инициализирует рендерер с реестром текстур
        public InventoryUIRenderer(TextureRegistry textureRegistry)
        {
            _textureRegistry = textureRegistry;
        }

        /// <summary>
        /// Отрисовывает панель инвентаря с предметами и их количеством.
        /// </summary>
        /// <param name="width">Ширина экрана</param>
        /// <param name="height">Высота экрана</param>
        /// <param name="inventoryManager">Менеджер инвентаря</param>
        /// <param name="numberAnimationScales">Масштабы анимации чисел</param>
        /// <param name="playerId">ID игрока</param>
        /// <param name="viewportOffsetX">Смещение вьюпорта по X</param>
        public void RenderInventoryPanel(int width, int height, InventoryManager inventoryManager, float[] numberAnimationScales, int playerId, int viewportOffsetX)
        {
            // Переключаемся в 2D-режим для рендеринга UI
            GL.MatrixMode(MatrixMode.Projection); GL.PushMatrix(); GL.LoadIdentity(); GL.Ortho(0, width, height, 0, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview); GL.PushMatrix(); GL.LoadIdentity();
            GL.Enable(EnableCap.Texture2D); GL.Enable(EnableCap.Blend);

            var items = inventoryManager.GetItems();
            float numberBaseX = width - ItemIconWidth - Margin - NumberIconSize - 5; // Базовая позиция X для чисел (справа налево)
            float startY = Margin; // Начальная позиция Y

            // Отрисовка каждого предмета и его количества
            for (int i = 0; i < items.Count; i++)
            {
                if (i >= 8) break;
                var item = items[i];
                float itemPosY = startY + i * (ItemIconHeight + Margin);
                float texLeft = item.IsAvailable ? 0f : 0.5f; // Текстурные координаты в зависимости от доступности
                float texRight = item.IsAvailable ? 0.5f : 1.0f;

                // Рендеринг иконки предмета
                float itemPosX = numberBaseX + NumberIconSize + 5;
                GL.BindTexture(TextureTarget.Texture2D, item.TextureId);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(texLeft, 0f); GL.Vertex2(itemPosX, itemPosY);
                GL.TexCoord2(texRight, 0f); GL.Vertex2(itemPosX + ItemIconWidth, itemPosY);
                GL.TexCoord2(texRight, 1f); GL.Vertex2(itemPosX + ItemIconWidth, itemPosY + ItemIconHeight);
                GL.TexCoord2(texLeft, 1f); GL.Vertex2(itemPosX, itemPosY + ItemIconHeight);
                GL.End();

                // Рендеринг числа с анимацией масштаба
                float numberPosX = numberBaseX;
                float numberPosY = itemPosY + (ItemIconHeight - NumberIconSize) / 2;
                float scale = numberAnimationScales[i];
                GL.BindTexture(TextureTarget.Texture2D, _textureRegistry.NumberTextureIds[i]);
                GL.PushMatrix();
                GL.Translate(numberPosX + NumberIconSize / 2f, numberPosY + NumberIconSize / 2f, 0);
                GL.Scale(scale, scale, 1f);
                GL.Translate(-(numberPosX + NumberIconSize / 2f), -(numberPosY + NumberIconSize / 2f), 0);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0f, 0f); GL.Vertex2(numberPosX, numberPosY);
                GL.TexCoord2(1f, 0f); GL.Vertex2(numberPosX + NumberIconSize, numberPosY);
                GL.TexCoord2(1f, 1f); GL.Vertex2(numberPosX + NumberIconSize, numberPosY + NumberIconSize);
                GL.TexCoord2(0f, 1f); GL.Vertex2(numberPosX, numberPosY + NumberIconSize);
                GL.End();
                GL.PopMatrix();
            }

            // Восстановление матриц после рендеринга
            GL.MatrixMode(MatrixMode.Projection); GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview); GL.PopMatrix();
        }
    }
}