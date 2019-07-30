using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine2D.Engine.Utilities.Cameras;
using MonoEngine2D.Engine.Utilities.Misc;

namespace MonoEngine2D.Shared.Engine.Geometry
{
    class Quad : Shape
    {
        IndexBuffer indexBuffer;
        short[] indices;
        public Quad(float x, float y, int width, int height) : base(x, y, width, height)
        {
            CreateVertices();

            indices = new short[3 * 2]
            {
                0, 1, 2,
                0, 2, 3
            };           
        }

        protected override void CreateVertices()
        {
            vertices = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(-X, -Y, 0), Color.Red),
                new VertexPositionColor(new Vector3(-X + -Width, -Y, 0), Color.Green),
                new VertexPositionColor(new Vector3(-X + -Width, -Y + -Height, 0), Color.Green),
                new VertexPositionColor(new Vector3(-X, -Y + -Height, 0), Color.Blue),
            };
            ShapeManager.AddToVertexBuffer(vertices);
        }

        public override void RemoveFromVertexBuffer()
        {
            ShapeManager.RemoveFromVertexBuffer(vertices);
        }

        public new void SetLocation(float x, float y)
        {
            base.SetLocation(x, y);
            RemoveFromVertexBuffer();
            CreateVertices();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (effect == null)
            {
                effect = new BasicEffect(spriteBatch.GraphicsDevice);
                indexBuffer = new IndexBuffer(spriteBatch.GraphicsDevice, typeof(short), indices.Length, BufferUsage.WriteOnly);
                indexBuffer.SetData(indices);
            }
               
            effect.World = Camera.World;
            effect.View = Camera.View;
            effect.Projection = Camera.Projection;
            effect.VertexColorEnabled = true;

            spriteBatch.GraphicsDevice.Indices = indexBuffer;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                spriteBatch.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, 2);
            }
        }
    }
}
