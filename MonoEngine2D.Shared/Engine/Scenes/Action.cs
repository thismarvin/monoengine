using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine2D.Engine.Entities;
using MonoEngine2D.Shared.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoEngine2D.Shared.Engine.Scenes
{
    abstract class Action : Scene
    {
        protected List<Entity> Entities { get; private set; }
        protected List<Entity> EntityBuffer { get; private set; }
        protected Vector2 cameraTopLeft;

        public Action(SceneType type) : base(type)
        {
            Entities = new List<Entity>();
            EntityBuffer = new List<Entity>();
        }

        protected abstract void UpdateEntities(GameTime gameTime);
    }
}
