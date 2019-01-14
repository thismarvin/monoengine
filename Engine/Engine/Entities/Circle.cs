using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Engine.GameComponents;
using Engine.Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Engine.Entities
{
    class Circle : Entity
    {
        List<Line> lines;
        public int Radius { get; private set; }
        int lineWidth;
        const float INCREMENT = (float)Math.PI * 2 / 359;

        public Circle(float x, float y, int radius) : base(x, y, 1, 1, Entities.CIRCLE)
        {
            lines = new List<Line>();
            Radius = radius;
            lineWidth = radius;

            CreateCircle();
        }

        public Circle(float x, float y, int radius, int lineWidth) : this(x, y, radius)
        {
            this.lineWidth = lineWidth;
            CreateCircle();
        }

        public Circle(float x, float y, int radius, Color objectColor) : this(x, y, radius)
        {
            ObjectColor = objectColor;
            CreateCircle();
        }

        public Circle(float x, float y, int radius, int lineWidth, Color objectColor) : this(x, y, radius)
        {
            this.lineWidth = lineWidth;
            ObjectColor = objectColor;
            CreateCircle();
        }

        private void CreateCircle()
        {
            lines.Clear();
            for (float i = 0; i < Math.PI; i += INCREMENT)
            {
                lines.Add(new Line(X - Radius + CircleX(i), Y + CircleY(i), X - Radius + CircleX(i + INCREMENT), Y + CircleY(i + INCREMENT), lineWidth, ObjectColor));
            }
        }

        public new void SetLocation(float x, float y)
        {
            base.SetLocation(x, y);
            CreateCircle();
        }

        public new void SetCollisionRectangle(float x, float y, int width, int height)
        {
            SetLocation(x, y);
        }

        public void SetLineWidth(int lineWidth)
        {
            this.lineWidth = lineWidth <= Radius ? lineWidth : Radius;
            CreateCircle();
        }

        public void SetRadius(int radius)
        {
            Radius = radius;
            CreateCircle();
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
