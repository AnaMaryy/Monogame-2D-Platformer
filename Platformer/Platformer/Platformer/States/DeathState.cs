using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        private SpriteFont Font { get; set; }

        public DeathState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, SpriteBatch spriteBatch)
      : base(game, graphicsDevice, content, spriteBatch) //menu state ima ste te parametra od svojega starsa
        {
            this.ScreenWidth = 800;
            this.ScreenHeight = 480;


#if DESKTOP
            if (_graphicsDevice.Viewport.Width != ScreenWidth || _graphicsDevice.Viewport.Height != ScreenHeight)
            {
                _game.ChangeScreenSize(ScreenWidth, ScreenHeight);
            }
#endif

            var buttonTexture = GameData.ImageSprites["button3"];
            Font = GameData.Fonts["ThaleahFat_Normal"];
            TitleFont = GameData.Fonts["ThaleahFat_Title"];
            //change width and height if values differ
          


            var tryAgainButton = new Button(buttonTexture, Font, new Vector2(ScreenWidth / 2,ScreenHeight/8*3+20),null)
            {
               // Position = new Vector2(ScreenWidth / 2, 290),
                Text = "Try Again",
            };
            tryAgainButton.Click += Button_Try_Again_Click;


            var mainMenuButton = new Button(buttonTexture, Font, new Vector2(ScreenWidth / 2, ScreenHeight / 8 * 4 +40), null)
            {
               // Position = new Vector2(ScreenWidth / 2, 390),
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.Exit();
            foreach (var component in components)
                component.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
#if DESKTOP
            _spriteBatch.Begin();
#elif ANDROID
            _spriteBatch.Begin(transformMatrix: GameData.MenuScaleMatrix);

#endif
            SupportingFunctions.DrawBackground(GraphicsDictionary, _spriteBatch, ScreenWidth, ScreenHeight);
            var x = (ScreenWidth / 2) - (TitleFont.MeasureString("GAME OVER :(").X / 2);
            _spriteBatch.DrawString(TitleFont, "GAME OVER :(", new Vector2(x, ScreenHeight/8*2), Color.Black);

            foreach (var component in components)
                component.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
        }
    }
}
