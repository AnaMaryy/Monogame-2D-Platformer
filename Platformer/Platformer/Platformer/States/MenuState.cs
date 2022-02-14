
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
using Platformer.Models;
using Platformer.Sprites;
using Platformer.Utilities;

namespace Platformer.States
{
    public class MenuState : State
    {
        private List<Component> components;
        private List<Tile> Tiles;
        private List<PalmTreeTile> PalmTiles;


        private Texture2D coverTexture;

        private Dictionary<string, Texture2D> GraphicsDictionary;

        public int ScreenWidth { get; }
        public int ScreenHeight { get; }
        public SpriteFont TitleFont { get; private set; }
        public SpriteFont Font { get; set; }
        public Color Green { get; set; }
        public Color Brown { get; set; }

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, SpriteBatch spriteBatch)
      : base(game, graphicsDevice, content, spriteBatch) //menu state ima ste te parametra od svojega starsa
        {
            TitleFont = GameData.Fonts["ThaleahFat_Title"];
            Font = GameData.Fonts["ThaleahFat_Normal"];

            Brown = new Color(168,88,21);
            Green = new Color(161,195,60);


            this.ScreenWidth = 800;
            this.ScreenHeight = 480;


#if DESKTOP
            if (_graphicsDevice.Viewport.Width != ScreenWidth || _graphicsDevice.Viewport.Height != ScreenHeight)
            {
                _game.ChangeScreenSize(ScreenWidth, ScreenHeight);
            }
#endif
            var playGameButton = new Button(GameData.ImageSprites["playButton"], Font, new Vector2(ScreenWidth / 2, ScreenHeight / 2), 0.65f) ;
            playGameButton.Click += Button_PlayGame_Click;

            var settingsButton = new Button(GameData.ImageSprites["settingsButton"],Font, new Vector2(ScreenWidth / 8*7, ScreenHeight / 8), 0.5f);
            settingsButton.Click += Button_Settings_Click;

            var highscoreButton = new Button(GameData.ImageSprites["highscoreButton"], Font, new Vector2(ScreenWidth / 8, ScreenHeight / 8), 0.5f);
            highscoreButton.Click += Button_Highscore_Click;

            var creditsButton = new Button(GameData.ImageSprites["creditsButton"],Font, new Vector2(ScreenWidth / 8, ScreenHeight / 8*2+10), 0.5f);
            creditsButton.Click += Button_Credits_Click;

            var quitGameButton = new Button(GameData.ImageSprites["exitButton"],Font, new Vector2(ScreenWidth / 8, ScreenHeight / 8*3+20), 0.5f);
            quitGameButton.Click += Button_Quit_Clicked;

            components = new List<Component>()
            {
                playGameButton,
                settingsButton,
                highscoreButton,
                creditsButton,
                quitGameButton,
            };

            BackgroundDecoration();
           

        }

        private void BackgroundDecoration() //tiles for the background :D
        {
            Tiles = new List<Tile>();
            PalmTiles = new List<PalmTreeTile>();

            //first the floor -> calculate how many tiles we need
            int tilesNeeded = ScreenWidth / GameData.TileSize + 1;
            var x = 0;
            Random rand = new Random();
            for(int i = 0; i < tilesNeeded; ++i)
            {

                Rectangle drawRectangle = new Rectangle(rand.Next(0,5)* GameData.TileSize, GameData.TileSize, GameData.TileSize, GameData.TileSize);
                Tiles.Add(new Tile(GameData.ImageSprites["terrain"], new Vector2(x, ScreenHeight / 8 * 7), drawRectangle, null));
                x += GameData.TileSize;
            }
           
            var palmY = ScreenHeight / 8 * 7 - GameData.TileSize;
            //crate
            Tiles.Add(new CrateTile(GameData.ImageSprites["crate"], new Vector2(ScreenWidth / 8 * 2, palmY), null, null));
            //palms
            PalmTiles.Add(new PalmTreeTile(GameData.AnimatedSprites["env/palm_small"][0], new Vector2(ScreenWidth / 8 * 1,palmY),null, GameData.AnimatedSprites["env/palm_small"],38, null));
            PalmTiles.Add( new PalmTreeTile(GameData.AnimatedSprites["env/palm_small"][0], new Vector2(ScreenWidth / 8 * 6, palmY), null, GameData.AnimatedSprites["env/palm_small"], 38, null));
            PalmTiles.Add(new PalmTreeTile(GameData.AnimatedSprites["env/palm_large"][0], new Vector2(ScreenWidth / 8 * 7, palmY), null, GameData.AnimatedSprites["env/palm_large"], 70, null));
           
            //enemies
            //enemies are palmTreeTiles, because of the offset
             PalmTiles.Add(new PalmTreeTile(GameData.AnimatedSprites["enemies/enemyBoss/idle"][0], new Vector2((int)(ScreenWidth / 8 * 1.5), palmY), null, GameData.AnimatedSprites["enemies/enemyBoss/idle"], 0, null));

        }
        private string SwitchPlayButtonName()
        {
            switch (PlayerStats.CompletedLevels)
            {
                case -1:
                    return "Level 1";
                case 0:
                    return "Level 2";
                default: // means that we restarted the game from the beginning basically, restart the counter too
                    PlayerStats.CompletedLevels = -1;
                    PlayerStats.Save();
                    return "New Game";
            }
        }
        public override void LoadContent()
        {
            GraphicsDictionary = SupportingFunctions.LoadBackground(_content);
            coverTexture = GameData.ImageSprites["cover2"];
        }

        private void Button_PlayGame_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new LevelState(_game, _graphicsDevice, _content, _spriteBatch));
        }
        private void Button_Settings_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new SettingsState(_game, _graphicsDevice, _content, _spriteBatch));
        }
        private void Button_Highscore_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new HighscoreState(_game, _graphicsDevice, _content, _spriteBatch));
        }
        private void Button_Credits_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new CreditsState(_game, _graphicsDevice, _content, _spriteBatch));
        }
        private void Button_Quit_Clicked(object sender, EventArgs args)
        {
            _game.Exit();

        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.Exit();    
            //update the tiles
            foreach (var tile in PalmTiles)
            {
                tile.Update();
            }
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
            //draw the tiles
            foreach(var tile in Tiles)
            {
                tile.Draw(_spriteBatch);
            }
            foreach (var tile in PalmTiles)
            {
                tile.Draw(_spriteBatch);
            }
            // draw the text
            string levelText = SwitchPlayButtonName();
            Button playButton = (Button)components[0];
            //var x = ScreenWidth / 2 - (Font.MeasureString(levelText).X / 2);
            //var y = playButton.Rectangle.Y + playButton.Rectangle.Height + 35;
            var x = ScreenWidth / 2 - (TitleFont.MeasureString(levelText).X / 2);
            var y = ScreenHeight / 2 + playButton.Rectangle.Height*0.45f;
            _spriteBatch.DrawString(TitleFont, levelText, new Vector2(x, y), Brown, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);

            //draw the buttons
            foreach (var component in components)
                component.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();
        }
    }
}
