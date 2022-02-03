
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using Platformer.Controls;
using Platformer.Utilities;

namespace Platformer.States
{
    public class MenuState : State
    {
        private List<Component> components;

        private Texture2D coverTexture;

        private Dictionary<string, Texture2D> GraphicsDictionary;

        public int ScreenWidth { get; }
        public int ScreenHeight { get; }

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, SpriteBatch spriteBatch)
      : base(game, graphicsDevice, content, spriteBatch) //menu state ima ste te parametra od svojega starsa
        {
            var buttonTexture = _content.Load<Texture2D>("menu/button2");
            var buttonFont = _content.Load<SpriteFont>("font/ThaleahFat_Normal");


            this.ScreenWidth = 800;
            this.ScreenHeight = 480;


#if DESKTOP
            if (_graphicsDevice.Viewport.Width != ScreenWidth || _graphicsDevice.Viewport.Height != ScreenHeight)
            {
                _game.ChangeScreenSize(ScreenWidth, ScreenHeight);
            }
#endif
            var playGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(ScreenWidth / 2, 200),
                Text = SwitchPlayButtonName(),
            };

            playGameButton.Click += Button_PlayGame_Click;

            var settingsButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(ScreenWidth / 2, 300),
                Text = "Settings",
            };

            settingsButton.Click += Button_Settings_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(ScreenWidth / 2, 400),
                Text = "Quit Game",
            };

            quitGameButton.Click += Button_Quit_Clicked;

            components = new List<Component>()
            {
                playGameButton,
                settingsButton,
                quitGameButton,
            };
        }
        private string SwitchPlayButtonName()
        {
            switch (PlayerStats.CompletedLevels)
            {
                case -1:
                    return "Play";
                    break;
                case 0:
                    return "Play Level 2";
                    break;
                default: // means that we restarted the game from the beginning basically, restart the counter too
                    PlayerStats.CompletedLevels = -1;
                    PlayerStats.Save();
                    return "New Game";
                    break;
            }
        }
        public override void LoadContent()
        {
            GraphicsDictionary = SupportingFunctions.LoadBackground(_content);
            coverTexture = _content.Load<Texture2D>("menu/cover");
        }

        private void Button_PlayGame_Click(object sender, EventArgs e)
        {
            //_game.ChangeState(new OverworldState(_game, _graphicsDevice, _content));
            _game.ChangeState(new LevelState(_game, _graphicsDevice, _content, _spriteBatch));
        }
        private void Button_Settings_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new SettingsState(_game, _graphicsDevice, _content, _spriteBatch));
        }
        private void Button_Quit_Clicked(object sender, EventArgs args)
        {
#if DESKTOP
            _game.Exit();
#elif ANDROID
            //TODO : entirely kill the process
            _game.GameFinished(sender, args);
#endif
        }

        public override void Update(GameTime gameTime)
        {

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
            //draw the background
            SupportingFunctions.DrawBackground(GraphicsDictionary, _spriteBatch, ScreenWidth, ScreenHeight);
            _spriteBatch.Draw(coverTexture, new Vector2(ScreenWidth / 2 + 20, 100), null, Color.White, 0f, new Vector2(coverTexture.Width / 2, coverTexture.Height / 2), 1f, SpriteEffects.None, 0f);

            foreach (var component in components)
                component.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();
        }
    }
}
