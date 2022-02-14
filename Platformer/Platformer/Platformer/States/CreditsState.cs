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
    class CreditsState : State
    {
        private Dictionary<string, Texture2D> GraphicsDictionary;
        private List<Component> components;

        public SpriteFont TitleFont { get; private set; }
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        private SpriteFont Font { get; set; }

        public CreditsState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, SpriteBatch spriteBatch) : base(game, graphicsDevice, content, spriteBatch) //menu state ima ste te parametra od svojega starsa
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
            var x = (ScreenWidth / 2) - (TitleFont.MeasureString("CREDITS").X / 2);
            _spriteBatch.DrawString(TitleFont, "CREDITS", new Vector2(x, ScreenHeight/8*0.5f), Color.Black);

            var x1 = (ScreenWidth / 2) - (Font.MeasureString("DESIGN").X / 2);
            _spriteBatch.DrawString(Font, "DESIGN", new Vector2(x1, ScreenHeight / 8 * 1.5f), Color.Black );
            var x11 = (ScreenWidth / 2) - (Font.MeasureString("Amariani Studios").X/2);
            var y11 = ScreenHeight / 8 * 1.5f + Font.MeasureString("DESIGN").Y + 5;
            _spriteBatch.DrawString(Font, "Amariani Studios", new Vector2(x11, y11), Color.Gray, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);

            var x2 = (ScreenWidth / 2) - (Font.MeasureString("PROGRAMMING").X / 2);
            _spriteBatch.DrawString(Font, "PROGRAMMING", new Vector2(x2, ScreenHeight / 8 * 2.5f), Color.Black);
            var x21 = (ScreenWidth / 2) - (Font.MeasureString("Amariani Studios").X / 2);
            var y21 = ScreenHeight / 8 * 2.5f + Font.MeasureString("PROGRAMMING").Y + 5;
            _spriteBatch.DrawString(Font, "Amariani Studios", new Vector2(x21, y21), Color.Gray, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);

            var x3 = (ScreenWidth / 2) - (Font.MeasureString("ART").X / 2);
            _spriteBatch.DrawString(Font, "ART", new Vector2(x3, ScreenHeight / 8 * 3.5f), Color.Black);
            var x31 = (ScreenWidth / 2) - (Font.MeasureString("Amariani Studios, Pixel Frog, Angry Elk, 9E0, Jesse Munguia").X / 2);
            var y31 = ScreenHeight / 8 * 3.5f + Font.MeasureString("ART").Y + 5;
            _spriteBatch.DrawString(Font, "Amariani Studios, Pixel Frog, Angry Elk, 9E0, Jesse Munguia", new Vector2(x31, y31), Color.Gray, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);

            var x4 = (ScreenWidth / 2) - (Font.MeasureString("MUSIC & SOUND").X / 2);
            _spriteBatch.DrawString(Font, "MUSIC & SOUND", new Vector2(x4, ScreenHeight / 8 * 4.5f), Color.Black);
            var x41 = (ScreenWidth / 2) - (Font.MeasureString("Zapsplat.com").X / 2);
            var y41 = ScreenHeight / 8 * 4.5f + Font.MeasureString("Zapsplat.com").Y + 5;
            _spriteBatch.DrawString(Font, "Zapsplat.com", new Vector2(x41, y41), Color.Gray, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);

            foreach (var component in components)
                component.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
        }
    }
}