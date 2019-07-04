using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine2D.Engine.Utilities.User_Input;
using MonoEngine2D.Shared.Engine.Utilities.Transitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoEngine2D.Shared.Scenes
{
    public enum SceneType
    {
        Test
    }

    abstract class Scene
    {        
        public Transition EnterTransition { get; set; }
        public Transition ExitTransition { get; set; }
        public SceneType Type { get; private set; }
        protected Rectangle SceneBounds { get; private set; }
        protected Input input;

        public Scene(SceneType type)
        {
            Type = type;
            input = new Input(PlayerIndex.One);
            Initialize();
            InitializeTransitions();
        }

        protected abstract void Initialize();

        protected abstract void InitializeTransitions();

        public abstract void LoadScene();

        public abstract void UnloadScene();

        protected abstract void UpdateCamera(GameTime gameTime);

        protected abstract void UpdateInput(GameTime gameTime);

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
