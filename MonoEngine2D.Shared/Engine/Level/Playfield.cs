using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoEngine2D.Engine.Root;
using MonoEngine2D.Engine.Entities;
using MonoEngine2D.Engine.Entities.Geometry;
using MonoEngine2D.Engine.Utilities.User_Input;
using MonoEngine2D.Engine.Utilities.Cameras;

namespace MonoEngine2D.Engine.Level
{
    static class Playfield
    {
        public static List<Entity> Entities { get; private set; }
        public static List<Entity> EntityBuffer { get; private set; }
        public static List<Player> Players { get; private set; }
        public static List<Shape> BoundingBoxes { get; private set; }

        public static Vector2 CameraLocation { get; private set; }
        public static Random RNG { get; set; }
        public static bool Multiplayer { get; private set; }
        public static bool GameOver { get; set; }

        static Input input;

        public static void Initialize()
        {
            Entities = new List<Entity>();
            EntityBuffer = new List<Entity>();
            Players = new List<Player>();
            BoundingBoxes = new List<Shape>();

            RNG = new Random(DateTime.Now.Millisecond);

            Reset();
        }

        public static void Reset()
        {
            GameOver = false;

            Entities.Clear();
            ResetPlayers();

            input = new Input(PlayerIndex.One);

            HUD.Reset();
        }

        private static void ResetPlayers()
        {
            Players.Clear();

            Players.Add(new Player(0, 0, 12, 12, PlayerIndex.One));
            for (int i = 1; i < 4; i++)
            {
                if (GamePad.GetState(i).IsConnected)
                {
                    Multiplayer = true;
                    Players.Add(new Player(0, 0, 12, 12, (PlayerIndex)i));
                }
            }

            foreach (Player p in Players)
            {
                Entities.Add(p);
            }
        }

        public static void SetBoundingBoxes(List<Shape> boundingBoxes)
        {
            BoundingBoxes = boundingBoxes;
        }

        public static void AddEntity(Entity e)
        {
            EntityBuffer.Add(e);
        }

        private static void BackToMenu()
        {
            Reset();
            GameRoot.GameMode = GameRoot.Mode.Menu;
        }

        private static void CameraHandler(GameTime gameTime)
        {
            Camera.Update();
        }

        private static void UpdateInput(GameTime gameTime)
        {
            input.Update(gameTime);
            if (input.KeyState.IsKeyDown(Keys.R) && input.KeyReleased)
            {
                Reset();
                input.KeyReleased = false;
            }
        }

        private static void UpdateEntities(GameTime gameTime)
        {
            if (GameOver)
                return;

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

        public static void Update(GameTime gameTime)
        {
            CameraHandler(gameTime);
            UpdateInput(gameTime);
            UpdateEntities(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity e in Entities)
            {
                e.Draw(spriteBatch);
            }

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, Camera.Transform);
            {

            }
            spriteBatch.End();
        }
    }
}
