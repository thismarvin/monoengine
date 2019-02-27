
using System;

using Microsoft.Xna.Framework;

using Engine.Engine.Utilities;

namespace Engine.Engine.Resources
{
    class Animation
    {
        Sprite.Type[] frames;
        Timer timer;
        public int CurrentFrame { get; private set; }
        public int TotalFrames { get; private set; }
        public bool Finished { get; private set; }
        Type type;
        public enum Type
        {
            Loop, NoLoop
        }

        public Animation(Sprite.Type[] frames, float fps)
        {
            this.frames = frames;
            TotalFrames = frames.Length;
            timer = new Timer(fps);
            type = Type.Loop;
        }

        public Animation(Sprite.Type[] frames, float fps, int currentFrame) : this(frames, fps)
        {
            CurrentFrame = currentFrame;
        }

        public Animation(int totalFrames, float fps)
        {
            TotalFrames = totalFrames;
            timer = new Timer(fps);
            type = Type.Loop;
        }

        public void Update(GameTime gameTimer)
        {
            timer.Update(gameTimer);
            if (timer.Done)
            {
                switch (type)
                {
                    case Type.Loop:
                        CurrentFrame = CurrentFrame >= TotalFrames - 1 ? 0 : ++CurrentFrame;
                        break;
                    case Type.NoLoop:
                        CurrentFrame = CurrentFrame >= TotalFrames - 1 ? TotalFrames : ++CurrentFrame;
                        break;
                }
                timer.Reset();
            }
            Finished = type == Type.NoLoop && CurrentFrame == TotalFrames;
        }

        public Sprite.Type CurrentSprite()
        {
            return frames[CurrentFrame];
        }

        public void SetCurrentFrame(int frame)
        {
            CurrentFrame = frame;
            timer.Reset();
        }

        public void SetType(Type type)
        {
            this.type = type;
        }
    }
}
