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
    class Pinhole : MonoObject
    {
        Circle center;
        float increment;
        bool done;
        Type type;
        public enum Type
        {
            OPEN, CLOSED
        }

        public Pinhole(float x, float y, Type type) : base (x,y)
        {
            this.type = type;

            switch (type)
            {
                case Type.CLOSED:
                    center = new Circle(X, Y, -400, 400, Color.Black);
                    increment = -3;
                    break;
                case Type.OPEN:
                    center = new Circle(X, Y, 400, 400, Color.Black);
                    increment = 3;
                    break;
            }
        }

        public new void SetLocation(float x,float y)
        {
            base.SetLocation(x, y);
            switch (type)
            {
                case Type.CLOSED:
                    center = new Circle(X, Y, -400, 400, Color.Black);
                    increment = -3;
                    break;
                case Type.OPEN:
                    center = new Circle(X, Y, 400, 400, Color.Black);
                    increment = 3;
                    break;
            }
        }

        public void SetSpeed(float speed)
        {
            increment = increment < 0 ? -speed : speed;
        }

        public void Update(GameTime gameTime)
        {
            if (done)
                return;

            switch (type)
            {
                case Type.OPEN:
                    if (center.Radius < Camera.ScreenBounds.Width * 2 && !done)
                    {
                        center.SetRadius(center.Radius + (int)increment);
                        increment += 0.15f;
                    }
                    else
                    {
                        center.SetRadius(0);
                        done = true;
                    }

                    break;
                case Type.CLOSED:
                    if (center.Radius > -Camera.ScreenBounds.Width * 2 && !done)
                    {
                        center.SetRadius(center.Radius + (int)increment);
                        increment -= 0.15f;
                    }
                    else
                    {
                        center.SetRadius(0);
                        done = true;
                    }
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            center.Draw(spriteBatch);
        }
    }
}
