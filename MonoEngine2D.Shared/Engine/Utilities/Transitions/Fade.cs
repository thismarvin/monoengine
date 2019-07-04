using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine2D.Engine.Root;
using MonoEngine2D.Engine.Entities.Geometry;
using MonoEngine2D.Engine.Utilities.Time;
using MonoEngine2D.Engine.Utilities.Cameras;
using MonoEngine2D.Shared.Engine.Utilities.Transitions;

namespace MonoEngine2D.Engine.Utilities.Transitions
{
    class Fade : Transition
    {
        Shape shape;
        Color color;
        Color fade;
        byte alpha;
        
        public Fade(TransitionType type) : this(type, Color.Black, 100, 100)
        {

        }

        public Fade(TransitionType type, Color color) : this(type, color, 100, 100)
        {

        }

        public Fade(TransitionType type, Color color, float speed, float jerk) : base(-BUFFER, -BUFFER, type)
        {
            this.color = color;
            this.speed = speed;
            this.jerk = jerk;
            Reset();
        }

        public void Reset()
        {
            if (Type == TransitionType.Enter)
            {
                alpha = 255;
            }
            else if (Type == TransitionType.Exit)
            {
                alpha = 0;
            }

            fade = new Color(color, alpha);
            shape = new Shape(X, Y, Width, Height, fade);
        }

        public override void Update(GameTime gameTime)
        {
            if (!Started || Done)
                return;

            CalculateForce(gameTime);

            switch (Type)
            {
                case TransitionType.Exit:
                    if (alpha + velocity < 255)
                    {
                        alpha += (byte)(velocity);
                    }
                    else
                    {
                        alpha = 255;
                        Finished();
                    }
                    break;

                case TransitionType.Enter:
                    if (alpha - velocity > 0)
                    {
                        alpha -= (byte)(velocity);
                    }
                    else
                    {
                        alpha = 0;
                        Finished();
                    }
                    break;
            }
            Console.WriteLine(alpha);
            fade = new Color(color, alpha);
            shape.ObjectColor = fade;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (InProgress)
                shape.Draw(spriteBatch);
        }
    }
}
