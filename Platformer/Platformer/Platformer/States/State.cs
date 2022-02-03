using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Content;

namespace Platformer.States
{
    public abstract class State
    {
        #region Fields
#if ANDROID
        //for timer
        public bool Start { get; set; }
        public int WaitDuration { get; private set; }
        public int Time { get; private set; }

#endif
        protected ContentManager _content;

        protected GraphicsDevice _graphicsDevice;
        protected SpriteBatch _spriteBatch;

        protected Game1 _game;

        #endregion

        #region Methods
        public State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, SpriteBatch spriteBatch)
        {
            _game = game;

            _graphicsDevice = graphicsDevice;

            _content = content;

            _spriteBatch = spriteBatch;
#if ANDROID
            Start = true;
            WaitDuration = (int)(60 * 1.0f);//immortal for 1.5 s
            Time = 0;
#endif
        }
        public abstract void LoadContent();

        public abstract void Draw(GameTime gameTime);

        public abstract void Update(GameTime gameTime);
#if ANDROID
        public void Timer()// timer used because of android input
        {
            if (Start)
            {
                Time++;
                if (Time >= WaitDuration)
                {
                    Time = 0;
                    Start = false;
                }
            }
        }
#endif

        #endregion
    }
}
