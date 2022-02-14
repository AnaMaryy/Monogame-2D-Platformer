using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.Controls;
using Platformer.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platformer.Sprites
{
   public class StoryBoard // used for displaying the story line in game
    {
        public int Width;
        public int Height;
        public Texture2D Texture;
        public Button Button;
        public bool Clicked;
        public Vector2 Position;
        public Vector2 Origin;
        public float Scale;
        public StoryBoard(Texture2D texture, string buttonText)
        {
            Texture = texture;
            Width = Texture.Width;
            Height = Texture.Height;
            Button = new Button(GameData.ImageSprites["button3"], GameData.Fonts["ThaleahFat_Normal"], new Vector2(800,420),null)
            {
                Text = buttonText,
                Type = "game",
            };
            Clicked = false;
            Position = new Vector2(GameData.LevelScreenWidth / 2, GameData.LevelScreenHeight / 2);
            Origin = new Vector2(Width / 2, Height / 2);
            Button.Click += Button_Click;
            Scale = 0.8f;

           
        }
        private void Button_Click(object sender, EventArgs e)
        {
            Clicked = true;
        }
        public void Draw( GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Clicked)
            {
                spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Origin, Scale, SpriteEffects.None, 0f);
                Button.Draw(gameTime, spriteBatch);
            }
           
        }
        public void Update(GameTime gameTime, Vector2 centerPosition)
        {
            Position = centerPosition;
            var buttonPosition = new Vector2(centerPosition.X + 340 * Scale, centerPosition.Y + 180 * Scale);
            Button.Position = buttonPosition;
            //Button.Rectangle = new Rectangle((int)centerPosition.X, (int)centerPosition.Y, Button.Rectangle.Width, Button.Rectangle.Height);
            Button.Update(gameTime);
        }
    }
}
