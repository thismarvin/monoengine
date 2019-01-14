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
        public static bool GameOver { get; set; }

        static Random RNG;
        static int levelWidth;
        static int levelHeight;

        public static Player Player { get; private set; }

        public static void Initialize()
        {
            Entities = new List<Entity>();
            EntityBuffer = new List<Entity>();
            RNG = new Random(DateTime.Now.Millisecond);
            Reset();
        }

        public static void Reset()
        {
            GameOver = false;
            CameraLocation = new Vector2(0, 0);
            levelWidth = Camera.ScreenBounds.Width;
            levelHeight = Camera.ScreenBounds.Height;
            Entities.Clear();
        }

        private static void BackToMenu()
        {
            Reset();
            Game1.GameMode = Game1.Mode.MENU;
        }

        private static void CameraHandler()
        {
            Camera.Update(CameraLocation, 0, levelWidth, 0, levelHeight);
        }

        public static void UpdateInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                Reset();
            }
        }

        private static void UpdateEntities(GameTime gameTime)
        {
            if (GameOver)
                return;

            for (int i = Entities.Count - 1; i >= 0; i--)
            {
                // Makes it so only entites that are within twice the camera's bounds are updated.
                if (Entities[i].X < CameraLocation.X - Camera.ScreenBounds.Width || Entities[i].X > CameraLocation.X + Camera.ScreenBounds.Width * 2)
                    continue;
                Entities[i].Update(gameTime);

                if (Entities[i].Remove)
                    Entities.RemoveAt(i);
            }

            foreach (Entity e in EntityBuffer)
                Entities.Add(e);

            EntityBuffer.Clear();
        }

        // Permanently deletes entites that are offscreen. Useful for a game that has the character always moving right.
        private static void ClearOffScreen()
        {
            for (int i = Entities.Count - 1; i >= 0; i--)
            {
                if (Entities[i].X < CameraLocation.X - Camera.ScreenBounds.Width - 64)
                {
                    Entities.RemoveAt(i);
                }
            }
        }

        public static void Update(GameTime gameTime)
        {
            CameraHandler();
            UpdateInput();
            //ClearOffScreen();
            UpdateEntities(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity e in Entities)
            {
                // Makes it so only entites that are within twice the camera's bounds are drawn.
                if (e.X < CameraLocation.X - Camera.ScreenBounds.Width || e.X > CameraLocation.X + Camera.ScreenBounds.Width * 2)
                    continue;
                e.Draw(spriteBatch);
            }
        }
    }
}
