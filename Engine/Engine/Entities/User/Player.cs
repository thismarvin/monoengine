using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Engine.Engine.Entities.Geometry;
using Engine.Engine.GameComponents;
using Engine.Engine.Level;
using Engine.Engine.Resources;
using Engine.Engine.Utilities;

namespace Engine.Engine.Entities
{
    class Player : Kinetic
    {
        public PlayerIndex PlayerIndex { get; private set; }
        Input input;      

        public bool Dead { get; private set; }
        public int Health { get; private set; }

        public Player(float x, float y, int width, int height, PlayerIndex playerIndex) : base(x, y, width, height, 1)
        {
            PlayerIndex = playerIndex;
            input = new Input(playerIndex);
           
            Health = 6;
            MoveSpeed = 100;
        }

        public new void SetLocation(float x, float y)
        {
            base.SetLocation(x, y);
            // Update Sprite's Location to match the Player's Location
        }

        public new void SetCenter(float x, float y)
        {
            base.SetCenter(x, y);
            SetLocation(x - Width / 2, y - Height / 2);
        }

        protected override void ApplyForce(GameTime gameTime)
        {

        }

        protected override void Collision()
        {
            foreach (Shape s in Playfield.BoundingBoxes)
            {
                RectangleCollisionLogic(s);
            }

            foreach (Entity e in Playfield.Entities)
            {
                if (e != this)
                {
                    
                }
            }
        }

        private void RectangleCollisionLogic(Entity e)
        {
            //Bottom.
            if (CollisionRectangles[0].Intersects(e.CollisionRectangle) && Velocity.Y < 0)
            {
                SetLocation(X, e.CollisionRectangle.Bottom);
            }
            // Top.
            if (CollisionRectangles[1].Intersects(e.CollisionRectangle) && Velocity.Y > 0)
            {
                SetLocation(X, e.CollisionRectangle.Top - CollisionRectangle.Height);
            }
            // Right.
            if (CollisionRectangles[2].Intersects(e.CollisionRectangle) && Velocity.X < 0)
            {
                SetLocation(e.CollisionRectangle.Right, Y);
            }
            // Left.
            if (CollisionRectangles[3].Intersects(e.CollisionRectangle) && Velocity.X > 0)
            {
                SetLocation(e.CollisionRectangle.Left - CollisionRectangle.Width, Y);
            }
        }

        private void Move(Direction direction)
        {
            Facing = direction;

            switch (direction)
            {
                case Direction.Left:
                    SetLocation(X - Speed, Y);
                    SetVelocity(-1, Velocity.Y);
                    break;

                case Direction.Right:
                    SetLocation(X + Speed, Y);
                    SetVelocity(1, Velocity.Y);
                    break;

                case Direction.Up:
                    SetLocation(X, Y - Speed);
                    SetVelocity(Velocity.X, -1);
                    break;

                case Direction.Down:
                    SetLocation(X, Y + Speed);
                    SetVelocity(Velocity.X, 1);
                    break;
            }
        }        

        private void UpdateInput(GameTime gameTime)
        {
            input.Update(gameTime);
            Facing = Direction.None;           

            if (input.Pressing(Input.InputType.MovementLeft))
            {
                Move(Direction.Left);
            }

            if (input.Pressing(Input.InputType.MovementRight))
            {
                Move(Direction.Right);
            }

            if (input.Pressing(Input.InputType.MovementUp))
            {
                Move(Direction.Up);
            }

            if (input.Pressing(Input.InputType.MovementDown))
            {
                Move(Direction.Down);
            } 
        }       
        
        private void UpdateAnimation(GameTime gameTime)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            CalculateSpeed(gameTime);
            UpdateInput(gameTime);
            UpdateCollisionRectangles();
            Collision();
            UpdateAnimation(gameTime);
            LayerDepth = (int)Y + Height;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Game1.DebugMode)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, Camera.Transform);
                {
                    DrawCollisionRectangles(spriteBatch);
                }
                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, Camera.Transform);
                {

                }
                spriteBatch.End();                
            }
        }
    }
}
