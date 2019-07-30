using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine2D.Engine.Root;
using MonoEngine2D.Engine.Utilities.Cameras;
using MonoEngine2D.Engine.Utilities.Misc;

namespace MonoEngine2D.Shared.Engine.Geometry
{
    class Triangle : Shape
    {
        public Triangle(float x, float y, int width, int height) : base(x, y, width, height)
        {
            CreateVertices();
        }

        protected override void CreateVertices()
        {
            vertices = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(-X, -Y, 0), Color.Red),
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
                effect = new BasicEffect(spriteBatch.GraphicsDevice);

            effect.World = Camera.World;
            effect.View = Camera.View;
            effect.Projection = Camera.Projection;
            effect.VertexColorEnabled = true;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                spriteBatch.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 1);
            }
        }
    }
}
