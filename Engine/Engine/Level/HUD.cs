
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine.Engine.Resources;
using Engine.Engine.Utilities;
using System.Collections.Generic;
using Engine.Engine.Entities;
using Engine.Engine.GameComponents;
using System;

namespace Engine.Engine.Level
{
    static class HUD
    {
        static Pinhole pinhole;

        static public void Initialize()
        {
            Reset();
        }

        public static void Reset()
        {
            pinhole = new Pinhole(Camera.RealScreenBounds.Width / 2, Camera.RealScreenBounds.Height / 2, Pinhole.Type.Open);
        }

        public static void Update(GameTime gameTime)
        {
            pinhole.Update(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, StaticCamera.Transform);
            {
                pinhole.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
