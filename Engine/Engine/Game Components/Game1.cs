
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Engine.Engine.Utilities;
using Engine.Engine.Level;
using Engine.Engine.Resources;

namespace Engine.Engine.GameComponents
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        String title;
        int defaultWindowHeight;
        int defaultWindowWidth;

        public enum Mode
        { MENU, PLAYFIELD, NONE }
        public static Mode GameMode { get; set; }

        public enum Orientation
        { LANDSCAPE, PORTRAIT }
        public static Orientation GameOrientation { get; set; }

        public static bool ExitGame { get; set; }
        public static bool DebugMode { get; set; }
        bool released;

        public static SaveManager Save { get; private set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            title = "Mono Engine";

            // Toggle Mouse Visibility.
            IsMouseVisible = true;

            int displayWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int displayHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            GameOrientation = Orientation.LANDSCAPE;
            GameMode = Mode.PLAYFIELD;

            defaultWindowWidth = 21*50; //(int)(DisplayWidth * (2f / 3f));
            defaultWindowHeight = 9*50; //(int)(DisplayHeight * (2f / 3f));
            
            // Set Screen Dimensions.
            graphics.PreferredBackBufferWidth = defaultWindowWidth;
            graphics.PreferredBackBufferHeight = defaultWindowHeight;

            // Toggle VSync.
            //graphics.SynchronizeWithVerticalRetrace = false;
            //base.IsFixedTimeStep = false;

            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            ShapeManager.Initialize(graphics);           
            ScreenManager.Initialize(defaultWindowWidth, defaultWindowHeight);

            Camera.Initialize();
            StaticCamera.Initialize();

            // Start Game FullScreen.
            //ScreenManager.StartFullScreen(graphics);         
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Assets.LoadContent(Content);

            SoundManager.Initialize();
            HUD.Initialize();
            Playfield.Initialize();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();

            spriteBatch.Dispose();
            ShapeManager.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            #region Debug Toggle
            if (Keyboard.GetState().IsKeyDown(Keys.F12) && released)
            {
                DebugMode = !DebugMode;
                released = false;       
            }
            if (!released && Keyboard.GetState().IsKeyUp(Keys.F12)) { released = true; }
            if (DebugMode) { Window.Title = title + " " + Math.Round(ScreenManager.FPS) + " FPS"; }
            else { Window.Title = title; }
            #endregion

            #region Exit
            if (ExitGame) { Exit(); }
            #endregion

            ScreenManager.Update(gameTime, graphics);
            Playfield.Update(gameTime);

            switch (GameMode)
            {
                case Mode.MENU:
                    break;
                case Mode.PLAYFIELD:
                    Playfield.Update(gameTime);
                    HUD.Update(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Palette.ConeOrange);

            // Dynamic Camera.
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, Camera.Transform);
            switch (GameMode)
            {
                case Mode.PLAYFIELD:
                    Playfield.Draw(spriteBatch);
                    break;
            }
            spriteBatch.End();

            // Static Camera.
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, StaticCamera.Transform);
            switch (GameMode)
            {
                case Mode.MENU:
                    break;
                case Mode.PLAYFIELD:
                    HUD.Draw(spriteBatch);
                    break;
            }                    
            StaticCamera.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
