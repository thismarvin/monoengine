
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine.Engine.Resources;
using Engine.Engine.GameComponents;

namespace Engine.Engine.Utilities
{
    class Text : MonoObject
    {
        List<string> words;
        List<Sprite> sprites;
        Timer timer;
        Sprite.Type textType;
        int textWidth;
        int maximumCharacterCount;
        int spacing;
        string dialouge;
        bool showAll;

        //TimeManager Timer_Effect;
        //int EffectHelper;
        //int EffectIndex;

        bool compact;

        public Text(float x, float y, string dialouge, int maximumCharacterCount, float textSpeed, Sprite.Type textType) : base(x, y)
        {
            words = new List<string>();
            sprites = new List<Sprite>();

            this.maximumCharacterCount = maximumCharacterCount;
            this.dialouge = dialouge;
            this.textType = textType;
            compact = true;

            switch (textType)
            {
                case Sprite.Type.Text8x8:
                    textWidth = 7;
                    spacing = 2;
                    break;
                case Sprite.Type.Text16x16:
                    textWidth = 14;
                    spacing = 4;
                    break;
                case Sprite.Type.Text19x19:
                    this.dialouge = this.dialouge.ToUpper();
                    textWidth = 14;
                    spacing = 8;
                    break;
            }

            SetLocation(X - textWidth, Y);

            timer = new Timer(textSpeed);

            showAll = textSpeed <= 0 ? true : false;

            BreakUpWords();
            CreateText();
        }

        public void SetCompact(bool compact)
        {
            this.compact = compact;
            sprites.Clear();
            CreateText();
        }

        private void BreakUpWords()
        {
            String[] wordsArray = Regex.Split(dialouge, "[ ]+");
            foreach (String s in wordsArray)
            {
                words.Add(s);
            }
        }

        private void CreateText()
        {
            int dialougeIndex = 0;
            int lineIndex = 1;
            int wordLength = 0;
            int y = 0;

            foreach (string s in words)
            {
                if (s.Length + lineIndex + 1 > maximumCharacterCount)
                {
                    wordLength = 0;
                    lineIndex = 1;
                    y++;
                }

                for (int i = 0; i < s.Length; i++)
                {
                    sprites.Add(new Sprite((int)Location.X + wordLength, (int)Location.Y + ((textWidth + spacing) * y), textType));
                    sprites.Last().ChangeInto(dialouge.Substring(dialougeIndex, 1));
                    if (!showAll) { sprites.Last().Show = false; }

                    if (compact)
                    {
                        if (s[i] == 'I' || s[i] == 'i' || s[i] == '!' || s[i] == 'l' || s[i] == '.' || s[i] == ',' || s[i] == '\'' || s[i] == ':' || s[i] == ';')
                        {
                            switch (textType)
                            {
                                case Sprite.Type.Text8x8:
                                    wordLength += 3;
                                    break;
                                case Sprite.Type.Text16x16:
                                    wordLength += 6;
                                    break;
                                case Sprite.Type.Text19x19:
                                    wordLength += 6;
                                    break;
                            }
                        }
                        else if (s[i] == 't')
                        {
                            switch (textType)
                            {
                                case Sprite.Type.Text8x8:
                                    wordLength += 5;
                                    break;
                                case Sprite.Type.Text16x16:
                                    wordLength += 10;
                                    break;
                                case Sprite.Type.Text19x19:
                                    wordLength += 10;
                                    break;
                            }
                        }
                        else if (s[i] == 'f')
                        {
                            switch (textType)
                            {
                                case Sprite.Type.Text8x8:
                                    wordLength += 6;
                                    break;
                                case Sprite.Type.Text16x16:
                                    wordLength += 12;
                                    break;
                                case Sprite.Type.Text19x19:
                                    wordLength += 12;
                                    break;
                            }
                        }
                        else
                        {
                            wordLength += textWidth;
                        }
                    }
                    else
                    {
                        if (s[i] == 'I')
                        {
                            sprites.Last().SetLocation(sprites.Last().X + 4, sprites.Last().Y);
                        }
                        wordLength += textWidth;
                    }

                    dialougeIndex++;
                    lineIndex++;
                }

                // Acounts for space between words.
                dialougeIndex++;
                lineIndex++;
                wordLength += textWidth;
            }

            sprites.Reverse();
        }

        public void Update(GameTime gameTimer)
        {
            timer.Update(gameTimer);

            if (timer.Done && !showAll)
            {
                for (int i = 0; i < sprites.Count; i++)
                {
                    if (!sprites[i].Show)
                    {
                        sprites[i].Show = true;
                        break;
                    }
                    if (i == sprites.Count - 1)
                    {
                        showAll = true;
                    }
                }
                timer.Reset();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Sprite S in sprites)
            {
                S.Draw(spriteBatch);
            }
        }
    }
}
