using Engine.Engine.Entities.Geometry;
using Engine.Engine.GameComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Engine.Utilities
{
    class Fade : MonoObject
    {
        Shape shape;
        Color fade;
        public bool Done { get; private set; }
        Type type;
        Timer timer;

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

            shape = new Shape(X, Y, Camera.RealScreenBounds.Width, Camera.RealScreenBounds.Height, fade);
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
