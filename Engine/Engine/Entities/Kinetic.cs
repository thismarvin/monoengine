
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine.Engine.Utilities;

namespace Engine.Engine.Entities
{
    abstract class Kinetic : Entity
    {
        public Vector2 Velocity { get; set; }
        public List<Rectangle> CollisionRectangles { get; private set; }
        public float Gravity { get; set; }
        public float MoveSpeed { get; set; }
        public int CollisionWidth { get; private set; }

        public enum Direction
        { LEFT, RIGHT, UP, NONE }
        public Direction Facing { get; set; }

        public bool Falling { get; set; }

        public Kinetic(float x, float y, int width, int height, float moveSpeed, Entities type) : base(x, y, width, height, type)
        {
            Velocity = Vector2.Zero;
            Gravity = 0.2f;
            MoveSpeed = moveSpeed;
            Facing = Direction.RIGHT;
        }

        protected void ApplyForce()
        {
            SetLocation(X + Velocity.X, Y + Velocity.Y);
            if (Velocity.Y < 3)
            {
                Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity);
            }

            Falling = Velocity.Y > Gravity ? true : false;
        }

        protected void UpdateCollisionRectangles()
        {
            CollisionWidth = (int)MoveSpeed * 2;
            CollisionRectangles = new List<Rectangle>()
            {
                // Rectangle on Top.
                new Rectangle((int)Location.X + CollisionWidth, (int)Location.Y - CollisionWidth, CollisionRectangle.Width - CollisionWidth * 2, CollisionWidth),
                // Rectangle on Bottom.
                new Rectangle((int)Location.X, (int)Location.Y + CollisionRectangle.Height, CollisionRectangle.Width, CollisionWidth + (int)Velocity.Y),
                // Rectangle on Left.
                new Rectangle((int)Location.X - CollisionWidth, (int)Location.Y /*+ CollisionWidth*/, CollisionWidth, CollisionRectangle.Height /*- CollisionWidth * 2*/),
                // Rectangle on Right.
                new Rectangle((int)Location.X + CollisionRectangle.Width, (int)Location.Y /*+ CollisionWidth*/, CollisionWidth, CollisionRectangle.Height /*- CollisionWidth * 2*/)
            };
        }

        protected void DrawCollisionRectangles(SpriteBatch spriteBatch)
        {
            if (CollisionRectangles == null)
                return;
            spriteBatch.Draw(ShapeManager.Texture, CollisionRectangle, ObjectColor);
            foreach (Rectangle r in CollisionRectangles)
            {
                spriteBatch.Draw(ShapeManager.Texture, r, Color.Red);
            }
        }

        protected abstract void Collision();
    }
}
