
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Engine.Engine.GameComponents;

namespace Engine.Engine.Entities
{
    abstract class Entity : MonoObject, IComparable
    {
        public Rectangle CollisionRectangle { get; private set; }
        public Color ObjectColor { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int LayerDepth { get; set; }
        public bool Remove { get; set; }
        public Entities Type { get; private set; }

        public enum Entities
        {
            PLAYER, SHAPE, CIRCLE, LINE
        }

        public Entity(float x, float y, int width, int height, Entities type) : base(x, y)
        {
            ObjectColor = Color.White;
            Width = width;
            Height = height;
            LayerDepth = 1;
            CollisionRectangle = new Rectangle((int)X, (int)Y, Width, Height);
            Type = type;
        }

        public new void SetLocation(float x, float y)
        {
            base.SetLocation(x, y);
            SetCollisionRectangle(X, Y, Width, Height);
        }

        public void SetCollisionRectangle(float x, float y, int width, int height)
        {
            base.SetLocation(x, y);
            Width = width;
            Height = height;
            CollisionRectangle = new Rectangle((int)X, (int)Y, Width, Height);
        }

        public void SetWidth(int width)
        {
            Width = width;
            SetCollisionRectangle(X, Y, Width, Height);
        }

        public void SetHeight(int height)
        {
            Height = height;
            SetCollisionRectangle(X, Y, Width, Height);
        }

        public int CompareTo(object obj)
        {
            return LayerDepth.CompareTo(((Entity)obj).LayerDepth);
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
