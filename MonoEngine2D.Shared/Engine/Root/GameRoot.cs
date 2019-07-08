using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoEngine2D.Engine.Level;
using MonoEngine2D.Engine.Resources;
using MonoEngine2D.Engine.Utilities.Palettes;
using MonoEngine2D.Engine.Utilities.Display;
using MonoEngine2D.Engine.Utilities.Cameras;
using MonoEngine2D.Engine.Utilities.Audio;
using MonoEngine2D.Engine.Utilities.Misc;
using MonoEngine2D.Shared.Engine.Scenes;

namespace MonoEngine2D.Engine.Root
{
    public class GameRoot : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        string title;
        bool startFullscreen;
        int defaultWindowHeight;
        int defaultWindowWidth;
        int pixelWidth;
        int pixelHeight;
        int scale;

        public enum Orientation
        { Landscape, Portrait }
        public static Orientation GameOrientation { get; set; }

        public static bool ExitGame { get; set; }
        public static bool DebugMode { get; set; }
        bool released;

        RasterizerState rasterizerState;

        public GameRoot()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            title = "UberCoolCustomCraftedMonoGame2DEngine";

            IsMouseVisible = true;
            startFullscreen = false;

            SetupWindow(1280, 720, Orientation.Landscape);
            SetupPixelScene(320, 180, 1);
            EnableVSync(true);
            graphics.ApplyChanges();
        }

        private void SetupWindow(int defaultWindowWidth, int defaultWindowHeight, Orientation orientation)
        {
            this.defaultWindowWidth = defaultWindowWidth;
            this.defaultWindowHeight = defaultWindowHeight;
            GameOrientation = orientation;

            // Set Supported Orientations.
            switch (GameOrientation)
            {
                case Orientation.Landscape:
                    graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
                    break;
                case Orientation.Portrait:
                    graphics.SupportedOrientations = DisplayOrientation.Portrait;
                    break;
            }

            // Make sure default dimensions are in line with the game's orientation.
            if ((GameOrientation == Orientation.Landscape && this.defaultWindowHeight > this.defaultWindowWidth) || (GameOrientation == Orientation.Portrait && this.defaultWindowWidth > this.defaultWindowHeight))
            {
                int copy = this.defaultWindowWidth;
                this.defaultWindowWidth = this.defaultWindowHeight;
                this.defaultWindowHeight = copy;
            }

            // Set Screen Dimensions.
#if __IOS__ || __ANDROID__
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width; ;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
#else
            graphics.PreferredBackBufferWidth = this.defaultWindowWidth;
            graphics.PreferredBackBufferHeight = this.defaultWindowHeight;
#endif
        }

        private void SetupPixelScene(int pixelWidth, int pixelHeight, int scale)
        {
            this.pixelWidth = pixelWidth;
            this.pixelHeight = pixelHeight;
            this.scale = scale;
        }

        private void EnableVSync(bool vsync)
        {
            if (vsync)
            {
                graphics.SynchronizeWithVerticalRetrace = true;
                IsFixedTimeStep = false;
            }
            else
            {
                SetTargetFPS(500);
            }
        }

        private void SetTargetFPS(int fps)
        {
            TargetElapsedTime = TimeSpan.FromTicks((long)(1f / fps * 10000000));
        }

        private void DebugModeToggle()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F12) && released)
            {
                DebugMode = !DebugMode;
                released = false;

                rasterizerState = new RasterizerState();
                rasterizerState.FillMode = DebugMode ? FillMode.WireFrame : FillMode.Solid;
            }

            if (!released && Keyboard.GetState().IsKeyUp(Keys.F12))
            {
                released = true;
            }

            Window.Title = DebugMode ? title + " " + Math.Round(ScreenManager.FPS) + " FPS" : title;
        }

        private void ExitGameLogic()
        {
#if !__IOS__ && !__TVOS__
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) { ExitGame = true; }
            if (ExitGame) { Exit(); }
#endif
        }

        protected override void Initialize()
        {
            ShapeManager.Initialize(graphics);
            ScreenManager.Initialize(defaultWindowWidth, defaultWindowHeight);
            Camera.Initialize(pixelWidth, pixelHeight, scale);
            StaticCamera.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            rasterizerState = new RasterizerState();

            Assets.LoadContent(Content);

            SoundManager.Initialize();
            SceneManager.Initialize();           

#if !__IOS__ && !__ANDROID__
            if (startFullscreen)
                ScreenManager.StartFullScreen(graphics);
#endif            
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();

            spriteBatch.Dispose();
            ShapeManager.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            ExitGameLogic();
            DebugModeToggle();

            ScreenManager.Update(gameTime, graphics);
            SceneManager.Update(gameTime);
            ShapeManager.Update(GraphicsDevice);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(PICO8.SkyBlue);
            GraphicsDevice.RasterizerState = rasterizerState;


            SceneManager.Draw(spriteBatch);
            StaticCamera.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
