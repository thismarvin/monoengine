﻿
using System;

using Microsoft.Xna.Framework;

namespace Engine.Engine.Utilities
{
    class Timer
    {
        public bool Done { get; private set; }
        float duration;
        float elapsedTime;              
        float startingTime;
        bool executeOnce;

        public Timer(float timerLength)
        {
            duration = timerLength;
        }

        public void Reset()
        {
            elapsedTime = 0;
            Done = false;
            executeOnce = false;
        }

        private void Setup(GameTime gameTime)
        {
            if (!executeOnce)
            {
                startingTime = (float)gameTime.TotalGameTime.TotalMilliseconds;
                executeOnce = true;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!Done)
            {
                Setup(gameTime);
                elapsedTime = (float)gameTime.TotalGameTime.TotalMilliseconds - startingTime;

                if (elapsedTime >= duration)
                {
                    Done = true;
                }
            }
        }
    }
}
