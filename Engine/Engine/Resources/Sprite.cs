using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine.Engine.GameComponents;
using Engine.Engine.Utilities;

namespace Engine.Engine.Resources
{
    class Sprite : MonoObject
    {
        Texture2D spriteSheet;
        Rectangle sourceRectangle;
        Rectangle locationRectangle;
        public Color ObjectColor { get; private set; }
        public Vector2 Center { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public float Rotation { get; set; }
        public bool Show { get; set; }
        int frameX;
        int frameY;
        public SpriteEffects Effect { get; set; }
        public Type CurrentSprite { get; private set; }
        public Tag ID { get; set; }
        Vector2 scale;

        public enum Type
        {
            Text8x8, Colon8,
            Text16x16, Colon16,
            Text19x19,

            None
        }

        public enum Tag
        {
            AUTOTILE
        }

        public Sprite(float x, float y, Type sprite) : base(x, y)
        {
            spriteSheet = Assets.Sprites;
            ObjectColor = Color.White;
            Center = Vector2.Zero;
            Rotation = 0;
            Show = true;
            CurrentSprite = sprite;
            scale = new Vector2(Camera.Scale, Camera.Scale);
            InitializeSprite();
        }

        public Sprite(float x, float y, int increment, Type sprite) : this(x, y, sprite)
        {
            IncrementFrame(increment);
        }

        private void InitializeSprite()
        {
            switch (CurrentSprite)
            {
                #region Text
                case Type.Text8x8:
                    SpriteSetup(0, 0, 8, 8);
                    spriteSheet = Assets.Text8x8;
                    break;
                case Type.Colon8:
                    SpriteSetup(0, 0, 8, 8);
                    ChangeInto(":");
                    spriteSheet = Assets.Text8x8;
                    break;
                case Type.Text16x16:
                    SpriteSetup(0, 0, 16, 16);
                    spriteSheet = Assets.Text16x16;
                    break;
                case Type.Colon16:
                    SpriteSetup(0, 0, 16, 16);
                    ChangeInto(":");
                    spriteSheet = Assets.Text16x16;
                    break;
                case Type.Text19x19:
                    SpriteSetup(0, 0, 19, 19);
                    spriteSheet = Assets.Text19x19;
                    break;
                #endregion

                case Type.None:
                    SpriteSetup(0, 0, 0, 0);
                    break;
            }
            sourceRectangle = new Rectangle(frameX, frameY, Width, Height);
        }

        private void SpriteSetup(int frameX, int frameY, int width, int height)
        {
            this.frameX = frameX;
            this.frameY = frameY;
            Width = width;
            Height = height;
        }

        public void IncrementFrame(int increment)
        {
            frameX += increment * Width;
            sourceRectangle = new Rectangle(frameX, frameY, Width, Height);
        }

        public void SetFrame(int frame)
        {
            InitializeSprite();
            frameX += frame * Width;
            sourceRectangle = new Rectangle(frameX, frameY, Width, Height);
        }

        public void SetSprite(Type newSprite)
        {
            CurrentSprite = newSprite;
            InitializeSprite();
        }

        public void SetColor(Color color)
        {
            ObjectColor = color;
        }

        public void SetCenter(float xOffset, float yOffset)
        {
            Center = new Vector2(xOffset, yOffset);
        }

        public void SetScale(float scale)
        {
            this.scale = new Vector2(scale * Camera.Scale, scale * Camera.Scale);
        }

        // Used for Text / Number Class!
        public void ChangeInto(string value)
        {
            char newChar = value.ToCharArray()[0];
            frameX = newChar % 16 * Width;
            frameY = newChar / 16 * Width;
            sourceRectangle = new Rectangle(frameX, frameY, Width, Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            locationRectangle = new Rectangle((int)Location.X, (int)Location.Y, Width, Height);

            if (Show)
            {
                spriteBatch.Draw(spriteSheet, ScaledLocation, sourceRectangle, ObjectColor, Rotation, Center, scale, Effect, Y);
            }
        }
    }
}
