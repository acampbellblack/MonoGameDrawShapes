using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace MonoGameDrawShapes
{
    public static class GraphicsDeviceExtensions
    {
        private static bool Initialized;

        private static BasicEffect BasicEffect;

        private static VertexBuffer TriangleBuffer;
        private static VertexPositionColor[] TriangleVertices = new VertexPositionColor[3];

        private const int CircleNumPoints = 36;
        private static IndexBuffer CircleIndexBuffer;
        private static VertexBuffer CircleVertexBuffer;
        private static VertexPositionColor[] CircleVertices = new VertexPositionColor[CircleNumPoints + 1];

        private static void InitializeDrawShapes(this GraphicsDevice graphicsDevice)
        {
            var world = Matrix.CreateOrthographicOffCenter(new Rectangle(0, 0, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight), 1.0f, -1.0f);

            BasicEffect = new BasicEffect(graphicsDevice);
            BasicEffect.World = world;
            BasicEffect.VertexColorEnabled = true;

            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            graphicsDevice.RasterizerState = rasterizerState;

            TriangleBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);

            int numIndices = 3 * (CircleNumPoints - 1);
            int[] indices = new int[numIndices];
            for (int i = 1; i < CircleNumPoints; i++)
            {
                int baseIndex = (i - 1) * 3;
                indices[baseIndex] = 0;
                indices[baseIndex + 1] = i;
                indices[baseIndex + 2] = i == CircleNumPoints - 1 ? 1 : i + 1;
            }
            CircleIndexBuffer = new IndexBuffer(graphicsDevice, IndexElementSize.ThirtyTwoBits, indices.Length, BufferUsage.WriteOnly);
            CircleIndexBuffer.SetData(indices);
            CircleVertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), CircleNumPoints + 1, BufferUsage.WriteOnly);
        }

        public static void DrawTriangle(this GraphicsDevice graphicsDevice, Vector2 v1, Vector2 v2, Vector2 v3, Color color)
        {
            if (!Initialized)
            {
                graphicsDevice.InitializeDrawShapes();
                Initialized = true;
            }

            TriangleVertices[0] = new VertexPositionColor(new Vector3(v1.X, v1.Y, 0.0f), color);
            TriangleVertices[1] = new VertexPositionColor(new Vector3(v2.X, v2.Y, 0.0f), color);
            TriangleVertices[2] = new VertexPositionColor(new Vector3(v3.X, v3.Y, 0.0f), color);
            
            TriangleBuffer.SetData(TriangleVertices);
            graphicsDevice.SetVertexBuffer(TriangleBuffer);

            foreach (var pass in BasicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
            }
        }

        public static void DrawCircle(this GraphicsDevice graphicsDevice, Vector2 center, float radius, Color color)
        {
            if (!Initialized)
            {
                graphicsDevice.InitializeDrawShapes();
                Initialized = true;
            }

            CircleVertices[0] = new VertexPositionColor(new Vector3(center.X, center.Y, 0.0f), color);

            float angleStep = MathHelper.TwoPi / CircleNumPoints;

            for (int i = 1; i < CircleNumPoints + 1; i++)
            {
                float angle = angleStep * i;

                float x = center.X + radius * (float)Math.Cos(angle);
                float y = center.Y + radius * (float)Math.Sin(angle);

                CircleVertices[i] = new VertexPositionColor(new Vector3(x, y, 0.0f), color);
            }

            CircleVertexBuffer.SetData(CircleVertices);
            graphicsDevice.SetVertexBuffer(CircleVertexBuffer);
            graphicsDevice.Indices = CircleIndexBuffer;

            foreach (var pass in BasicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, CircleNumPoints + 1);
            }
        }
    }
}
