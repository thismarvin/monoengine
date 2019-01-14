using Engine.Engine.GameComponents;
using Engine.Engine.Level;
using Engine.Engine.Resources;
using Engine.Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Engine.Entities
{
    class Player : Kinetic
    {
        Input input;

        public Player(float x, float y, int width, int height, float moveSpeed, PlayerIndex playerIndex) : base(x, y, width, height, moveSpeed, Entities.PLAYER)
        {
            input = new Input(playerIndex);
        }

        private new void ApplyForce()
        {
            base.ApplyForce();
        }

        protected override void Collision()
        {

        }

        private void UpdateInput(GameTime gameTime)
        {
            input.Update(gameTime);

            if (input.Pressing(Input.InputType.LEFT))
            {

            }

            if (input.Pressing(Input.InputType.RIGHT))
            {

            }

            if ((input.Pressing(Input.InputType.JUMP) || input.IsKeyDown(Input.InputType.UP)))
            {

            }

            if (input.Pressing(Input.InputType.ATTACK))
            {
        
            }

            if (input.Pressing(Input.InputType.DOWN))
            {
           
            }          
        }
        public override void Update(GameTime gameTime)
        {
            UpdateInput(gameTime);
            ApplyForce();
            UpdateCollisionRectangles();
            Collision();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Game1.DebugMode)
            {
                DrawCollisionRectangles(spriteBatch);
            }
            else
            {

            }
        }
    }
}
