using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine2D.Engine.Root;
using MonoEngine2D.Engine.Entities.Geometry;
using MonoEngine2D.Engine.Utilities.Cameras;
using MonoEngine2D.Shared.Engine.Utilities.Transitions;

namespace MonoEngine2D.Engine.Utilities.Transitions
{
    class Pinhole : Transition
    {
        Circle center;

        public Pinhole(TransitionType type) : this(type, 100, 500)
        {

        }

        public Pinhole(TransitionType type, float speed, float jerk) : base(type)
        {
            this.speed = speed;
            this.jerk = jerk;
            Reset();
        }

        public void Reset()
        {            
            switch (Type)
            {
                case TransitionType.Enter:
                    center = new Circle(X, Y, Width / 2, Width / 2, Color.Black);
                    break;
                case TransitionType.Exit:
                    center = new Circle(X, Y, Width / 2, 1, Color.Black);
                    break;
            }
        }

        public new void SetLocation(float x, float y)
        {
            base.SetLocation(x, y);
            center.SetLocation(X, Y);
        }

        public void SetSpeed(float speed)
        {
            velocity = velocity < 0 ? -speed : speed;
        }

        public override void Update(GameTime gameTime)
        {
            if (!InProgress)
                return;

            CalculateForce(gameTime);

            switch (Type)
            {
                case TransitionType.Enter:
                    center.SetLineWidth(center.LineWidth - velocity);
                    if (center.LineWidth <= 1)
                    {
                        center.SetLineWidth(1);
                        Finished();
                    }
                    break;
                case TransitionType.Exit:
                    center.SetLineWidth(center.LineWidth + velocity);
                    if (center.LineWidth >= Width / 2)
                    {
                        center.SetLineWidth(Width / 2);
                        Finished();
                    }
                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (InProgress)
                center.Draw(spriteBatch);
        }
    }
}
