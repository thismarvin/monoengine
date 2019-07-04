using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine2D.Engine.Utilities.Cameras;
using MonoEngine2D.Engine.Utilities.Transitions;
using MonoEngine2D.Shared.Engine.Utilities.Transitions;

namespace MonoEngine2D.Engine.Level
{
    static class HUD
    {
        static Transition transition;

        static public void Initialize()
        {
            Reset();
        }

        public static void Reset()
        {
            transition = new Fade(TransitionType.Exit);
            transition.Start();
        }

        public static void Update(GameTime gameTime)
        {
            transition.Update(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, StaticCamera.Transform);
            {
                transition.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
