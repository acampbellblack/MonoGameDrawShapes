using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonoGameDrawShapes
{
    public static class SpriteBatchExtensions
    {
        private static Texture2D Pixel;

        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            if (Pixel == null)
            {
                Pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                Pixel.SetData(new[] { Color.White });
            }

            spriteBatch.Draw(Pixel, rectangle, color);
        }
    }
}