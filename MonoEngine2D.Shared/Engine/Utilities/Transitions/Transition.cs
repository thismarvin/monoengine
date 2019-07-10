using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine2D.Engine.Root;
using MonoEngine2D.Engine.Utilities.Cameras;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoEngine2D.Shared.Engine.Utilities.Transitions
{
    public enum TransitionType
    {
        Enter, Exit
    }

    abstract class Transition : MonoObject
    {
        protected const int BUFFER = 64;
        public bool Started { get; private set; }
        public bool Done { get; private set; }
        public bool InProgress { get { return Started && !Done; } }
        protected TransitionType Type { get; private set; }

        protected float velocity;
        protected float speed;
        protected float jerk;
        protected float acceleration;

        public Transition(TransitionType type) : this(Camera.Bounds.Width / 2, Camera.Bounds.Height / 2, type)
        {

        }

        public Transition(float x, float y, TransitionType type) : base(x, y, 0, 0)
        {
            Type = type;
            SetCollisionRectangle(X, Y, GreaterDimension(), GreaterDimension());
        }

        private int GreaterDimension()
        {
            return Camera.Bounds.Width > Camera.Bounds.Height ? Camera.Bounds.Width + BUFFER * 2 : Camera.Bounds.Height + BUFFER * 2;
        }

        public void Start()
        {
            Started = true;
        }

        protected void Finished()
        {
            Done = true;
        }

        protected void CalculateForce(GameTime gameTime)
        {
            velocity = (acceleration + speed) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            acceleration += jerk * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
