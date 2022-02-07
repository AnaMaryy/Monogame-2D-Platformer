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
    public class NextLevelState : State
    {
        private Dictionary<string, Texture2D> GraphicsDictionary;
        private List<Component> components;

        public SpriteFont TitleFont { get; private set; }
        public int ScreenWidth { get; }
        public int ScreenHeight { get; }
        private SpriteFont Font { get; set; }

        public NextLevelState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, SpriteBatch spriteBatch)
      : base(game, graphicsDevice, content, spriteBatch) //menu state ima ste te parametra od svojega starsa
        {
           
            var buttonTexture = _content.Load<Texture2D>("menu/button2");
            Font = GameData.Fonts["ThaleahFat_Normal"];
            TitleFont = GameData.Fonts["ThaleahFat_Title"];
            this.ScreenWidth = 800;
            this.ScreenHeight = 480;


#if DESKTOP
            if (_graphicsDevice.Viewport.Width != ScreenWidth || _graphicsDevice.Viewport.Height != ScreenHeight)
            {
                _game.ChangeScreenSize(ScreenWidth, ScreenHeight);
            }
#endif
            


            var playAgainButton = new Button(buttonTexture, Font, new Vector2(ScreenWidth / 2, 290))
            {
                //Position = new Vector2(ScreenWidth / 2, 290),
                Text = "Next Level",
            };
            playAgainButton.Click += Button_Play_Again_Click;


            var mainMenuButton = new Button(buttonTexture, Font, new Vector2(ScreenWidth / 2, 390))
            {
                //Position = new Vector2(ScreenWidth / 2, 390),
                Text = "Main Menu",
            };

            mainMenuButton.Click += Button_Main_Menu_Click;

            components = new List<Component>()
            {
                playAgainButton,
                mainMenuButton
            };
        }
        private void Button_Play_Again_Click(object sender, EventArgs e)
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
            _spriteBatch.Begin(transformMatrix:GameData.MenuScaleMatrix);
            SupportingFunctions.DrawBackground(GraphicsDictionary, _spriteBatch, ScreenWidth, ScreenHeight);
            var x = (ScreenWidth / 2) - (TitleFont.MeasureString("NEXT LEVEL").X / 2);
            _spriteBatch.DrawString(TitleFont, "NEXT LEVEL", new Vector2(x, 200), Color.Black);

            foreach (var component in components)
                component.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
        }
    }
}
