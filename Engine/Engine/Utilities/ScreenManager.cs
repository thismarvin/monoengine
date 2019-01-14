 
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Engine.Engine.GameComponents;

namespace Engine.Engine.Utilities
{
    static class ScreenManager
    {
        public static int DisplayWidth { get; private set; }
        public static int DisplayHeight { get; private set; }
        public static int DefaultWindowHeight { get; private set; }
        public static int DefaultWindowWidth { get; private set; }
        public static float FPS { get; private set; }
        static Keys mappedKey;
        static bool released;
        static bool fullscreen;

        public static bool WideScreenSupport { get; private set; }

        public static void Initialize(int defaultWindowWidth, int defaultWindowHeight)
        {
            DisplayWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            DisplayHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            DefaultWindowWidth = defaultWindowWidth;
            DefaultWindowHeight = defaultWindowHeight;

            mappedKey = Keys.F10;
            WideScreenSupport = true;

            //FindAspectRatio();
        }

        private static void FindAspectRatio()
        {
            List<int> widthFactors = new List<int>();
            List<int> heightFactors = new List<int>();

            FillListWithFactors(widthFactors, DisplayWidth);
            FillListWithFactors(heightFactors, DisplayHeight);

            int GCF = 1;
            for (int i = widthFactors.Count - 1; i >= 0; i--)
            {
                for (int j = heightFactors.Count - 1; j >= 0; j--)
                {
                    if (widthFactors[i] == heightFactors[j] && widthFactors[i] > GCF)
                    {
                        GCF = widthFactors[i];
                        break;
                    }
                }
            }
            Console.WriteLine(DisplayWidth / GCF + ":" + DisplayHeight / GCF);
        }

        private static void FillListWithFactors(List<int> list, int number)
        {
            for (int i = 1; i < (int)Math.Sqrt(number) + 1; i++)
            {
                if (number % i == 0)
                {
                    list.Add(i);
                    list.Add(number / i);
                }
            }
            list.Sort();
        }

        public static void StartFullScreen(GraphicsDeviceManager graphics)
        {
            ActivateFullScreenMode(graphics);
            fullscreen = true;
            released = false;
        }

        public static void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            UpdateFPS(gameTime);
            KeyboardInput(graphics);
        }

        private static void UpdateFPS(GameTime gameTime)
        {
            FPS = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private static void KeyboardInput(GraphicsDeviceManager graphics)
        {
            if (Keyboard.GetState().IsKeyUp(mappedKey))
            {
                released = true;
            }

            if (Keyboard.GetState().IsKeyDown(mappedKey) && released)
            {
                if (fullscreen) { ResetScreenToDefaultDimensions(graphics); }
                else { ActivateFullScreenMode(graphics); }
                fullscreen = !fullscreen;
                released = false;
            }
        }

        private static void ActivateFullScreenMode(GraphicsDeviceManager graphics)
        {
            Camera.Reset(DisplayWidth, DisplayHeight);
            StaticCamera.Reset(DisplayWidth, DisplayHeight);
            graphics.PreferredBackBufferHeight = DisplayHeight;
            graphics.PreferredBackBufferWidth = DisplayWidth;
            graphics.ToggleFullScreen();
            graphics.ApplyChanges();
        }

        private static void ResetScreenToDefaultDimensions(GraphicsDeviceManager graphics)
        {
            Camera.Reset(DefaultWindowWidth, DefaultWindowHeight);
            StaticCamera.Reset(DefaultWindowWidth, DefaultWindowHeight);
            graphics.PreferredBackBufferHeight = DefaultWindowHeight;
            graphics.PreferredBackBufferWidth = DefaultWindowWidth;
            graphics.ToggleFullScreen();
            graphics.ApplyChanges();
        }
    }
}
