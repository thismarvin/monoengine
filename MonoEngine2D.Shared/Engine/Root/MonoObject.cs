using System;
using Microsoft.Xna.Framework;
using MonoEngine2D.Engine.Utilities.Cameras;

namespace MonoEngine2D.Engine.Root
{
    abstract class MonoObject: IComparable
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int LayerDepth { get; set; }
        public bool Remove { get; set; }
        public Vector2 Location { get; set; }
        public Vector2 Center { get; private set; }
        public Vector2 ScaledLocation { get { return new Vector2(Location.X * Camera.Scale, Location.Y * Camera.Scale); } }
        public Rectangle CollisionRectangle { get; private set; }
        public Rectangle ScaledCollisionRectangle { get { return new Rectangle(CollisionRectangle.X * Camera.Scale, CollisionRectangle.Y * Camera.Scale, Width * Camera.Scale, Height * Camera.Scale); } }
        public Color ObjectColor { get; set; }

        public MonoObject(float x, float y, int width, int height)
        {
            X = x;
            Y = y;
            Location = new Vector2(X, Y);
            Center = new Vector2(X + Width / 2, Y + Height / 2);
            Width = width;
            Height = height;
            CollisionRectangle = new Rectangle((int)X, (int)Y, Width, Height);
            LayerDepth = 1;
            ObjectColor = Color.White;
        }

        public void SetLocation(float x, float y)
        {
            X = x;
            Y = y;
            Location = new Vector2(X, Y);
            CollisionRectangle = new Rectangle((int)X, (int)Y, Width, Height);
            Center = new Vector2(X + Width / 2, Y + Height / 2);
        }

        public void SetCenter(float x, float y)
        {
            SetLocation(x - Width / 2, y - Height / 2);
        }

        public void SetCollisionRectangle(float x, float y, int width, int height)
        {
            Width = width;
            Height = height;
            SetLocation(x, y);
        }

        public void SetWidth(int width)
        {
            Width = width;
            CollisionRectangle = new Rectangle((int)X, (int)Y, Width, Height);
        }

        public void SetHeight(int height)
        {
            Height = height;
            CollisionRectangle = new Rectangle((int)X, (int)Y, Width, Height);
        }

        public void SetColor(Color color)
        {
            ObjectColor = color;
        }

        public int CompareTo(object obj)
        {
            return LayerDepth.CompareTo(((MonoObject)obj).LayerDepth);
        }
    }
}
