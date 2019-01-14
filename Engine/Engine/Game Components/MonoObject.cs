
using System;
using Microsoft.Xna.Framework;

namespace Engine.Engine.GameComponents
{
    abstract class MonoObject
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public Vector2 Location { get;  set; }

        public MonoObject(float x, float y)
        {
            SetLocation(x, y);
        }

        public void SetLocation(float x, float y)
        {
            X = x;
            Y = y;
            Location = new Vector2(X, Y);
        }
    }
}
