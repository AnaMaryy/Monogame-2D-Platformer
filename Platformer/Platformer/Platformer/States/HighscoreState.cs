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
    class HighscoreState : State
    {
        private Dictionary<string, Texture2D> GraphicsDictionary;
        private List<Component> components;

        public SpriteFont TitleFont { get; private set; }
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        private SpriteFont Font { get; set; }

        public HighscoreState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, SpriteBatch spriteBatch) : base(game, graphicsDevice, content, spriteBatch) //menu state ima ste te parametra od svojega starsa
        {
            this.ScreenWidth = 800;
            this.ScreenHeight = 480;

            Font = GameData.Fonts["ThaleahFat_Normal"];
            TitleFont = GameData.Fonts["ThaleahFat_Title"];
#if DESKTOP
            if (_graphicsDevice.Viewport.Width != ScreenWidth || _graphicsDevice.Viewport.Height != ScreenHeight)
            {
                _game.ChangeScreenSize(ScreenWidth, ScreenHeight);
            }
#endif
            var backButton = new Button(GameData.ImageSprites["button3"], Font, new Vector2(ScreenWidth / 2, 380), null)
            {
                Text = "Back",
            };

            backButton.Click += Button_Back_Click;

            components = new List<Component>()
            {

                backButton
            };
        }

        private void Button_Back_Click(object sender, EventArgs e)
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
                _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, _spriteBatch));

#if DESKTOP
          
            foreach(var component in components)
                component.Update(gameTime);
#elif ANDROID
            Timer();
            if (!Start)
            {

                foreach (var component in components)
                    component.Update(gameTime);
            }
#endif
        }


        public override void Draw(GameTime gameTime)
        {
#if DESKTOP
            _spriteBatch.Begin();
#elif ANDROID
            _spriteBatch.Begin(transformMatrix: GameData.MenuScaleMatrix);

#endif
            SupportingFunctions.DrawBackground(GraphicsDictionary, _spriteBatch, ScreenWidth, ScreenHeight);
            var x = (ScreenWidth / 2) - (TitleFont.MeasureString("HIGHSCORES").X / 2);
            _spriteBatch.DrawString(TitleFont, "HIGHSCORES", new Vector2(x, ScreenHeight / 8 * 0.5f), Color.Black);

            
            if (PlayerStats.HighScores == null ||PlayerStats.HighScores.Count == 0 )
            {
                var x1 = (ScreenWidth / 2) - (Font.MeasureString("Nothing to show yet").X / 2);
                _spriteBatch.DrawString(Font, "Nothing to show yet", new Vector2(x1, ScreenHeight / 8 * 1.5f), Color.Gray);
            }
            else
            {
                float y_grid = 1.5f;
                int count = 0;
                foreach (var item in PlayerStats.HighScores)
                {
                
                    string level = "level " + (Int32.Parse(item.Key) + 2);
                    string content = "Coins: " + item.Value[0] + " | Time: " + item.Value[1] + " seconds";
                    var y_level = ScreenHeight / 8 * y_grid;
                    var x_level = (ScreenWidth / 2) - (Font.MeasureString(level).X / 2);
                    var x_content = (ScreenWidth / 2) - (Font.MeasureString(content).X / 2);
                    var y_content = ScreenHeight / 8 * y_grid + Font.MeasureString(level).Y + 5;

                    _spriteBatch.DrawString(Font, level, new Vector2(x_level, y_level), Color.Black);
                    _spriteBatch.DrawString(Font, content, new Vector2(x_content, y_content), Color.Gray, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);

                    y_grid += 1.0f;
                }
            }



            foreach (var component in components)
                component.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
        }
    }
}
