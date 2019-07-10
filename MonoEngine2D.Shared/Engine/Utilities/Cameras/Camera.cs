using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoEngine2D.Engine.Root;
using MonoEngine2D.Engine.Utilities.Display;
using MonoEngine2D.Engine.Utilities.User_Input;

namespace MonoEngine2D.Engine.Utilities.Cameras
{
    static class Camera
    {
        public static Matrix Transform { get; private set; }
        public static Matrix World { get; private set; }
        public static Matrix View { get; private set; }
        public static Matrix Projection { get; private set; }

        public static Rectangle Bounds { get; private set; }

        public static Vector3 TopLeft;

        static Vector3 cameraPosition;
        static Vector3 cameraTarget;
        static Vector3 cameraCenter;

        static float rotation;

        public static int Scale { get; private set; }
        public static float Zoom { get; private set; }

        static int pixelWidth;
        static int pixelHeight;

        public static void Initialize(int pixelWidth, int pixelHeight, int scale)
        {
            CreatePixelScene(pixelWidth, pixelHeight, scale);
            Reset(ScreenManager.DefaultWindowWidth, ScreenManager.DefaultWindowHeight);

            //rotation = (float)Math.PI / 2;

            UpdateMatrices();           
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
            UpdateInput();
            UpdateMatrices();
        }

        public static void Update(Vector2 topLeft, float minWidth, float maxWidth, float minHeight, float maxHeight)
        {
            TopLeft = new Vector3(topLeft.X, topLeft.Y, 0);

            UpdateInput();
            StayWithinBounds(minWidth, maxWidth, minHeight, maxHeight);
            UpdateMatrices();
        }

        private static void UpdateInput()
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
            TopLeft.X = (TopLeft.X < minWidth ? minWidth : TopLeft.X);
            TopLeft.X = (TopLeft.X + Bounds.Width > maxWidth ? maxWidth - Bounds.Width: TopLeft.X);
            TopLeft.Y = (TopLeft.Y < minHeight ? 0 : TopLeft.Y);
            TopLeft.Y = (TopLeft.Y + Bounds.Height > maxHeight ? maxHeight - Bounds.Height : TopLeft.Y);
        }

        /// TODO:
        /// Scale breaks the camera!
        /// Maybe make it so the camera can zoom in on any given point? 
        private static void UpdateMatrices()
        {
            cameraPosition = new Vector3(-TopLeft.X, -TopLeft.Y, -1);
            cameraTarget = new Vector3(cameraPosition.X, cameraPosition.Y, 0);
            cameraCenter = new Vector3(Bounds.Width / 2, Bounds.Height / 2, 0);

            Transform =
                    // M = R * T * S 
                    // Translate the transform matrix to the inverse of the camera's center.
                    Matrix.CreateTranslation(-cameraCenter) *
                    // Rotate the camera relative to the center of the camera bounds.
                    Matrix.CreateRotationZ(rotation) *
                    // Translate the transform matrix to the transform matrix to the inverse of the camera's top left.
                    Matrix.CreateTranslation(-TopLeft) *
                    // Scale the transform matrix by the camera's zoom.
                    Matrix.CreateScale(Zoom) *
                    // Anchor the transform matrix to the center of the screen instead of the top left.
                    Matrix.CreateTranslation(new Vector3(ScreenManager.CurrentWindowWidth / 2, ScreenManager.CurrentWindowHeight / 2, 0));

            World =
                    // Translate to the center of the camera bounds.
                    Matrix.CreateTranslation(cameraCenter) *
                    // Rotate the camera relative to the center of the camera bounds.
                    Matrix.CreateRotationZ(rotation);

            View = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);

            Projection = Matrix.CreateOrthographic(ScreenManager.CurrentWindowWidth / Zoom, ScreenManager.CurrentWindowHeight / Zoom, -1000, 1000);
        }
    }
}
