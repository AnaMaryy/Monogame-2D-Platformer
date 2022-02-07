using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Platformer.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Platformer.States;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Platformer
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //private Level level1;
        //track states
        private State _currentState;
        private State _nextState;

        public Action<object, object> GameFinished { get; internal set; }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content"; 
        }

        protected override void Initialize()
        {
            Window.Title = "Charlie's Adventures";

#if DEKSTOP
            _graphics.PreferredBackBufferWidth =GameData.InitialScreenWidth;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = GameData.InitialScreenHeight;   // set this value to the desired height of your window
            _graphics.IsFullScreen = false;
#elif ANDROID
            GameData.AndroidScreenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            GameData.AndroidScreenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //_graphics.PreferredBackBufferWidth =GameData.AndroidScreenWidth;
            //_graphics.PreferredBackBufferHeight = GameData.AndroidScreenHeight;
            Trace.WriteLine("X :" + _graphics.GraphicsDevice.Viewport.Width + " Y " + _graphics.GraphicsDevice.Viewport.Height);
            //scale matrixes
            var scale_wid = (float)GameData.AndroidScreenWidth / GameData.InitialScreenWidth;
            var scale_hei = (float)GameData.AndroidScreenHeight / GameData.InitialScreenHeight;
            GameData.MenuScaleMatrix = Matrix.CreateScale(scale_wid, scale_hei, 1.0f);

             var scale_width = (float)GameData.AndroidScreenWidth / GameData.LevelScreenWidth;
            var scale_height = (float)GameData.AndroidScreenHeight / GameData.LevelScreenHeight;
            GameData.LevelScaleMatrix = Matrix.CreateScale(scale_width, scale_height, 1.0f);

            //for full screen
            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Trace.WriteLine("XX :" + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width + " YY " + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

            _graphics.SupportedOrientations = DisplayOrientation.LandscapeRight;//if your game does NOT support for anything else but portrait mode
            _graphics.ApplyChanges();

#endif

            _graphics.ApplyChanges();

            IsMouseVisible = true;

            base.Initialize();
                       
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            
            //loads the settings and playerstats
            if (PlayerStats.SaveExist())
            {
                PlayerStats.Load();
            }


           
            //load all sound effects
            GameData.SoundEffects = new Dictionary<string, SoundEffect>();
#if DESKTOP
            GameData.SoundEffects.Add("jump", Content.Load<SoundEffect>("../../../../Content/sound/jump"));
            GameData.SoundEffects.Add("bone", Content.Load<SoundEffect>("../../../../Content/sound/bone"));
            GameData.SoundEffects.Add("goldCoin", Content.Load<SoundEffect>("../../../../Content/sound/goldCoin"));
            GameData.SoundEffects.Add("silverCoin", Content.Load<SoundEffect>("../../../../Content/sound/silverCoin"));
            GameData.SoundEffects.Add("stomp", Content.Load<SoundEffect>("../../../../Content/sound/stomp"));
            GameData.SoundEffects.Add("hit", Content.Load<SoundEffect>("../../../../Content/sound/hit"));
            GameData.SoundEffects.Add("win", Content.Load<SoundEffect>("../../../../Content/sound/win"));
            GameData.SoundEffects.Add("heart", Content.Load<SoundEffect>("../../../../Content/sound/heart"));
                        GameData.SoundEffects.Add("lose", Content.Load<SoundEffect>("../../../../Content/sound/lose"));

#elif ANDROID
            GameData.SoundEffects.Add("jump", Content.Load<SoundEffect>("sound/jump2"));
            GameData.SoundEffects.Add("bone", Content.Load<SoundEffect>("sound/bone"));
            GameData.SoundEffects.Add("goldCoin", Content.Load<SoundEffect>("sound/goldCoin"));
            GameData.SoundEffects.Add("silverCoin", Content.Load<SoundEffect>("sound/silverCoin"));
            GameData.SoundEffects.Add("stomp", Content.Load<SoundEffect>("sound/stomp"));
            GameData.SoundEffects.Add("hit", Content.Load<SoundEffect>("sound/hit"));
            GameData.SoundEffects.Add("win", Content.Load<SoundEffect>("sound/win"));
            GameData.SoundEffects.Add("heart", Content.Load<SoundEffect>("sound/heart"));
            GameData.SoundEffects.Add("lose", Content.Load<SoundEffect>("sound/lose"));





#endif
            //load all songs
            GameData.Songs = new Dictionary<string, Song>();
            GameData.Songs.Add("0", Content.Load<Song>("sound/africanSong"));

            //loads all image sprites
            GameData.ImageSprites = new Dictionary<string, Texture2D>();
            GameData.ImageSprites.Add("terrain", Content.Load<Texture2D>("game/env/final_tileset"));
            GameData.ImageSprites.Add("constraint", Content.Load<Texture2D>("game/env/constraint"));
            GameData.ImageSprites.Add("grass", Content.Load<Texture2D>("game/decoration/grass/grass"));
            GameData.ImageSprites.Add("leavesPurple", Content.Load<Texture2D>("game/decoration/leaves/leavesPurple"));
            GameData.ImageSprites.Add("crate", Content.Load<Texture2D>("game/env/crate"));
            GameData.ImageSprites.Add("singleHeart", Content.Load<Texture2D>("game/hearts/heart1"));
            GameData.ImageSprites.Add("backgroundClouds", Content.Load<Texture2D>("game/background/cloudBackground800"));
            GameData.ImageSprites.Add("backgroundSky", Content.Load<Texture2D>("game/background/backgroundTop"));
            GameData.ImageSprites.Add("backgroundBottom", Content.Load<Texture2D>("game/background/backgroundBottom2"));
            GameData.ImageSprites.Add("waterFront", Content.Load<Texture2D>("game/background/waterFront"));
            GameData.ImageSprites.Add("buttonUp", Content.Load<Texture2D>("androidGui/up"));
            GameData.ImageSprites.Add("buttonDown", Content.Load<Texture2D>("androidGui/down"));
            GameData.ImageSprites.Add("buttonLeft", Content.Load<Texture2D>("androidGui/left"));
            GameData.ImageSprites.Add("buttonRight", Content.Load<Texture2D>("androidGui/right"));
            GameData.ImageSprites.Add("instructionBone", Content.Load<Texture2D>("game/instructions/jumpBoardSniff"));
            GameData.ImageSprites.Add("instructionMove", Content.Load<Texture2D>("game/instructions/moveBoard"));
            GameData.ImageSprites.Add("instructionJump", Content.Load<Texture2D>("game/instructions/jumpBoardSpacebar1"));
            GameData.ImageSprites.Add("bubbleFail", Content.Load<Texture2D>("game/human/bubble/collectBubble"));
            GameData.ImageSprites.Add("bubbleWin", Content.Load<Texture2D>("game/human/bubble/winBubble"));
            GameData.ImageSprites.Add("death", Content.Load<Texture2D>("game/dog/death"));

            //load fonts
            GameData.Fonts = new Dictionary<string, SpriteFont>();
            GameData.Fonts.Add("ThaleahFat_Title", Content.Load<SpriteFont>("font/ThaleahFat_Title"));
            GameData.Fonts.Add("ThaleahFat_Normal", Content.Load<SpriteFont>("font/ThaleahFat_Normal"));

            //load all animated image sprites

            GameData.AnimatedSprites = new Dictionary<string, List<Texture2D>>();
#if DESKTOP
            foreach (var item in GameData.NamesAnimatedSprites)
            {
                GameData.AnimatedSprites.Add(item.Key,  SupportingFunctions.ImportFolder("game/" +item.Key, "../../../../Platformer/Content/game/" +item.Key, Content));

            }

#elif ANDROID

            foreach (var item in GameData.NamesAnimatedSprites)
            {
                GameData.AnimatedSprites.Add(item.Key, SupportingFunctions.ImportFolder(item.Key,item.Value, Content));

            }

#endif

            //start in the menu state
            _currentState = new MenuState(this, GraphicsDevice, Content, _spriteBatch);
            _currentState.LoadContent();
            _nextState = null;
        }

        protected override void Update(GameTime gameTime)
        {
            
            if(_nextState != null)
            {
                _currentState = _nextState;
                _currentState.LoadContent();

                _nextState = null;
            }
            _currentState.Update(gameTime);
            
            
            base.Update(gameTime);
        }
        
        public void ChangeState(State state)
        {
            _nextState = state;
        }
        public void ChangeScreenSize(int width, int height)
        {
            _graphics.PreferredBackBufferWidth = width;  
            _graphics.PreferredBackBufferHeight = height;   
            _graphics.ApplyChanges();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _currentState.Draw(gameTime);
           
            base.Draw(gameTime);
        }

    }
}
