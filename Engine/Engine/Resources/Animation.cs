
using System;

using Microsoft.Xna.Framework;

using Engine.Engine.Utilities;

namespace Engine.Engine.Resources
{
    class Animation
    {
        Sprite.Type[] frames;
        Timer timer;
        int index;

        public Animation(Sprite.Type[] frames, float fps)
        {
            this.frames = frames;
            timer = new Timer(fps);
        }

        public Animation(Sprite.Type[] frames, float fps, int currentFrame) : this(frames, fps)
        {
            index = currentFrame;
        }

        public void Update(GameTime gameTimer)
        {
            timer.Update(gameTimer);
            if (timer.Done)
            {
                index = index >= frames.Length - 1 ? 0 : ++index;
                timer.Reset();
            }
        }

        public Sprite.Type CurrentFrame()
        {
            return frames[index];
        }
    }
}
