using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawShapes
{
    public static class SpriteBatchExtensions
    {
        // DrawRectangle: Draw a scaled white 1x1 pixel in any color (requires SpriteBatch.Begin())

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

        // DrawTriangle: Creates VertexPositionColor array from Vectors and color and draws (does not require SpriteBatch.Begin())
        
        private static BasicEffect BasicEffect;
        private static VertexBuffer VertexBuffer;
        private static Matrix World;
        private static RasterizerState RasterizerState;

        public static void DrawTriangle(this SpriteBatch spriteBatch, Vector2 v1, Vector2 v2, Vector2 v3, Color color)
        {
            if (BasicEffect == null)
            {
                BasicEffect = new BasicEffect(spriteBatch.GraphicsDevice);
                VertexBuffer = new VertexBuffer(spriteBatch.GraphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
                World = Matrix.CreateOrthographicOffCenter(new Rectangle(0, 0, spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth, spriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight), 1.0f, -1.0f);
                RasterizerState = new RasterizerState();
                RasterizerState.CullMode = CullMode.None;
            }

            VertexPositionColor[] vertices = new VertexPositionColor[3];
            vertices[0] = new VertexPositionColor(new Vector3(v1.X, v1.Y, 0.0f), color);
            vertices[1] = new VertexPositionColor(new Vector3(v2.X, v2.Y, 0.0f), color);
            vertices[2] = new VertexPositionColor(new Vector3(v3.X, v3.Y, 0.0f), color);

            VertexBuffer.SetData(vertices);

            BasicEffect.World = World;
            BasicEffect.VertexColorEnabled = true;

            spriteBatch.GraphicsDevice.SetVertexBuffer(VertexBuffer);

            spriteBatch.GraphicsDevice.RasterizerState = RasterizerState;

            foreach (var pass in BasicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                spriteBatch.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
            }
        }
    }
}