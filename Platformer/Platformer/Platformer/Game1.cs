using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Platformer.Utilities;
using System.Collections.Generic;
using System.Diagnostics;

namespace Platformer
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //private Level level1;
        //track states
        //private State _currentState;
        //private State _nextState;

       

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //for some reason resize window + title  needs to be here :) love them bugs 
            Window.Title = "Charlie's Adventures";
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth =GameData.InitialScreenWidth;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = GameData.InitialScreenHeight;   // set this value to the desired height of your window
            Trace.WriteLine(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            Trace.WriteLine(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);
            _graphics.ApplyChanges();

            IsMouseVisible = true;

            base.Initialize();
                       
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            /*
            //loads the settings and playerstats
            if (PlayerStats.SaveExist())
            {
                PlayerStats.Load();
            }


            //start in the menu state
            _currentState = new MenuState(this, GraphicsDevice, Content, _spriteBatch);
            _currentState.LoadContent();
            _nextState = null;
            */
            //loads all image sprites

            GameData.ImageSprites = new Dictionary<string, Texture2D>();
            GameData.ImageSprites.Add("terrain", Content.Load<Texture2D>("game/env/final_tileset"));


            GameData.ImageSprites.Add("constraint", Content.Load<Texture2D>("game/env/constraint"));
            GameData.ImageSprites.Add("grass", Content.Load<Texture2D>("game/decoration/grass/grass"));
            GameData.ImageSprites.Add("leavesPurple", Content.Load<Texture2D>("game/decoration/leaves/leavesPurple"));
            GameData.ImageSprites.Add("crate", Content.Load<Texture2D>("game/env/crate"));
            GameData.ImageSprites.Add("singleHeart", Content.Load<Texture2D>("game/hearts/heart1"));
            GameData.ImageSprites.Add("backgroundClouds", Content.Load<Texture2D>("game/background/cloudBackground800"));

            GameData.AnimatedSprites = new Dictionary<string, List<Texture2D>>();
#if DESKTOP
            GameData.AnimatedSprites.Add("enemies/enemyHorizontal/run", SupportingFunctions.ImportFolder("game/enemies/enemyHorizontal/run", "../../../../Platformer/Content/game/enemies/enemyHorizontal/run", Content));
            GameData.AnimatedSprites.Add("enemies/enemyVertical/jump", SupportingFunctions.ImportFolder("game/enemies/enemyVertical/jump", "../../../../Platformer/Content/game/enemies/enemyVertical/jump", Content));
            GameData.AnimatedSprites.Add("enemies/enemyBoss/idle", SupportingFunctions.ImportFolder("game/enemies/enemyBoss/idle", "../../../../Platformer/Content/game/enemies/enemyBoss/idle", Content));
            GameData.AnimatedSprites.Add("coins/gold", SupportingFunctions.ImportFolder("game/coins/gold", "../../../../Platformer/Content/game/coins/gold", Content));
            GameData.AnimatedSprites.Add("coins/silver", SupportingFunctions.ImportFolder("game/coins/silver", "../../../../Platformer/Content/game/coins/silver", Content));
            GameData.AnimatedSprites.Add("env/palm_small", SupportingFunctions.ImportFolder("game/env/palm_small", "../../../../Platformer/Content/game/env/palm_small", Content));
            GameData.AnimatedSprites.Add("env/palm_large", SupportingFunctions.ImportFolder("game/env/palm_large", "../../../../Platformer/Content/game/env/palm_large", Content));
            GameData.AnimatedSprites.Add("env/palm_bg", SupportingFunctions.ImportFolder("game/env/palm_bg", "../../../../Platformer/Content/game/env/palm_bg", Content));
            GameData.AnimatedSprites.Add("bone", SupportingFunctions.ImportFolder("game/bone", "../../../../Platformer/Content/game/bone", Content));
            GameData.AnimatedSprites.Add("hearts", SupportingFunctions.ImportFolder("game/hearts", "../../../../Platformer/Content/game/hearts", Content));
            GameData.AnimatedSprites.Add("effects/explosion", SupportingFunctions.ImportFolder("game/effects/explosion", "../../../../Platformer/Content/game/effects/explosion", Content));
            GameData.AnimatedSprites.Add("human/idle", SupportingFunctions.ImportFolder("game/human/idle", "../../../../Platformer/Content/game/human/idle", Content));
#elif ANDROID
            foreach (var item in GameData.AndroidAnimatedSprites)
            {
                GameData.AnimatedSprites.Add(item.Key, SupportingFunctions.ImportFolder(item.Key,item.Value, Content));

            }

#endif

        }

        protected override void Update(GameTime gameTime)
        {
            /*
            // TODO: Add your update logic here
            if(_nextState != null)
            {
                _currentState = _nextState;
                _currentState.LoadContent();

                _nextState = null;
            }
            _currentState.Update(gameTime);
            
            */
            base.Update(gameTime);
        }
        /*
        public void ChangeState(State state)
        {
            _nextState = state;
        }
        public void ChangeScreenSize(int width, int height)
        {
            _graphics.PreferredBackBufferWidth = width;  
            _graphics.PreferredBackBufferHeight = height;   
            _graphics.ApplyChanges();
        }*/

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // _currentState.Draw(gameTime);
            _spriteBatch.Begin();
            _spriteBatch.Draw(GameData.AnimatedSprites["enemies/enemyHorizontal/run"][0], new Vector2(20, 02), Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
