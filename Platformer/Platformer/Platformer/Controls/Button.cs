using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Platformer.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Platformer.Controls
{
    public class Button : Component
    {
        #region Fields

        private MouseState _currentMouse;

        private MouseState _previousMouse;

        private SpriteFont _font;

        private bool _isHovering;


        public Texture2D Texture;

        #endregion

        #region Properties

        public EventHandler Click;

        public bool Clicked { get; private set; }

        public float Layer { get; set; } // what texture is on top

        // origin point of button probably
        public Vector2 Origin {get;set;}

        public Color PenColour { get { return Color.Black; } }

        public Vector2 Position { get; set; }
        //rectangle of the button
        public Rectangle Rectangle { get; set; } //rectangle of the button
        

        public string Text;
        public string Type; // type of button -> android game gui button ; game

        #endregion

        #region Methods

        public Button(Texture2D texture, SpriteFont font, Vector2 position)
        {
            Texture = texture;

            _font = font;
            Position = position;
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            Rectangle = new Rectangle((int)Position.X - ((int)Origin.X), (int)Position.Y - (int)Origin.Y, Texture.Width, Texture.Height);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var colour = Color.White;

            if (_isHovering)
                colour = Color.Gray;
            //where the f do we get the position of the button?
            spriteBatch.Draw(Texture, Position, null, colour, 0f, Origin, 1f, SpriteEffects.None, Layer);


            if (!string.IsNullOrEmpty(Text)) //draws the string
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, Layer + 0.01f);

            }
        }
#if DESKTOP
        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = false;
            Mouse.SetCursor(MouseCursor.Arrow);

            //TODO -> check this intersects
            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;



                //if new click
                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }
#elif ANDROID
        public override void Update(GameTime gameTime)
        {

            TouchCollection touchState = TouchPanel.GetState();
            //if new click
            if(Type == "game")
            {
                Rectangle = new Rectangle((int)Position.X - ((int)Origin.X), (int)Position.Y - (int)Origin.Y, Texture.Width, Texture.Height);
                if (TouchInput.CheckTouchGameGui(Rectangle, touchState))
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
            else
            {
                if (TouchInput.CheckTouch(Rectangle, touchState))
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
           
        }
       
#endif
        #endregion
    }
}
