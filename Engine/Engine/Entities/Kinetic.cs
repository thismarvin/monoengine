
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine.Engine.Utilities;

namespace Engine.Engine.Entities
{
    abstract class Kinetic : Entity
    {
        public Vector2 Velocity { get; private set; }
        public List<Rectangle> CollisionRectangles { get; private set; }
        public float Gravity { get; set; }
        public float MoveSpeed { private get; set; }
        protected float Speed { get; private set; }
        public int CollisionWidth { get; private set; }

        public enum Direction
        { Left, Right, Up, Down, None }
        public Direction Facing { get; set; }

        public bool Falling { get; set; }

        public Kinetic(float x, float y, int width, int height, float moveSpeed) : base(x, y, width, height)
        {
            Velocity = Vector2.Zero;
            Gravity = 0.2f;
            MoveSpeed = moveSpeed;
            Facing = Direction.Right;
        }

        protected void SetVelocity(float x, float y)
        {
            Velocity = new Vector2(x, y);
        }

        protected void UpdateCollisionRectangles()
        {
            CollisionWidth = 2;
            CollisionRectangles = new List<Rectangle>()
            {
                // Rectangle on Top.
                new Rectangle((int)Location.X + CollisionWidth, (int)Location.Y - CollisionWidth, CollisionRectangle.Width - CollisionWidth * 2, CollisionWidth),
                // Rectangle on Bottom.
                new Rectangle((int)Location.X + CollisionWidth, (int)Location.Y + CollisionRectangle.Height, CollisionRectangle.Width - CollisionWidth * 2, CollisionWidth),
                // Rectangle on Left.
                new Rectangle((int)Location.X - CollisionWidth, (int)Location.Y + CollisionWidth, CollisionWidth, CollisionRectangle.Height - CollisionWidth * 2),
                // Rectangle on Right.
                new Rectangle((int)Location.X + CollisionRectangle.Width, (int)Location.Y + CollisionWidth, CollisionWidth, CollisionRectangle.Height - CollisionWidth * 2)
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

        protected void CalculateSpeed(GameTime gameTime)
        {
            Speed = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected abstract void ApplyForce(GameTime gameTime);
        protected abstract void Collision();
    }
}
