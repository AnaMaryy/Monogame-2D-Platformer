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
        }
        public abstract void LoadContent();

        public abstract void Draw(GameTime gameTime);

        public abstract void Update(GameTime gameTime);

        #endregion
    }
}
