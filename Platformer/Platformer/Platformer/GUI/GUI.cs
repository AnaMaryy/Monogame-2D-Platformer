using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Platformer.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platformer.GUI
{
    public class Gui
    {
        private ContentManager _content;
        private Dictionary<string, Texture2D> Textures { get; set; }
        private SpriteFont _font;
        public Vector2 OriginalPosition { get; set; } // original point, that everything binds to it
        public Vector2 HeartsPosition { get; set; }
        public Vector2 CoinsPosition { get; set; }

        public Vector2 BonesPosition { get; set; }
        public Gui(ContentManager content)
        {
            _content = content;

            //create the dictionary with textures
            //health bar
            Textures = new Dictionary<string, Texture2D>();
            Textures.Add("healthBar", _content.Load<Texture2D>("../Content/game/gui/HeartGui"));
            Textures.Add("silverCoin", _content.Load<Texture2D>("../Content/game/coins/silver/0"));
            Textures.Add("bone", _content.Load<Texture2D>("../Content/game/gui/bone64x"));
            _font = _content.Load<SpriteFont>("font/ThaleahFat_Title");

            //positions
            OriginalPosition = new Vector2(20, 20);
            HeartsPosition = new Vector2(OriginalPosition.X, OriginalPosition.Y);
            CoinsPosition = new Vector2(OriginalPosition.X + 5, OriginalPosition.Y + 40);
            BonesPosition = new Vector2(OriginalPosition.X + 20, OriginalPosition.Y + 75);

        }
        public void Update(Vector2 center)
        {
#if DESKTOP
            Vector2 position = new Vector2(center.X - 320,center.Y - 300);
#elif ANDROID
            //TODO : test!
            var x = center.X - GameData.AndroidScreenWidth / 6 + 20;
            var y = center.Y - GameData.AndroidScreenHeight / 6 - 20;
            Vector2 position = new Vector2(x, y);
#endif

            OriginalPosition = new Vector2((int)position.X, (int)position.Y);
            HeartsPosition = new Vector2(OriginalPosition.X, OriginalPosition.Y);
            CoinsPosition = new Vector2(OriginalPosition.X + 5, OriginalPosition.Y + 40);
            BonesPosition = new Vector2(OriginalPosition.X + 20, OriginalPosition.Y + 75);
        }
        public void Draw(SpriteBatch spriteBatch, int currentHealth, int maxHealth, int coinAmount, int currentBones, int neededBones)
        {
            HealthBarDraw(currentHealth, maxHealth, spriteBatch);
            CoinsDraw(coinAmount, spriteBatch);
            BonesDraw(currentBones, neededBones, spriteBatch);
        }
        private void HealthBarDraw(int current, int max, SpriteBatch spriteBatch)
        {
            //atlas rectangles
            var fullheartRect = new Rectangle(0, 0, Textures["healthBar"].Width / 2, Textures["healthBar"].Height);
            var emptyheartRect = new Rectangle(Textures["healthBar"].Width / 2, 0, Textures["healthBar"].Width / 2, Textures["healthBar"].Height);

            int y = (int)HeartsPosition.Y;
            int x = (int)HeartsPosition.X;
            //draw the full hearts

            for (int i = 0; i < max - (max - current); i++)
            {

                spriteBatch.Draw(Textures["healthBar"], new Vector2(x, y), fullheartRect, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
                x += Textures["healthBar"].Width / 2;
            }
            //draw empty hearts
            x += 8;
            for (int i = 0; i < max - current; i++)
            {
                spriteBatch.Draw(Textures["healthBar"], new Vector2(x, y), emptyheartRect, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
                x += Textures["healthBar"].Width / 2;
            }
        }
        private void CoinsDraw(int amount, SpriteBatch spriteBatch)
        {
            //you have to know the center
            int x = (int)CoinsPosition.X;
            int y = (int)CoinsPosition.Y;
            spriteBatch.Draw(Textures["silverCoin"], new Vector2(x, y), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            //draws the string
            string text = amount.ToString();
            int x1 = x + 55;
            int y1 = (int)((y + Textures["silverCoin"].Height / 2) - (_font.MeasureString(text).Y / 2)) + 3; //center of the coin texture
            spriteBatch.DrawString(_font, text, new Vector2(x1, y1), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        }
        private void BonesDraw(int current, int needed, SpriteBatch spriteBatch)
        {
            int x = (int)BonesPosition.X;
            int y = (int)BonesPosition.Y;
            spriteBatch.Draw(Textures["bone"], new Vector2(x, y), null, Color.White, 0.8f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);

            //draws the string
            string textCurrent = current.ToString();
            string textMiddle = "/";
            string textNeeded = needed.ToString();
            //text Current
            int x1 = x + 40;
            int y1 = (int)((y + Textures["bone"].Height / 2) - (_font.MeasureString(textCurrent).Y / 2)) + 5; //center of the coin texture
            spriteBatch.DrawString(_font, textCurrent, new Vector2(x1, y1), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //text middle
            spriteBatch.DrawString(_font, textMiddle, new Vector2(x1 + 25, y1), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //text needed
            spriteBatch.DrawString(_font, textNeeded, new Vector2(x1 + 40, y1), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

        }


    }
}
