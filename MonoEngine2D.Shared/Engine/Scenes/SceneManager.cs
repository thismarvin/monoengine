using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine2D.Engine.Utilities.Cameras;
using MonoEngine2D.Shared.Engine.Utilities.Transitions;
using MonoEngine2D.Shared.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoEngine2D.Shared.Engine.Scenes
{
    static class SceneManager
    {
        public static Scene CurrentScene { get; private set; }
        public static Scene NextScene { get; private set; }
        private static List<Scene> scenes;
        private static Transition enterTransition;
        private static Transition exitTransition;
        private static bool transitionInProgress;

        public static void Initialize()
        {
            PreLoadScenes();
            SetStartingScene(SceneType.Test);
        }

        private static void PreLoadScenes()
        {
            scenes = new List<Scene>()
            {
                new Test(),
            };
        }

        private static void SetStartingScene(SceneType scene)
        {
            QueueScene(scene);
        }

        private static Scene ParseSceneType(SceneType scene)
        {
            foreach (Scene s in scenes)
            {
                if (s.Type == scene)
                    return s;
            }
            return null;
        }

        private static void SetupTransitions()
        {
            if (CurrentScene == null)
            {
                exitTransition = null;
                enterTransition = NextScene.EnterTransition;
            }
            else
            {
                exitTransition = CurrentScene.ExitTransition;
                enterTransition = NextScene.EnterTransition;
                exitTransition.Start();
            }

            transitionInProgress = true;
        }

        public static void QueueScene(SceneType scene)
        {
            NextScene = ParseSceneType(scene);
            SetupTransitions();
        }

        private static void UnloadCurrentScene()
        {
            if (CurrentScene == null)
                return;

            CurrentScene.UnloadScene();
        }

        private static void LoadNextScene()
        {
            CurrentScene = NextScene;
            CurrentScene.LoadScene();
            NextScene = null;
        }

        private static void UpdateTransitions(GameTime gameTime)
        {
            if (!transitionInProgress)
                return;

            if (exitTransition != null && exitTransition.InProgress)
            {
                exitTransition.Update(gameTime);
            }
            else if (((exitTransition != null && exitTransition.Done) || (exitTransition == null)) && !enterTransition.Started)
            {
                UnloadCurrentScene();
                LoadNextScene();
                enterTransition.Start();
            }
            else if (enterTransition.InProgress)
            {
                enterTransition.Update(gameTime);
            }
            else if (enterTransition.Done)
            {
                transitionInProgress = false;
            }
        }

        private static void UpdateCurrentScene(GameTime gameTime)
        {
            if (transitionInProgress)
                return;

            CurrentScene.Update(gameTime);
        }

        public static void Update(GameTime gameTime)
        {
            UpdateTransitions(gameTime);
            UpdateCurrentScene(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            CurrentScene.Draw(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, StaticCamera.Transform);
            {
                if (enterTransition != null)
                    enterTransition.Draw(spriteBatch);
                if (exitTransition != null)
                    exitTransition.Draw(spriteBatch);
            }
            spriteBatch.End();

        }
    }
}
