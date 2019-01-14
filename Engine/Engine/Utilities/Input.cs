
using System;
using Engine.Engine.GameComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.Engine.Utilities
{
    class Input
    {
        PlayerIndex playerIndex;
        Timer timer;

        public KeyboardState KeyState { get; private set; }
        public bool KeyReleased { get; set; }

        public MouseState MouseState { get; private set; }
        public bool MouseReleased { get; set; }

        public enum InputType
        { UP, LEFT, DOWN, RIGHT, JUMP, ATTACK, BACK }

        // Specialized for Dynamic Camera.
        public Vector2 DynamicCursorLocation { get; private set; }
        public Rectangle DynamicCollisionRectangle { get; private set; }

        // Specialized for Stationary Camera.
        public Vector2 StaticCursorLocation { get; private set; }
        public Rectangle StaticCollisionRectangle { get; private set; }

        public Input(PlayerIndex playerIndex)
        {
            this.playerIndex = playerIndex;
            timer = new Timer(200);
        }

        public void Update(GameTime gameTimer)
        {
            UpdateKeyboard(gameTimer);
            UpdateMouse();
        }

        private void UpdateKeyboard(GameTime gameTimer)
        {
            KeyState = Keyboard.GetState();
            if (!KeyReleased)
            {
                timer.Update(gameTimer);
            }
            if (timer.Done)
            {
                KeyReleased = true;
                timer.Reset();
            }
        }

        private void UpdateMouse()
        {
            MouseState = Mouse.GetState();
            if (MouseState.LeftButton == ButtonState.Released)
            { MouseReleased = true; }

            switch (Game1.GameOrientation)
            {
                case Game1.Orientation.LANDSCAPE:
                    DynamicCursorLocation = new Vector2(MouseState.X / Camera.Zoom + Camera.TopLeft.X, MouseState.Y / Camera.Zoom - StaticCamera.VerticalLetterBox + Camera.TopLeft.Y);
                    DynamicCollisionRectangle = new Rectangle((int)DynamicCursorLocation.X, (int)DynamicCursorLocation.Y, 1, 1);
                    StaticCursorLocation = new Vector2(MouseState.X / StaticCamera.Zoom, MouseState.Y / StaticCamera.Zoom - StaticCamera.VerticalLetterBox);
                    StaticCollisionRectangle = new Rectangle((int)StaticCursorLocation.X, (int)StaticCursorLocation.Y, 1, 1); ;
                    break;
                case Game1.Orientation.PORTRAIT:
                    DynamicCursorLocation = new Vector2(MouseState.X / Camera.Zoom + Camera.TopLeft.X - StaticCamera.VerticalLetterBox, MouseState.Y / Camera.Zoom + Camera.TopLeft.Y);
                    DynamicCollisionRectangle = new Rectangle((int)DynamicCursorLocation.X, (int)DynamicCursorLocation.Y, 1, 1);
                    StaticCursorLocation = new Vector2(MouseState.X / StaticCamera.Zoom - StaticCamera.VerticalLetterBox, MouseState.Y / StaticCamera.Zoom - StaticCamera.VerticalLetterBox);
                    StaticCollisionRectangle = new Rectangle((int)StaticCursorLocation.X, (int)StaticCursorLocation.Y, 1, 1); ;
                    break;
            }
        }

        public bool LeftClick()
        {
            return MouseState.LeftButton == ButtonState.Pressed;
        }

        public bool RightClick()
        {
            return MouseState.RightButton == ButtonState.Pressed;
        }

        public bool IsKeyDown(InputType inputType)
        {
            switch (inputType)
            {
                case InputType.UP:
                    if (KeyState.IsKeyDown(Keys.W) || KeyState.IsKeyDown(Keys.Up)) { return true; }
                    break;
                case InputType.LEFT:
                    if (KeyState.IsKeyDown(Keys.A) || KeyState.IsKeyDown(Keys.Left)) { return true; }
                    break;
                case InputType.DOWN:
                    if (KeyState.IsKeyDown(Keys.S) || KeyState.IsKeyDown(Keys.Down)) { return true; }
                    break;
                case InputType.RIGHT:
                    if (KeyState.IsKeyDown(Keys.D) || KeyState.IsKeyDown(Keys.Right)) { return true; }
                    break;
                case InputType.JUMP:
                    if (KeyState.IsKeyDown(Keys.Space)) { return true; }
                    break;
                case InputType.ATTACK:
                    if (KeyState.IsKeyDown(Keys.LeftShift) || KeyState.IsKeyDown(Keys.LeftAlt)) { return true; }
                    break;
                case InputType.BACK:
                    if (KeyState.IsKeyDown(Keys.Escape)) { return true; }
                    break;
            }
            return false;
        }

        public bool IsButtonDown(InputType inputType)
        {
            switch (inputType)
            {
                case InputType.UP:
                    if (GamePad.GetState(playerIndex).IsButtonDown(Buttons.DPadUp) || GamePad.GetState(playerIndex).IsButtonDown(Buttons.LeftThumbstickUp) || GamePad.GetState(playerIndex).IsButtonDown(Buttons.RightThumbstickUp)) { return true; }
                    break;
                case InputType.LEFT:
                    if (GamePad.GetState(playerIndex).IsButtonDown(Buttons.DPadLeft) || GamePad.GetState(playerIndex).IsButtonDown(Buttons.LeftThumbstickLeft) || GamePad.GetState(playerIndex).IsButtonDown(Buttons.RightThumbstickLeft)) { return true; }
                    break;
                case InputType.DOWN:
                    if (GamePad.GetState(playerIndex).IsButtonDown(Buttons.DPadDown) || GamePad.GetState(playerIndex).IsButtonDown(Buttons.LeftThumbstickDown) || GamePad.GetState(playerIndex).IsButtonDown(Buttons.RightThumbstickDown)) { return true; }
                    break;
                case InputType.RIGHT:
                    if (GamePad.GetState(playerIndex).IsButtonDown(Buttons.DPadRight) || GamePad.GetState(playerIndex).IsButtonDown(Buttons.LeftThumbstickRight) || GamePad.GetState(playerIndex).IsButtonDown(Buttons.RightThumbstickRight)) { return true; }
                    break;
                case InputType.JUMP:
                    if (GamePad.GetState(playerIndex).IsButtonDown(Buttons.A)) { return true; }
                    break;
                case InputType.ATTACK:
                    if (GamePad.GetState(playerIndex).IsButtonDown(Buttons.X)) { return true; }
                    break;
                case InputType.BACK:
                    if (GamePad.GetState(playerIndex).IsButtonDown(Buttons.Start)) { return true; }
                    break;
            }
            return false;
        }

        public bool Pressing(InputType inputType)
        {
            switch (inputType)
            {
                case InputType.UP:
                    if (IsKeyDown(InputType.UP) || IsButtonDown(InputType.UP)) { return true; }
                    break;
                case InputType.LEFT:
                    if (IsKeyDown(InputType.LEFT) || IsButtonDown(InputType.LEFT)) { return true; }
                    break;
                case InputType.DOWN:
                    if (IsKeyDown(InputType.DOWN) || IsButtonDown(InputType.DOWN)) { return true; }
                    break;
                case InputType.RIGHT:
                    if (IsKeyDown(InputType.RIGHT) || IsButtonDown(InputType.RIGHT)) { return true; }
                    break;
                case InputType.JUMP:
                    if (IsKeyDown(InputType.JUMP) || IsButtonDown(InputType.JUMP)) { return true; }
                    break;
                case InputType.ATTACK:
                    if (IsKeyDown(InputType.ATTACK) || IsButtonDown(InputType.ATTACK)) { return true; }
                    break;
                case InputType.BACK:
                    if (IsKeyDown(InputType.BACK) || (IsButtonDown(InputType.BACK))) { return true; }
                    break;
            }
            return false;
        }
    }
}
