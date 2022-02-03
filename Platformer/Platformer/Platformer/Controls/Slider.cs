using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Controls
{
    public class Slider : Component
    {
        #region Fields

        private MouseState _currentMouse;

        private MouseState _previousMouse;


        private bool _isHovering;


        private Texture2D _textureSlider;
        private Texture2D _textureBall;


        #endregion

        #region Properties

        public EventHandler Click;

        public bool Clicked { get; private set; }

        public float Layer { get; set; } // what texture is on top

        public Vector2 Origin // origin point of slider
        {
            get
            {
                return new Vector2(_textureSlider.Width / 2, _textureSlider.Height / 2);
            }
        }


        public Vector2 Position { get; set; }//position of the slider and the ball
        private Vector2 PositionBall
        {
            get
            ; set;
        }
        public Vector2 OriginBall // origin point of slider
        {
            get
            {
                return new Vector2(_textureBall.Width / 2, _textureBall.Height / 2);
            }
        }

        public Rectangle RectangleSlider //rectangle of the slider
        {
            get
            {
                return new Rectangle((int)Position.X - ((int)Origin.X), (int)Position.Y - (int)Origin.Y, _textureSlider.Width, _textureSlider.Height);
            }
        }

        public string Text;

        public float Value { get; set; }

        #endregion

        #region Methods

        public Slider(Texture2D texture, Texture2D texture1, Vector2 position, float value)
        {
            _textureSlider = texture;
            _textureBall = texture1;
            Position = position;
            Value = value;
            PositionBall = new Vector2((Position.X - _textureSlider.Width / 2) + (Value * RectangleSlider.Width), Position.Y);

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var colour = Color.White;

            if (_isHovering)
                colour = Color.Gray;
            //position -> in space ,Origin -> origin point of the 

            spriteBatch.Draw(_textureSlider, Position, null, colour, 0f, Origin, 1f, SpriteEffects.None, Layer);
            spriteBatch.Draw(_textureBall, PositionBall, null, colour, 0f, OriginBall, 2f, SpriteEffects.None, Layer);


        }

        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = false;

            //PositionBall = new Vector2((Position.X - _textureSlider.Width / 2) + (Value * RectangleSlider.Width), Position.Y);


            //TODO -> check this intersects
            if (mouseRectangle.Intersects(RectangleSlider))
            {
                _isHovering = true;



                //if new click
                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    //change the ball position
                    Value = ((float)_currentMouse.X - (float)RectangleSlider.X) / (float)RectangleSlider.Width;

                    PositionBall = new Vector2((Position.X - _textureSlider.Width / 2) + (Value * RectangleSlider.Width), Position.Y);
                    Click?.Invoke(this, new EventArgs());

                }
            }
        }
        #endregion
    }
}
