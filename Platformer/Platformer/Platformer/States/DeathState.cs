using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Platformer.Controls;
using Platformer.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platformer.States
{
    public class DeathState : State
    {
        private Dictionary<string, Texture2D> GraphicsDictionary;
        private List<Component> components;

        public SpriteFont TitleFont { get; private set; }
        public int ScreenWidth { get; }
        public int ScreenHeight { get; }
        private SpriteFont Font { get; set; }

        public DeathState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, SpriteBatch spriteBatch)
      : base(game, graphicsDevice, content, spriteBatch) //menu state ima ste te parametra od svojega starsa
        {
            this.ScreenWidth = GameData.LevelScreenWidth;
            this.ScreenHeight = GameData.LevelScreenHeight;
            var buttonTexture = _content.Load<Texture2D>("menu/button2");
            Font = _content.Load<SpriteFont>("font/ThaleahFat_Normal");
            TitleFont = _content.Load<SpriteFont>("font/ThaleahFat_Title");
            //change width and height if values differ
            if (_graphicsDevice.Viewport.Width != ScreenWidth || _graphicsDevice.Viewport.Height != ScreenHeight)
            {
                _game.ChangeScreenSize(ScreenWidth, ScreenHeight);
            }


            var tryAgainButton = new Button(buttonTexture, Font)
            {
                Position = new Vector2(ScreenWidth / 2, 290),
                Text = "Try Again",
            };
            tryAgainButton.Click += Button_Try_Again_Click;


            var mainMenuButton = new Button(buttonTexture, Font)
            {
                Position = new Vector2(ScreenWidth / 2, 390),
                Text = "Main Menu",
            };

            mainMenuButton.Click += Button_Main_Menu_Click;

            components = new List<Component>()
            {
                tryAgainButton,
                mainMenuButton
            };
        }
        private void Button_Try_Again_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new LevelState(_game, _graphicsDevice, _content, _spriteBatch));

        }
        private void Button_Main_Menu_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, _spriteBatch));

        }
        public override void LoadContent()
        {
            GraphicsDictionary = SupportingFunctions.LoadBackground(_content);


        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in components)
                component.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            SupportingFunctions.DrawBackground(GraphicsDictionary, _spriteBatch, ScreenWidth, ScreenHeight);
            var x = (ScreenWidth / 2) - (TitleFont.MeasureString("GAME OVER :(").X / 2);
            _spriteBatch.DrawString(TitleFont, "GAME OVER :(", new Vector2(x, 200), Color.Black);

            foreach (var component in components)
                component.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
        }
    }
}
