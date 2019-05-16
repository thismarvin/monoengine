using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Engine.GameComponents;
using Engine.Engine.Utilities;

namespace Engine.Engine.Entities.Geometry
{
    class Circle : Entity
    {
        List<Line> lines;
        public float Radius { get; private set; }
        public float LineWidth { get; private set; }
        const float INCREMENT = (float)Math.PI * 2 / 360;

        public Circle(float x, float y, float radius) : base(x, y, 1, 1)
        {
            lines = new List<Line>();
            Radius = radius;
            LineWidth = radius;

            CreateCircle(X, Y);
        }

        public Circle(float x, float y, float radius, float lineWidth) : this(x, y, radius)
        {
            LineWidth = lineWidth;
            CreateCircle(X, Y);
        }

        public Circle(float x, float y, float radius, Color objectColor) : this(x, y, radius)
        {
            ObjectColor = objectColor;
            CreateCircle(X, Y);
        }

        public Circle(float x, float y, float radius, float lineWidth, Color objectColor) : this(x, y, radius)
        {
            LineWidth = lineWidth;
            ObjectColor = objectColor;
            CreateCircle(X, Y);
        }

        private void CreateCircle(float x, float y)
        {
            lines.Clear();
            for (float i = 0; i < Math.PI; i += INCREMENT / 2)
            {
                lines.Add(new Line(x - Radius + CircleX(i), y + CircleY(i), x - Radius + CircleX(i + INCREMENT), y + CircleY(i + INCREMENT), LineWidth, ObjectColor));
            }
        }

        public new void SetLocation(float x, float y)
        {
            base.SetLocation(x, y);
            CreateCircle(X, Y);
        }

        public new void SetCenter(float x, float y)
        {
            SetLocation(x, y);
        }

        public new void SetCollisionRectangle(float x, float y, int width, int height)
        {
            SetLocation(x, y);
        }

        public void SetLineWidth(float lineWidth)
        {
            LineWidth = lineWidth <= Radius ? lineWidth : Radius;
            CreateCircle(X, Y);
        }

        public void SetRadius(float radius)
        {
            Radius = radius;
            CreateCircle(X, Y);
        }

        private float CircleX(float x)
        {
            return (float)((Math.Cos(x)) * Math.Cos(x)) * Radius * 2;
        }

        private float CircleY(float y)
        {
            return (float)((Math.Cos(y)) * Math.Sin(y)) * Radius * 2;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Line l in lines)
            {
                l.Draw(spriteBatch);
            }

            if (Game1.DebugMode)
            {
                spriteBatch.Draw(ShapeManager.Texture, CollisionRectangle, Palette.TreeGreen);
            }
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
