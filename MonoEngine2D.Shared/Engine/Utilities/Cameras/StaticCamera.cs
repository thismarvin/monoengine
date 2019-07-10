using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine2D.Engine.Root;
using MonoEngine2D.Engine.Entities.Geometry;
using MonoEngine2D.Engine.Utilities.Display;

namespace MonoEngine2D.Engine.Utilities.Cameras
{
    static class StaticCamera
    {
        public static Matrix Transform { get; private set; }
        public static float Zoom { get; private set; }
        public static float VerticalLetterBox { get; private set; }
        public static float HorizontalLetterBox { get; private set; }

        static Shape topLetterBox;
        static Shape bottomLetterBox;
        static Shape leftLetterBox;
        static Shape rightLetterBox;

        public static void Initialize()
        {
            Reset(ScreenManager.DefaultWindowWidth, ScreenManager.DefaultWindowHeight);
        }

        private static void ResetZoom()
        {
            Zoom = Camera.Zoom;
        }

        private static void SetupLandscapeLetterBox(int windowWidth, int windowHeight)
        {
            int longDisplayDimension = windowWidth > windowHeight ? windowWidth : windowHeight;
            int shortDisplayDimension = windowWidth < windowHeight ? windowWidth : windowHeight;

            VerticalLetterBox = (shortDisplayDimension / Zoom - Camera.Bounds.Height * Camera.Scale) / 2;
            topLetterBox = new Shape(-128, -(int)VerticalLetterBox - 128, Camera.Bounds.Width + 128 * 2, (int)VerticalLetterBox + 128, Color.Black);
            bottomLetterBox = new Shape(-128, Camera.Bounds.Height, Camera.Bounds.Width + 128 * 2, (int)VerticalLetterBox + 128, Color.Black);

            if (!ScreenManager.WideScreenSupport)
            {
                HorizontalLetterBox = (longDisplayDimension / Zoom - Camera.Bounds.Width * Camera.Scale) / 2;
                leftLetterBox = new Shape(-128 - (int)HorizontalLetterBox, -128, 128 + (int)HorizontalLetterBox, Camera.Bounds.Height + 128 * 2, Color.Black);
                rightLetterBox = new Shape(Camera.Bounds.Width, -128, (int)HorizontalLetterBox + 128, Camera.Bounds.Height + 128 * 2, Color.Black);
            }
        }

        private static void SetupPortraitLetterBox(int windowWidth, int windowHeight)
        {
            int longDisplayDimension = windowWidth > windowHeight ? windowWidth : windowHeight;
            int shortDisplayDimension = windowWidth < windowHeight ? windowWidth : windowHeight;

            HorizontalLetterBox = (shortDisplayDimension / Zoom - Camera.Bounds.Width * Camera.Scale) / 2;
            leftLetterBox = new Shape(-(int)HorizontalLetterBox - 128, -128, (int)HorizontalLetterBox + 128, Camera.Bounds.Height + 128 * 2, Color.Black);
            rightLetterBox = new Shape(Camera.Bounds.Width, -128, (int)HorizontalLetterBox + 128, Camera.Bounds.Height + 128 * 2, Color.Black);

            VerticalLetterBox = (longDisplayDimension / Zoom - Camera.Bounds.Height * Camera.Scale) / 2;
            topLetterBox = new Shape(-128, -(int)VerticalLetterBox - 128, Camera.Bounds.Width + 128 * 2, (int)VerticalLetterBox + 128, Color.Black);
            bottomLetterBox = new Shape(-128, Camera.Bounds.Height, Camera.Bounds.Width + 128 * 2, (int)VerticalLetterBox + 128, Color.Black);
        }

        private static void FinalizeMatrix()
        {
            // Fixed on Top Left.
            Transform = Matrix.CreateTranslation(new Vector3(HorizontalLetterBox, VerticalLetterBox, 0)) *
                        Matrix.CreateScale(new Vector3(Zoom, Zoom, 0));
        }

        public static void Reset(int windowWidth, int windowHeight)
        {
            ResetZoom();
            switch (GameRoot.GameOrientation)
            {
                case GameRoot.Orientation.Landscape:
                    SetupLandscapeLetterBox(windowWidth, windowHeight);
                    break;
                case GameRoot.Orientation.Portrait:
                    SetupPortraitLetterBox(windowWidth, windowHeight);
                    break;
            }
            FinalizeMatrix();
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, Transform);
            {
                switch (GameRoot.GameOrientation)
                {
                    case GameRoot.Orientation.Landscape:
                        topLetterBox.Draw(spriteBatch);
                        bottomLetterBox.Draw(spriteBatch);
                        if (!ScreenManager.WideScreenSupport)
                        {
                            leftLetterBox.Draw(spriteBatch);
                            rightLetterBox.Draw(spriteBatch);
                        }
                        break;

                    case GameRoot.Orientation.Portrait:
                        leftLetterBox.Draw(spriteBatch);
                        rightLetterBox.Draw(spriteBatch);
                        break;
                }                
            }
            spriteBatch.End();
        }
    }
}