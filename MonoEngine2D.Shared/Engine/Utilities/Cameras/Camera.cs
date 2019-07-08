using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoEngine2D.Engine.Root;
using MonoEngine2D.Engine.Utilities.Display;

namespace MonoEngine2D.Engine.Utilities.Cameras
{
    static class Camera
    {
        public static Matrix Transform { get; private set; }
        public static Matrix World { get; private set; }
        public static Matrix View { get; private set; }
        public static Matrix Projection { get; private set; }

        public static Rectangle Bounds { get; private set; }

        public static Vector2 TopLeft;
        public static int Scale { get; private set; }
        public static float Zoom { get; private set; }

        static int pixelWidth;
        static int pixelHeight;

        public static void Initialize(int pixelWidth, int pixelHeight, int scale)
        {
            CreatePixelScene(pixelWidth, pixelHeight, scale);
            Reset(ScreenManager.DefaultWindowWidth, ScreenManager.DefaultWindowHeight);
            SetupMatrices();
            UpdateMatrices();
        }

        private static void CreatePixelScene(int pixelWidth, int pixelHeight, int scale)
        {
            Scale = scale;
            Camera.pixelWidth = pixelWidth * Scale;
            Camera.pixelHeight = pixelHeight * Scale;
        }

        private static void SetupMatrices()
        {
            World = Matrix.Identity * Matrix.CreateTranslation(new Vector3(ScreenManager.CurrentWindowWidth / Zoom / 2 - StaticCamera.HorizontalLetterBox + TopLeft.X, ScreenManager.CurrentWindowHeight / Zoom / 2 - StaticCamera.VerticalLetterBox + TopLeft.Y, 0));
            View = Matrix.CreateLookAt(new Vector3(0, 0, -2), Vector3.Forward, Vector3.Up);
            Projection = Matrix.CreateOrthographic(ScreenManager.CurrentWindowWidth / Zoom, ScreenManager.CurrentWindowHeight / Zoom, -1000, 1000);
        }

        public static void Reset(int windowWidth, int windowHeight)
        {
            int longSide = pixelWidth > pixelHeight ? pixelWidth : pixelHeight;
            int shortSide = pixelWidth < pixelHeight ? pixelWidth : pixelHeight;
            int longDisplayDimension = windowWidth > windowHeight ? windowWidth : windowHeight;
            int shortDisplayDimension = windowWidth < windowHeight ? windowWidth : windowHeight;

            switch (GameRoot.GameOrientation)
            {
                case GameRoot.Orientation.Landscape:
                    Zoom = (float)shortDisplayDimension / shortSide;
                    // Check if letterboxing is required.
                    if (longSide * Zoom > longDisplayDimension)
                    {
                        Zoom = (float)longDisplayDimension / longSide;
                    }
                    else if (longSide * Zoom < longDisplayDimension)
                    {
                        // Disable letterboxing if WideScreenSupport is enabled.
                        if (ScreenManager.WideScreenSupport)
                        {
                            longSide = (int)((longDisplayDimension - longSide * Zoom) / Zoom) + longSide;
                        }
                    }
                    Bounds = new Rectangle(0, 0, longSide / Scale, shortSide / Scale);
                    break;

                case GameRoot.Orientation.Portrait:
                    Zoom = (float)shortDisplayDimension / shortSide;
                    // Check if letterboxing is required. ??? Im not sure if i really need this.
                    if (longSide * Zoom > longDisplayDimension)
                    {
                        Zoom = (float)longDisplayDimension / longSide;
                    }
                    Bounds = new Rectangle(0, 0, shortSide / Scale, longSide / Scale);
                    break;
            }
        }

        public static void Update()
        {
            Input();
            UpdateMatrices();
        }

        public static void Update(Vector2 topLeft, float minWidth, float maxWidth, float minHeight, float maxHeight)
        {
            TopLeft = topLeft * Scale;

            Input();
            StayWithinBounds(minWidth, maxWidth, minHeight, maxHeight);
            UpdateMatrices();
        }

        private static void Input()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
            {
                Zoom += 0.01f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            {
                Zoom -= 0.01f;
            }
        }

        private static void StayWithinBounds(float minWidth, float maxWidth, float minHeight, float maxHeight)
        {
            // Still Buggy!
            TopLeft.X = (TopLeft.X < minWidth ? minWidth : TopLeft.X);
            TopLeft.X = (TopLeft.X + Bounds.Width > maxWidth ? maxWidth - Bounds.Width: TopLeft.X);
            TopLeft.Y = (TopLeft.Y < minHeight ? 0 : TopLeft.Y);
            TopLeft.Y = (TopLeft.Y + Bounds.Height > maxHeight ? maxHeight - Bounds.Height : TopLeft.Y);
        }

        private static void UpdateMatrices()
        {
            Transform = Matrix.CreateTranslation(new Vector3(-TopLeft.X + StaticCamera.HorizontalLetterBox, -TopLeft.Y + StaticCamera.VerticalLetterBox, 0)) * Matrix.CreateScale(new Vector3(Zoom, Zoom, 0));
            World = Matrix.Identity * Matrix.CreateTranslation(new Vector3(ScreenManager.CurrentWindowWidth / Zoom / 2 - StaticCamera.HorizontalLetterBox + TopLeft.X, ScreenManager.CurrentWindowHeight / Zoom / 2 - StaticCamera.VerticalLetterBox + TopLeft.Y, 0));
            Projection = Matrix.CreateOrthographic(ScreenManager.CurrentWindowWidth / Zoom, ScreenManager.CurrentWindowHeight / Zoom, -1000, 1000);
        }
    }
}
