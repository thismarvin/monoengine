using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Engine.Engine.Utilities;
using Engine.Engine.Entities;
using Engine.Engine.Resources;
using Engine.Engine.GameComponents;

namespace Engine.Engine.Level
{
    static class Playfield
    {
        public static List<Entity> Entities { get; private set; }
        public static List<Entity> EntityBuffer { get; private set; }
        public static List<Shape> BoundingBoxes { get; private set; }

        public static Vector2 CameraLocation { get; private set; }

        public static Random RNG { get; set; }

        public static bool GameOver { get; set; }

        public static List<Player> Players { get; private set; }
        public static bool Multiplayer { get; private set; }

        private static bool noSpam;

        public static void Initialize()
        {
            Entities = new List<Entity>();
            EntityBuffer = new List<Entity>();
            Players = new List<Player>();
            RNG = new Random(DateTime.Now.Millisecond);
            Reset();
        }

        public static void Reset()
        {
            GameOver = false;

            Entities.Clear();
            ResetPlayers();
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
            Game1.GameMode = Game1.Mode.MENU;
        }

        private static void CameraHandler(GameTime gameTime)
        {
            //Console.WriteLine(cameraPanX + " " + cameraPanY);
            //Console.WriteLine(panDirX + " " + panDirY);           

            Camera.Update();
        }

        private static void UpdateInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.R) && noSpam)
            {
                Reset();
                HUD.Reset();
                noSpam = false;
            }
            noSpam = Keyboard.GetState().IsKeyUp(Keys.R) ? true : noSpam;
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
            UpdateInput();
            UpdateEntities(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, Camera.Transform);
            {

            }
            spriteBatch.End();

            foreach (Entity e in Entities)
            {
                e.Draw(spriteBatch);
            }
        }
    }
}
