using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using Platformer.Utilities;

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

        public AndroidGui()
        {
            //positions
            OriginalPosition = new Vector2(0, 0);
            UpPosition = new Vector2(OriginalPosition.X, OriginalPosition.Y);
            DownPosition = new Vector2(OriginalPosition.X + 5, OriginalPosition.Y + 40);
            RightPosition = new Vector2(OriginalPosition.X + 20, OriginalPosition.Y + 75);
            LeftPosition = new Vector2(OriginalPosition.X + 20, OriginalPosition.Y + 75);

            //all the buttons
            var font = GameData.Fonts["ThaleahFat_Title"];
            var upButton = new Button(GameData.ImageSprites["buttonUp"], font)
            {
                Position = UpPosition,
                Text = "",
            };
            var downButton = new Button(GameData.ImageSprites["buttonDown"], font)
            {
                Position = DownPosition,
                Text = "",
            };
            var leftButton = new Button(GameData.ImageSprites["buttonLeft"], font)
            {
                Position = LeftPosition,
                Text = "",
            };
            var rightButton = new Button(GameData.ImageSprites["buttonRight"], font)
            {
                Position = RightPosition,
                Text = "",
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
        }
        private void Button_Up_Click(object sender, EventArgs e)
        {
            
        }
        private void Button_Down_Click(object sender, EventArgs e)
        {

        }
        private void Button_Left_Click(object sender, EventArgs e)
        {

        }
        private void Button_Right_Click(object sender, EventArgs e)
        {

        }
        public void Update(Vector2 center)
        {
            //updates based on the position of the player
            OriginalPosition = new Vector2((int)center.X, (int)center.Y);
            UpPosition = new Vector2(OriginalPosition.X, OriginalPosition.Y);
            DownPosition = new Vector2(OriginalPosition.X + 5, OriginalPosition.Y + 40);
            RightPosition = new Vector2(OriginalPosition.X + 20, OriginalPosition.Y + 75);
            LeftPosition = new Vector2(OriginalPosition.X + 20, OriginalPosition.Y + 75);

            Buttons["up"].Position = UpPosition;
            Buttons["down"].Position = DownPosition;
            Buttons["left"].Position = LeftPosition;
            Buttons["right"].Position = RightPosition;

        }
        public void Draw(SpriteBatch spriteBatch,GameTime gameTime)
        {
            foreach (var button in Buttons)
                button.Value.Draw(gameTime, spriteBatch);
        }
    }
#endif
}
