using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine2D.Engine.Utilities.Misc
{
    class ShapeManager
    {
        public static Texture2D Texture { get; private set; }

        static VertexBuffer vertexBuffer;
        private static List<VertexPositionColor> vertices;

        private ShapeManager()
        {

        }

        public static void Initialize(GraphicsDeviceManager graphics)
        {
            Texture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            Texture.SetData(new[] { Color.White });

            vertices = new List<VertexPositionColor>();
            vertexBuffer = new VertexBuffer(graphics.GraphicsDevice, typeof(VertexPositionColor), 1, BufferUsage.WriteOnly);
        }

        public static void AddToVertexBuffer(VertexPositionColor[] vertices)
        {
            foreach (VertexPositionColor vertexPositionColor in vertices)
            {
                if (!ShapeManager.vertices.Contains(vertexPositionColor))
                {
                    ShapeManager.vertices.Add(vertexPositionColor);
                }                   
            }
        }

        public static void RemoveFromVertexBuffer(VertexPositionColor[] vertices)
        {
            for (int i = vertices.Length - 1; i >= 0; i--)
            {
                if (ShapeManager.vertices.Contains(vertices[i]))
                {
                    ShapeManager.vertices.RemoveAt(i);
                }
            }
        }

        public static void UnloadContent()
        {
            Texture.Dispose();
        }

        public static void Update(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.SetVertexBuffer(vertexBuffer);
        }
    }
}
