using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine2D.Engine.Entities;
using MonoEngine2D.Engine.Entities.Geometry;
using MonoEngine2D.Engine.Utilities.Cameras;
using MonoEngine2D.Engine.Utilities.Transitions;
using MonoEngine2D.Engine.Utilities.User_Input;
using MonoEngine2D.Shared.Engine.Geometry;
using MonoEngine2D.Shared.Engine.Utilities.Transitions;
using MonoEngine2D.Shared.Scenes;

namespace MonoEngine2D.Shared.Engine.Scenes
{
    class Test : Action
    {
        List<Geometry.Shape> shapes;
        public Test() : base(SceneType.Test)
        {

        }

        public override void LoadScene()
        {
            Console.WriteLine("Scene LOADED!! TEST");
        }

        public override void UnloadScene()
        {
            Console.WriteLine("Scene UNLOADLOADED!! TEST");
        }

        protected override void Initialize()
        {
            Entities.Add(new MonoEngine2D.Engine.Entities.Geometry.Shape(0, 0, Camera.Bounds.Width, Camera.Bounds.Height, 2, Color.Red));
            shapes = new List<Geometry.Shape>()
            {
                new Triangle(16,16,50,50),
                new Quad(100,64,200,75),
            };
        }

        protected override void InitializeTransitions()
        {
            ExitTransition = new Pinhole(TransitionType.Exit);
            EnterTransition = new Pinhole(TransitionType.Enter);
        }

        protected override void UpdateCamera(GameTime gameTime)
        {
            cameraTopLeft = new Vector2(cameraTopLeft.X + 0.1f * (float)gameTime.TotalGameTime.TotalSeconds, 0);
            Camera.Update(cameraTopLeft, 0, Camera.Bounds.Width * 1.5f, 0, Camera.Bounds.Height * 1.5f);
        }

        protected override void UpdateEntities(GameTime gameTime)
        {
            for (int i = Entities.Count - 1; i >= 0; i--)
            {
                Entities[i].Update(gameTime);

                if (Entities[i].Remove)
                    Entities.RemoveAt(i);
            }

            if (EntityBuffer.Count > 0)
            {
                foreach (Entity e in EntityBuffer)
                    Entities.Add(e);

                EntityBuffer.Clear();
            }

            Entities.Sort();
        }

        protected override void UpdateInput(GameTime gameTime)
        {
            input.Update(gameTime);

            if (input.Pressing(Input.InputType.MovementUp) && input.KeyReleased)
            {
                shapes[0].Delete();
                shapes.Remove(shapes[0]);
                input.KeyReleased = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, Camera.Transform);
            {
                foreach (Entity e in Entities)
                {
                    e.Draw(spriteBatch);
                }

                foreach (Geometry.Shape s in shapes)
                {
                    s.Draw(spriteBatch);
                }
            }
            spriteBatch.End();
        }
    }
}
