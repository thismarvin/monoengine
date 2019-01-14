using Engine.Engine.Entities;
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

        public enum Type
        {
            FADETOBLACK, FADEFROMBACK
        }

        public Fade(float x, float y, Type type) : base(x, y)
        {
            this.type = type;
            Reset();
        }

        public void Reset()
        {
            if (type == Type.FADEFROMBACK)
            {
                fade = new Color(0, 0, 0, 255);
            }
            else
            {
                fade = new Color(0, 0, 0, 0);
            }

            shape = new Shape(X, Y, Camera.ScreenBounds.Width, Camera.ScreenBounds.Height, fade);
        }

        private void FadeLogic()
        {
            if (!Done)
            {
                switch (type)
                {
                    case Type.FADETOBLACK:
                        if (fade.A <= 240)
                        {
                            fade.A += 10;
                        }
                        else
                        {
                            Done = true;
                        }
                        break;

                    case Type.FADEFROMBACK:
                        if (fade.A >= 0)
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
            }
        }

        public void Update(GameTime gameTime)
        {
            FadeLogic();
        }

        public void Draw (SpriteBatch spriteBatch)
        {
            shape.Draw(spriteBatch);
        }
    }
}
