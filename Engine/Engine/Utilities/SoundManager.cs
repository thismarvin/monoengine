using Engine.Engine.GameComponents;
using Engine.Engine.Resources;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Engine.Utilities
{
    static class SoundManager
    {
        public static float Volume { get; set; }

        public enum Sound
        {
           NONE
        }

        public static void Initialize()
        {
            Volume = 0.5f;
        }

        public static void IncrementVolume(float increment)
        {
            Volume = ((int)Math.Round(Volume * 100) + increment) / 100;
            Volume = Volume > 1 ? 0 : Volume;
        }

        public static void Play(Sound sound)
        {
            switch (sound)
            {
                case Sound.NONE:
                    break;
            }
        }
    }
}
