﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine2D.Engine.Root;
using MonoEngine2D.Engine.Entities.Geometry;
using MonoEngine2D.Engine.Utilities.Time;
using MonoEngine2D.Engine.Utilities.Cameras;

namespace MonoEngine2D.Engine.Utilities.Transitions
{
    class Fade : MonoObject
    {
        Shape shape;
        Color fade;
        Timer timer;
        public bool Done { get; private set; }
        Type type;
        
        public enum Type
        {
            FadeToBlack, FadeFromBlack
        }

        public Fade(float x, float y, Type type) : base(x, y)
        {
            this.type = type;
            timer = new Timer(10);
            Reset();
        }

        public void Reset()
        {
            if (type == Type.FadeFromBlack)
            {
                fade = new Color(0, 0, 0, 255);
            }
            else
            {
                fade = new Color(0, 0, 0, 0);
            }

            shape = new Shape(X - 32, Y - 32, Camera.ScreenBounds.Width + 64, Camera.ScreenBounds.Height + 64, fade);
        }

        private void FadeLogic(GameTime gameTime)
        {
            timer.Update(gameTime);
            if (!Done && timer.Done)
            {
                switch (type)
                {
                    case Type.FadeToBlack:
                        if (fade.A <= 240)
                        {
                            fade.A += 10;
                        }
                        else
                        {
                            Done = true;
                        }
                        break;

                    case Type.FadeFromBlack:
                        if (fade.A > 0)
                        {
                            fade *= 0.95f;
                        }
                        else
                        {
                            Done = true;
                        }
                        break;
                }
                shape.ObjectColor = fade;
                timer.Reset();
            }
        }

        public void Update(GameTime gameTime)
        {
            FadeLogic(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            shape.Draw(spriteBatch);
        }
    }
}
