using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using Platformer.Utilities;
using System.Diagnostics;

namespace Platformer.GUI
{
#if ANDROID
    public class AndroidGui
    {
        public Vector2 OriginalPosition { get; set; } // original point, that everything binds to it
        public Vector2 UpPosition { get; set; }
        public Vector2 DownPosition { get; set; }
        public Vector2 RightPosition { get; set; }
        public Vector2 LeftPosition { get; set; }
        private Dictionary<string,Button> Buttons;

        //for tracking which button is pressed
        public bool RightKey { get; set; }
        public bool LeftKey { get; set; }
        public bool UpKey { get; set; }
        public bool DownKey { get; set; }


        public AndroidGui()
        {
            //positions
            OriginalPosition = new Vector2(0, 0);
            UpPosition = new Vector2(0, 0);
            DownPosition = new Vector2(0, 0);
            RightPosition = new Vector2(0, 0);
            LeftPosition = new Vector2(0, 0);

            //all the buttons
            var font = GameData.Fonts["ThaleahFat_Title"];
            var upButton = new Button(GameData.ImageSprites["buttonUp"], font,UpPosition)
            {
                //Position = UpPosition,
                Text = "",
                Type = "game",
            };
            var downButton = new Button(GameData.ImageSprites["buttonDown"], font,DownPosition)
            {
                //Position = DownPosition,
                Text = "",
                Type ="game",
            };
            var leftButton = new Button(GameData.ImageSprites["buttonLeft"], font,LeftPosition)
            {
                //Position = LeftPosition,
                Text = "",
                Type ="game",
            };
            var rightButton = new Button(GameData.ImageSprites["buttonRight"], font,RightPosition)
            {
                //Position = RightPosition,
                Text = "",
                Type = "game",
            };

            upButton.Click += Button_Up_Click;
            downButton.Click += Button_Down_Click;
            leftButton.Click += Button_Left_Click;
            rightButton.Click += Button_Right_Click;


            Buttons = new Dictionary<string, Button>()
                {
                    { "up",upButton },
                    {"down", downButton },
                    {"left", leftButton},
                    {"right", rightButton}
                };

            RightKey = false;
            LeftKey = false;
            UpKey = false;
            DownKey = false;

        }
        private void Button_Up_Click(object sender, EventArgs e)
        {
            UpKey = true;
            
        }
        private void Button_Down_Click(object sender, EventArgs e)
        {
            DownKey = true;
        }
        private void Button_Left_Click(object sender, EventArgs e)
        {
            LeftKey = true;
        }
        private void Button_Right_Click(object sender, EventArgs e)
        {
            RightKey = true;      
        }
        public void Update(Vector2 center, GameTime gameTime)
        {
            //updates based on the position of the player
            OriginalPosition = new Vector2((int)center.X - GameData.AndroidScreenWidth / 7 + 20, (int)center.Y + GameData.AndroidScreenHeight / 6);
            LeftPosition = new Vector2(OriginalPosition.X , OriginalPosition.Y );
            RightPosition = new Vector2(OriginalPosition.X + (int)(1.5f*Buttons["left"].Texture.Width), OriginalPosition.Y );
            UpPosition = new Vector2((int)center.X + GameData.AndroidScreenWidth / 9, OriginalPosition.Y);
            DownPosition = new Vector2(UpPosition.X+ 2*Buttons["down"].Texture.Width , OriginalPosition.Y );

            Buttons["up"].Position = UpPosition;
            Buttons["down"].Position = DownPosition;
            Buttons["left"].Position = LeftPosition;
            Buttons["right"].Position = RightPosition;

            RightKey = false;
            LeftKey = false;
            UpKey = false;
            DownKey = false;

            foreach (var button in Buttons)
                button.Value.Update(gameTime);

        }
        public void Draw(SpriteBatch spriteBatch,GameTime gameTime)
        {
            foreach (var button in Buttons)
                button.Value.Draw(gameTime, spriteBatch);
        }
    }
#endif
}
