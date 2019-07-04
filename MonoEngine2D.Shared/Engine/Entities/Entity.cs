using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine2D.Engine.Root;
using MonoEngine2D.Engine.Utilities.Cameras;

namespace MonoEngine2D.Engine.Entities
{
    abstract class Entity : MonoObject
    {
        public Entity(float x, float y, int width, int height) : base(x, y, width, height)
        {

        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
