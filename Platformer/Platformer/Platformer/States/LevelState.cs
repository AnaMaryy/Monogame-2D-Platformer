using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame;
using Platformer.Controls;
using Platformer.Models;
using Platformer.Utilities;

namespace Platformer.States
{
    public class LevelState : State
    {
        private Level Level { get; set; }
        private int ScreenWidth { get; set; }
        private int ScreenHeight { get; set; }

        public LevelState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, SpriteBatch spriteBatch)
      : base(game, graphicsDevice, content, spriteBatch) //menu state ima ste te parametra od svojega starsa; _game,_graphicsDevice,_content
        {
            // decide which level to create -> so here for now we will create level 1

            //resize the screen to the level 
#if DESKTOP
            this.ScreenWidth = GameData.LevelScreenWidth;
            this.ScreenHeight = GameData.LevelScreenHeight;
#elif ANDROID
            this.ScreenWidth = GameData.AndroidScreenWidth;
            this.ScreenHeight = GameData.AndroidScreenHeight;
#endif

            if (_graphicsDevice.Viewport.Width != ScreenWidth || _graphicsDevice.Viewport.Height != ScreenHeight)
            {
                _game.ChangeScreenSize(ScreenWidth, ScreenHeight);
            }
        }
        private Dictionary<string, string> SwitchLevel() //switch between levels
        {
            switch (PlayerStats.CompletedLevels)
            {
                case -1:
                    return GameData.level_0;
                    break;
                case 0:
                    return GameData.level_1;
                    break;
                default: // means that we restarted the game from the beginning basically, restart the counter too
                    PlayerStats.CompletedLevels = -1;
                    PlayerStats.Save();
                    return GameData.level_0;
                    break;
            }
        }
        public override void LoadContent()
        {
            //create the level
            var levelData = SwitchLevel();
            Level = new Level(_game, _spriteBatch, _graphicsDevice, _content, levelData); 
        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                //stop playing music and go to main menu
                MediaPlayer.Stop();
                _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, _spriteBatch));

            }
            Level.Update();


        }

        public override void Draw(GameTime gameTime)
        {

            Level.Draw();
        }
    }
}
