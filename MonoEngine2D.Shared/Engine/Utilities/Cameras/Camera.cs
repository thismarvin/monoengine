using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoEngine2D.Engine.Root;
using MonoEngine2D.Engine.Utilities.Display;

namespace MonoEngine2D.Engine.Utilities.Cameras
{
    static class Camera
    {        
        public static Rectangle ScreenBounds { get; private set; }
        public static Matrix Transform { get; private set; }
        public static Vector2 TopLeft;
        public static int Scale { get; private set; }
        public static float Zoom { get; private set; }
        static int pixelWidth;
        static int pixelHeight;

        public static void Initialize(int pixelWidth, int pixelHeight, int scale)
        {
            CreatePixelScene(pixelWidth, pixelHeight, scale);
            Reset(ScreenManager.DefaultWindowWidth, ScreenManager.DefaultWindowHeight);
        }

        private static void CreatePixelScene(int pixelWidth, int pixelHeight, int scale)
        {            
            Scale = scale;
            Camera.pixelWidth = pixelWidth * Scale;
            Camera.pixelHeight = pixelHeight * Scale;
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
                    ScreenBounds = new Rectangle(0, 0, longSide / Scale, shortSide / Scale);
                    break;

                case GameRoot.Orientation.Portrait:
                    Zoom = (float)shortDisplayDimension / shortSide;
                    // Check if letterboxing is required. ??? Im not sure if i really need this.
                    if (longSide * Zoom > longDisplayDimension)
                    {
                        Zoom = (float)longDisplayDimension / longSide;
                    }
                    ScreenBounds = new Rectangle(0, 0, shortSide / Scale, longSide / Scale);
                    break;
            }
        }

        public static void Update()
        {
            Input();
            FinalizeMatrix();
        }

        public static void Update(Vector2 topLeft, float minWidth, float maxWidth, float minHeight, float maxHeight)
        {
            TopLeft = topLeft * Scale;

            Input();
            StayWithinBounds(minWidth, maxWidth, minHeight, maxHeight);
            FinalizeMatrix();
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
            TopLeft.X = (TopLeft.X > maxWidth ? maxWidth : TopLeft.X);
            TopLeft.Y = (TopLeft.Y < minHeight ? 0 : TopLeft.Y);
            TopLeft.Y = (TopLeft.Y + ScreenBounds.Height > maxHeight ? maxHeight - ScreenBounds.Height : TopLeft.Y);
        }

        private static void FinalizeMatrix()
        {
            // Fixed on Top Left.
            Transform = Matrix.CreateTranslation(new Vector3(-TopLeft.X + StaticCamera.HorizontalLetterBox, -TopLeft.Y + StaticCamera.VerticalLetterBox, 0)) *
                        Matrix.CreateScale(new Vector3(Zoom, Zoom, 0));
        }
    }
}
