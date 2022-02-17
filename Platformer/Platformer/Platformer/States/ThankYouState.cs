using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Platformer.Controls;
using Platformer.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Platformer.States
{
    public class ThankYouState : State
    {
        private Dictionary<string, Texture2D> GraphicsDictionary;
        private List<Component> components;

        public SpriteFont TitleFont { get; private set; }
        public int ScreenWidth { get; }
        public int ScreenHeight { get; }
        private SpriteFont Font { get; set; }

        public ThankYouState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, SpriteBatch spriteBatch)
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
            var playAgainButton = new Button(buttonTexture, Font, new Vector2(ScreenWidth / 2, ScreenHeight / 8 * 4 + 20), null)
            {
               // Position = new Vector2(ScreenWidth / 2, 290),
                Text = "Play Again",
            };
            playAgainButton.Click += Button_Play_Again_Click;


            var mainMenuButton = new Button(buttonTexture, Font, new Vector2(ScreenWidth / 2, ScreenHeight / 8 * 5 + 40), null)
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, _spriteBatch));
            foreach (var component in components)
                component.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
#if DESKTOP
            _spriteBatch.Begin();
#elif ANDROID
            _spriteBatch.Begin(transformMatrix:GameData.MenuScaleMatrix);
#endif
            SupportingFunctions.DrawBackground(GraphicsDictionary, _spriteBatch, ScreenWidth, ScreenHeight);
            
            var x = (ScreenWidth / 2) - (TitleFont.MeasureString("Thanks for playing!").X / 2);
            var y = ScreenHeight / 8 * 2;
            _spriteBatch.DrawString(TitleFont, "Thanks for playing!", new Vector2(x, y), Color.Black);
            var x1 = (ScreenWidth / 2) - (Font.MeasureString("Your HighScore:").X / 2);
            //var y1 = y + TitleFont.MeasureString("Bravo!").Y + 5;
            var y1 = ScreenHeight / 8 * 2.5f;


            try
            {
                 _spriteBatch.DrawString(Font, "Your HighScore:", new Vector2(x1, y1), Color.Black);
                var item = PlayerStats.HighScores[SupportingFunctions.getLastHighscoreKey()];
                string content = "Coins: " + item[0] + " | Time: " + item[1] + " seconds";
                var x2 = (ScreenWidth / 2) - (Font.MeasureString(content).X / 2);
                //var y2 = y1 + Font.MeasureString("Your Score:").Y + 5;
                var y2 = ScreenHeight / 8 * 3f;
                _spriteBatch.DrawString(Font, content, new Vector2(x2, y2), Color.Gray);
            }catch(Exception e)
            {
                Trace.WriteLine(e);
            }
           
            
            foreach (var component in components)
                component.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
        }
    }
}
