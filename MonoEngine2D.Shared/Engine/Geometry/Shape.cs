using Microsoft.Xna.Framework.Graphics;
using MonoEngine2D.Engine.Root;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoEngine2D.Shared.Engine.Geometry
{
    abstract class Shape : MonoObject
    {
        protected VertexPositionColor[] vertices;
        protected BasicEffect effect;

        public Shape(float x, float y, int width, int height) : base(x, y, width, height)
        {

        }

        protected abstract void CreateVertices();
        public abstract void RemoveFromVertexBuffer();
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
