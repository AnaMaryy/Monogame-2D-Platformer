using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class SettingsState : State
    {
        private Dictionary<string, Texture2D> GraphicsDictionary;
        private List<Component> components;

        public SpriteFont TitleFont { get; private set; }
        public int ScreenWidth { get; }
        public int ScreenHeight { get; }
        private SpriteFont Font { get; set; }

        public SettingsState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, SpriteBatch spriteBatch)
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

            var buttonTexture = _content.Load<Texture2D>("menu/button2");
            Font = _content.Load<SpriteFont>("font/ThaleahFat_Normal");
            TitleFont = _content.Load<SpriteFont>("font/ThaleahFat_Title");
            
            var volumeSliderTexture = _content.Load<Texture2D>("settings/VolumeSlider");
            var volumeBallTexture = _content.Load<Texture2D>("settings/VolumeBall");


            var musicVolumeSlider = new Slider(volumeSliderTexture, volumeBallTexture, new Vector2(ScreenWidth / 2, 190), PlayerStats.MusicVolume)
            {
                //Position = new Vector2(ScreenWidth / 2, 190),
                //Value = GameData.MusicVolume

            };

            musicVolumeSlider.Click += Slider_MusicVolume_Click;

            var soundEffectsVolumeSlider = new Slider(volumeSliderTexture, volumeBallTexture, new Vector2(ScreenWidth / 2, 290), PlayerStats.SoundEffectsVolume)
            {
                //Position = new Vector2(ScreenWidth / 2, 290),
                //Value = GameData.SoundEffectsVolume
            };

            soundEffectsVolumeSlider.Click += Slider_SoundEffectsVolume_Click;

            var backButton = new Button(buttonTexture, Font)
            {
                Position = new Vector2(ScreenWidth / 2, 380),
                Text = "Back",
            };

            backButton.Click += Button_Back_Click;

            components = new List<Component>()
            {
                musicVolumeSlider,
                soundEffectsVolumeSlider,
                backButton
            };
        }
        private void Slider_MusicVolume_Click(object sender, EventArgs e)
        {
            Slider slider = sender as Slider;
            if (slider != null)
            {
                float volume = slider.Value;
                //PlayerStats.MusicVolume = volume;
                //PlayerStats.Save();
            }
        }

        private void Slider_SoundEffectsVolume_Click(object sender, EventArgs e)
        {
            Slider slider = sender as Slider;
            if (slider != null)
            {
                float volume = slider.Value;
                //PlayerStats.SoundEffectsVolume = volume;
                //PlayerStats.Save();
            }
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



#if DESKTOP
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, _spriteBatch));
            foreach(var component in components)
                component.Update(gameTime);
#elif ANDROID
            Timer();
            if (!Start)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, _spriteBatch));
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
            var x = (ScreenWidth / 2) - (TitleFont.MeasureString("SETTINGS").X / 2);
            _spriteBatch.DrawString(TitleFont, "SETTINGS", new Vector2(x, 60), Color.Black);

            var x1 = (ScreenWidth / 2) - (Font.MeasureString("MUSIC VOLUME").X / 2);
            _spriteBatch.DrawString(Font, "MUSIC VOLUME", new Vector2(x1, 140), Color.Black);

            var x2 = (ScreenWidth / 2) - (Font.MeasureString("SOUND EFFECTS VOLUME").X / 2);
            _spriteBatch.DrawString(Font, "SOUND EFFECTS VOLUME", new Vector2(x2, 240), Color.Black);


            foreach (var component in components)
                component.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
        }
    }
}
