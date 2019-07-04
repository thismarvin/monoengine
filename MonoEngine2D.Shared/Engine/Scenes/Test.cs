using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine2D.Engine.Entities;
using MonoEngine2D.Engine.Entities.Geometry;
using MonoEngine2D.Engine.Utilities.Cameras;
using MonoEngine2D.Engine.Utilities.Transitions;
using MonoEngine2D.Shared.Engine.Utilities.Transitions;
using MonoEngine2D.Shared.Scenes;

namespace MonoEngine2D.Shared.Engine.Scenes
{
    class Test : Action
    {
        public Test() : base(SceneType.Test)
        {
            Entities.Add(new Circle(200, 100, 50, 5));
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
            //Entities.Add(new Circle(0, 0, 50));
        }

        protected override void InitializeTransitions()
        {
            ExitTransition = new Pinhole(TransitionType.Exit);
            EnterTransition = new Pinhole(TransitionType.Enter);
        }

        protected override void UpdateCamera(GameTime gameTime)
        {
            Camera.Update();
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
        }

        public override void Update(GameTime gameTime)
        {
            UpdateInput(gameTime);            
            UpdateEntities(gameTime);
            UpdateCamera(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, Camera.Transform);
            {
                foreach (Entity e in Entities)
                {
                    e.Draw(spriteBatch);
                }
            }
            spriteBatch.End();
        }
    }
}
