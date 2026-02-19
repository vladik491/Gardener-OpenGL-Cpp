using OpenTK.Graphics.OpenGL;

namespace Gardener.gameClasses.Rendering
{
    // Рендерер для дерева, фруктов и их теней
    public class TreeRenderer
    {
        private readonly TextureRegistry _textureRegistry; // Реестр текстур

        // Конструктор: Инициализирует рендерер дерева
        public TreeRenderer(TextureRegistry textureRegistry)
        {
            _textureRegistry = textureRegistry;
        }

        // Рендерит дерево
        public void RenderTree(int grassStartX, int grassStartY)
        {
            GL.BindTexture(TextureTarget.Texture2D, _textureRegistry.TreeTextureId);
            float treePosXActual = (grassStartX + SpriteRenderer.DefaultTreePosX) * GameRenderer.TileSize;
            float treePosYActual = (grassStartY + SpriteRenderer.DefaultTreePosY) * GameRenderer.TileSize + SpriteRenderer.DefaultTreeOffsetY;
            float treeWidth = 32f * (GameRenderer.TileSize / 16f);
            float treeHeight = 39f * (GameRenderer.TileSize / 16f);
            float treeOffsetX = (GameRenderer.TileSize - treeWidth) / 2;
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0f, 0f); GL.Vertex2(treePosXActual + treeOffsetX, treePosYActual);
            GL.TexCoord2(1f, 0f); GL.Vertex2(treePosXActual + treeOffsetX + treeWidth, treePosYActual);
            GL.TexCoord2(1f, 1f); GL.Vertex2(treePosXActual + treeOffsetX + treeWidth, treePosYActual + treeHeight);
            GL.TexCoord2(0f, 1f); GL.Vertex2(treePosXActual + treeOffsetX, treePosYActual + treeHeight);
            GL.End();
        }

        // Рендерит фрукт
        public void RenderFruit(int textureId, int grassStartX, int grassStartY, int fruitWidthPixels, int fruitHeightPixels)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            float scaledFruitWidth = fruitWidthPixels * SpriteRenderer.FruitScale;
            float scaledFruitHeight = fruitHeightPixels * SpriteRenderer.FruitScale;
            float fruitPosXActual = (grassStartX + SpriteRenderer.DefaultFruitPosX) * GameRenderer.TileSize;
            float fruitPosYActual = (grassStartY + SpriteRenderer.DefaultFruitPosY) * GameRenderer.TileSize;
            float fruitOffsetX = (GameRenderer.TileSize - scaledFruitWidth) / 2f;
            float fruitOffsetY = (GameRenderer.TileSize - scaledFruitHeight) / 2f;
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0f, 0f); GL.Vertex2(fruitPosXActual + fruitOffsetX, fruitPosYActual + fruitOffsetY);
            GL.TexCoord2(1f, 0f); GL.Vertex2(fruitPosXActual + fruitOffsetX + scaledFruitWidth, fruitPosYActual + fruitOffsetY);
            GL.TexCoord2(1f, 1f); GL.Vertex2(fruitPosXActual + fruitOffsetX + scaledFruitWidth, fruitPosYActual + fruitOffsetY + scaledFruitHeight);
            GL.TexCoord2(0f, 1f); GL.Vertex2(fruitPosXActual + fruitOffsetX, fruitPosYActual + fruitOffsetY + scaledFruitHeight);
            GL.End();
        }

        // Рендерит тень фрукта
        public void RenderShadow(int grassStartX, int grassStartY)
        {
            GL.BindTexture(TextureTarget.Texture2D, _textureRegistry.FruitShadowTextureId);
            float shadowPosX = (grassStartX + SpriteRenderer.DefaultFruitShadowPosX) * GameRenderer.TileSize;
            float shadowPosY = (grassStartY + SpriteRenderer.DefaultFruitShadowPosY) * GameRenderer.TileSize;
            float shadowWidth = 30f * (GameRenderer.TileSize / 16f);
            float shadowHeight = 22f * (GameRenderer.TileSize / 16f);
            float shadowOffsetX = (GameRenderer.TileSize - shadowWidth) / 2;
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0f, 0f); GL.Vertex2(shadowPosX + shadowOffsetX, shadowPosY);
            GL.TexCoord2(1f, 0f); GL.Vertex2(shadowPosX + shadowOffsetX + shadowWidth, shadowPosY);
            GL.TexCoord2(1f, 1f); GL.Vertex2(shadowPosX + shadowOffsetX + shadowWidth, shadowPosY + shadowHeight);
            GL.TexCoord2(0f, 1f); GL.Vertex2(shadowPosX + shadowOffsetX, shadowPosY + shadowHeight);
            GL.End();
        }
    }
}