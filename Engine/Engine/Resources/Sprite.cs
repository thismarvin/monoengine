using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine.Engine.GameComponents;

namespace Engine.Engine.Resources
{
    class Sprite : MonoObject
    {
        Texture2D spriteSheet;
        Rectangle sourceRectangle;
        Rectangle locationRectangle;
        public Color ObjectColor { get; set; }
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

        public enum Type
        {
            AMSQUAREDLOGO,
            TEXT_8x8, COLON8,
            TEXT_16x16, COLON16,
            TEXT_19x19,
            NONE
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
                // How to initialize a Sprite.
                //case Type.NAME_DEFINED_IN_TYPE_ENUM:
                //        SpriteSetup(SPRITES TOPLEFT X POSITION IN SPRITESHEET, SPRITES TOPLEFT Y POSITION IN SPRITESHEET, SPRITES WIDTH, SPRITES HEIGHT);
                //break;

                #region Logo
                case Type.AMSQUAREDLOGO:
                    SpriteSetup(0, 0, 45, 29);
                    break;
                #endregion

                #region Text
                case Type.TEXT_8x8:
                    SpriteSetup(0, 0, 8, 8);
                    spriteSheet = Assets.Text8x8;
                    break;
                case Type.COLON8:
                    SpriteSetup(0, 0, 8, 8);
                    ChangeInto(":");
                    spriteSheet = Assets.Text8x8;
                    break;
                case Type.TEXT_16x16:
                    SpriteSetup(0, 0, 16, 16);
                    spriteSheet = Assets.Text16x16;
                    break;
                case Type.COLON16:
                    SpriteSetup(0, 0, 16, 16);
                    ChangeInto(":");
                    spriteSheet = Assets.Text16x16;
                    break;
                case Type.TEXT_19x19:
                    SpriteSetup(0, 0, 19, 19);
                    spriteSheet = Assets.Text19x19;
                    break;
                    #endregion
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
                spriteBatch.Draw(spriteSheet, locationRectangle, sourceRectangle, ObjectColor, Rotation, Center, Effect, 0);
            }
        }
    }
}
