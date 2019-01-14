
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine.Engine.Utilities;

namespace Engine.Engine.Entities
{
    class Shape : Entity
    {
        List<Rectangle> addRectangles;
        int lineWidth;
        public bool Show { get; set; }
        public Tag ID { get; set; }

        public enum Tag
        {
            NORMAL, ONEWAY, BARRIER
        }
        
        public Shape(float x, float y, int width, int height, Color objectColor) : base(x, y, width, height, Entities.SHAPE)
        {
            Show = true;
            ObjectColor = objectColor;
            addRectangles = new List<Rectangle>();
            Setup();
        }

        public Shape(float x, float y, int width, int height, int lineWidth, Color objectColor) : this(x, y, width, height, objectColor)
        {
            this.lineWidth = lineWidth;
            Setup();
        }

        public Shape(float x, float y, int width, int height, int lineWidth, Tag tag, Color objectColor) : this(x, y, width, height, lineWidth, objectColor)
        {
            ID = tag;
        }

        private void Setup()
        {
            addRectangles.Clear();
            if (lineWidth == 0)
            {
                addRectangles.Add(new Rectangle((int)Location.X, (int)Location.Y, Width, Height));
            }
            else
            {
                addRectangles.Add(new Rectangle((int)Location.X, (int)Location.Y, Width, lineWidth));
                addRectangles.Add(new Rectangle((int)Location.X, (int)Location.Y + Height - lineWidth, Width, lineWidth));
                addRectangles.Add(new Rectangle((int)Location.X, (int)Location.Y, lineWidth, Height));
                addRectangles.Add(new Rectangle((int)Location.X + Width - lineWidth, (int)Location.Y, lineWidth, Height));
            }
        }

        public new void SetLocation(float x, float y)
        {
            base.SetLocation(x, y);
            Setup();        
        }

        public new void SetWidth(int width)
        {
            base.SetWidth(width);
            Setup();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Show)
            {
                foreach (Rectangle R in addRectangles)
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
