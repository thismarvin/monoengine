using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Engine.Utilities;

namespace Engine.Engine.Entities.Geometry
{
    class Shape : Entity
    {
        List<Rectangle> rectangles;
        int lineWidth;
        public bool Show { get; set; }
        public Tag ID { get; set; }

        public enum Tag
        {
            None
        }

        public Shape(float x, float y, int width, int height, Color objectColor) : base(x, y, width, height)
        {
            Show = true;
            ObjectColor = objectColor;
            rectangles = new List<Rectangle>();
            ID = Tag.None;
            Setup();
        }

        public Shape(float x, float y, int width, int height, int lineWidth, Color objectColor) : this(x, y, width, height, objectColor)
        {
            this.lineWidth = lineWidth;
            Setup();
        }

        public Shape(float x, float y, int width, int height, Tag tag, Color objectColor) : this(x, y, width, height, objectColor)
        {
            ID = tag;
        }

        public Shape(float x, float y, int width, int height, int lineWidth, Tag tag, Color objectColor) : this(x, y, width, height, lineWidth, objectColor)
        {
            ID = tag;
        }

        private void Setup()
        {
            rectangles.Clear();
            if (lineWidth == 0)
            {
                rectangles.Add(new Rectangle((int)ScaledLocation.X, (int)ScaledLocation.Y, Width * Camera.Scale, Height * Camera.Scale));
            }
            else
            {
                rectangles.Add(new Rectangle((int)ScaledLocation.X, (int)ScaledLocation.Y, Width * Camera.Scale, lineWidth * Camera.Scale));
                rectangles.Add(new Rectangle((int)ScaledLocation.X, (int)ScaledLocation.Y + (Height - lineWidth) * Camera.Scale, Width * Camera.Scale, lineWidth * Camera.Scale));
                rectangles.Add(new Rectangle((int)ScaledLocation.X, (int)ScaledLocation.Y + lineWidth * Camera.Scale, lineWidth * Camera.Scale, Height * Camera.Scale - lineWidth * 2 * Camera.Scale));
                rectangles.Add(new Rectangle((int)ScaledLocation.X + Width * Camera.Scale - lineWidth * Camera.Scale, (int)ScaledLocation.Y + lineWidth * Camera.Scale, lineWidth * Camera.Scale, Height * Camera.Scale - lineWidth * 2 * Camera.Scale));
            }
        }

        public new void SetLocation(float x, float y)
        {
            base.SetLocation(x, y);
            Setup();
        }

        public new void SetCenter(float x, float y)
        {
            base.SetCenter(x, y);
            Setup();
        }

        public new void SetWidth(int width)
        {
            base.SetWidth(width);
            Setup();
        }

        public new void SetHeight(int height)
        {
            base.SetHeight(height);
            Setup();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Show)
            {
                foreach (Rectangle R in rectangles)
                {
                    spriteBatch.Draw(ShapeManager.Texture, R, ObjectColor);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
