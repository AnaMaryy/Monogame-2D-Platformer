using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework;
using Platformer.Physics;
using Microsoft.Xna.Framework.Media;
using Platformer.Sprites;
using Platformer.GUI;
using Microsoft.Xna.Framework.Input;
using Platformer.States;
using Microsoft.Xna.Framework.Graphics;
using Platformer.Utilities;

namespace Platformer.Models
{
    public class Level
    {
        private readonly Game1 _game;
        private SpriteBatch _spriteBatch;
        private GraphicsDevice _graphicsDevice;
        private readonly ContentManager _content;





        //# setup the level
      
        private int DeathLine { get; set; }


        private Player Player { get; set; }
        private Camera _camera { get; set; }
        private HumanTile EndLevelTile { get; set; }

        //all tiles
        public List<Tile> TerrainTiles { get; private set; }
        public List<Tile> GrassTiles { get; private set; }
        public List<Tile> LeavesTiles { get; private set; }
        public List<Tile> CratesTiles { get; private set; }
        public List<Tile> CoinsTiles { get; private set; }
        public List<Tile> FgPalmTiles { get; private set; }
        public List<Tile> BgPalmTiles { get; private set; }
        public List<Enemy> Enemies { get; private set; }
        public List<Tile> ConstraintTiles { get; private set; }
        public List<Tile> BoneTiles { get; private set; }
        public List<Tile> HeartTiles { get; private set; }
        public List<OneLoopAnimation> OneLoopAnimations { get; private set; }


        public ScrollingBackground ScrollingBackground { get; set; }
        public Water Water { get; private set; }
        public Gui Gui { get; private set; }

        //game rules
        public int MaxLevel = 2;

        public int Coins = 0;
        public int BonesCurrent = 0;
        public int BonesNeeded { get; set; }

        public Level(Game1 game, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, ContentManager content, Dictionary<string, string> levelData)
        {
            _game = game;
            _spriteBatch = spriteBatch;
            _graphicsDevice = graphicsDevice;
            _content = content;

             _camera = new Camera();
            //todo -> change to screen si
            DeathLine = GameData.VerticalTileNumber * GameData.TileSize;
            //SpritesDict= LoadContent(content);

            //create player and endlevelTile
            var playerLayout = SupportingFunctions.ImportCvsLayout(levelData["player"],_content);
            createPlayer(playerLayout);
            //create the level
            var terrainLayout = SupportingFunctions.ImportCvsLayout(levelData["terrain"], _content);
            TerrainTiles = createTileGroup(terrainLayout, "terrain");
            //grass
            var grassLayout = SupportingFunctions.ImportCvsLayout(levelData["grass"], _content);
            GrassTiles = createTileGroup(grassLayout, "grass");
            //leaves
            var leavesLayout = SupportingFunctions.ImportCvsLayout(levelData["leaves"], _content);
            LeavesTiles = createTileGroup(leavesLayout, "leaves");
            //crates
            var cratesLayout = SupportingFunctions.ImportCvsLayout(levelData["crate"], _content);
            CratesTiles = createTileGroup(cratesLayout, "crate");
            //coins
            var coinsLayout = SupportingFunctions.ImportCvsLayout(levelData["coins"], _content);
            CoinsTiles = createTileGroup(coinsLayout, "coins");
            //foreground palms 
            var fgpalmsLayout = SupportingFunctions.ImportCvsLayout(levelData["fg_palms"], _content);
            FgPalmTiles = createTileGroup(fgpalmsLayout, "fg_palms");
            //foreground palms 
            var bgpalmsLayout = SupportingFunctions.ImportCvsLayout(levelData["bg_palms"], _content);
            BgPalmTiles = createTileGroup(bgpalmsLayout, "bg_palms");
            //bones 
            var bonesLayout = SupportingFunctions.ImportCvsLayout(levelData["bones"], _content);
            BoneTiles = createTileGroup(bonesLayout, "bones");
            //hearts
            var heartsLayout = SupportingFunctions.ImportCvsLayout(levelData["hearts"], _content);
            HeartTiles = createTileGroup(heartsLayout, "hearts");
            //one loop animation
            OneLoopAnimations = new List<OneLoopAnimation>();
            //enemy
            var enemiesLayout = SupportingFunctions.ImportCvsLayout(levelData["enemies"], _content);
            Enemies = createEnemies(enemiesLayout, "enemies");
            //constraints for enemies
            var constraintLayout = SupportingFunctions.ImportCvsLayout(levelData["constraints"], _content);
            ConstraintTiles = createTileGroup(constraintLayout, "constraints");

            //decoration classes
            int horizon = 6; //at which tile is the horizon
            int levelWidth = terrainLayout[0].Length * GameData.TileSize;
            Water = new Water(_content, levelWidth);
            ScrollingBackground = new ScrollingBackground(GameData.ImageSprites["backgroundClouds"], GameData.ImageSprites["backgroundSky"], GameData.ImageSprites["backgroundBottom"], levelWidth);

            //gui
            Gui = new Gui(_content);


            playBackgoundMusic();
            _game.IsMouseVisible = false;

            //gameplay rules
            BonesNeeded = 3;

        }
        //loads the content of all the files -> stores them into two dicts

        //generates enemies based on cvs
        private List<Enemy> createEnemies(List<string[]> terrainLayout, string grafic)
        {
            List<Enemy> enemies = new List<Enemy>();

            int indexRow = 0; // y position
            foreach (string[] row in terrainLayout)
            {
                int indexColumn = 0; //x position
                foreach (string val in row)
                {
                    int val1 = Convert.ToInt32(val);
                    //if var = -1 its empty
                    if (val1 != -1)
                    {
                        int x = indexColumn * GameData.TileSize;
                        int y = indexRow * GameData.TileSize;
                        if (val1 == 0)
                        {
                            Dictionary<string, List<Texture2D>> temp = new Dictionary<string, List<Texture2D>>();
                            temp.Add("run", GameData.AnimatedSprites["enemies/enemyHorizontal/run"]);
                            HorizontalEnemy enemy = new HorizontalEnemy(new Vector2(x, y), temp, 0.8f, "horizontal");
                            enemies.Add(enemy);
                        }
                        else if (val1 == 1)
                        {
                            Dictionary<string, List<Texture2D>> temp = new Dictionary<string, List<Texture2D>>();
                            temp.Add("jump", GameData.AnimatedSprites["enemies/enemyVertical/jump"]);
                            temp.Add("fall", GameData.AnimatedSprites["enemies/enemyVertical/jump"]);

                            VerticalEnemy enemy = new VerticalEnemy(new Vector2(x, y), temp, 0.8f, "vertical");
                            enemies.Add(enemy);
                        }
                        else if (val1 == 4)
                        {
                            Dictionary<string, List<Texture2D>> temp = new Dictionary<string, List<Texture2D>>();
                            temp.Add("idle", GameData.AnimatedSprites["enemies/enemyBoss/idle"]);

                            EnemyBoss enemy = new EnemyBoss(new Vector2(x, y), temp, 0.8f, "boss");
                            enemies.Add(enemy);
                        }


                    }
                    indexColumn++;
                }
                indexRow++;
            }
            return enemies;
        }
        private void createPlayer(List<string[]> terrainLayout) //sets the player and end tile
        {
            int indexRow = 0; // y position
            foreach (string[] row in terrainLayout)
            {
                int indexColumn = 0; //x position
                foreach (string val in row)
                {
                    int val1 = Convert.ToInt32(val);
                    //if var = -1 its empty
                    if (val1 != -1)
                    {
                        int x = indexColumn * GameData.TileSize;
                        int y = indexRow * GameData.TileSize;
                        if (val1 == 0)
                        {
                            Player = new Player(new Vector2(x, y), _content, _spriteBatch);
                        }
                        else if (val1 == 1)
                        {

                            EndLevelTile = new HumanTile(GameData.AnimatedSprites["human/idle"][0], new Vector2(x, y), null, GameData.AnimatedSprites["human/idle"], null);
                        }

                    }
                    indexColumn++;
                }
                indexRow++;
            }
        }
        //loops throught the level terain cvs and sets tiles       
        private List<Tile> createTileGroup(List<string[]> terrainLayout, string grafic)
        {
            List<Tile> tiles = new List<Tile>();
            int indexRow = 0; // y position
            foreach (string[] row in terrainLayout)
            {
                int indexColumn = 0; //x position
                foreach (string val in row)
                {
                    int val1 = Convert.ToInt32(val);
                    //if var = -1 its empty
                    if (val1 != -1)
                    {
                        int x = indexColumn * GameData.TileSize;
                        int y = indexRow * GameData.TileSize;
                        // TODO could optimize the location of the atlases and load them only once!
                        if (grafic == "terrain")
                        {
                            //slice terrain tile -> getting the source for drawing the rectangles                           
                            List<Rectangle> terrainTilesRectangles = SupportingFunctions.importCutGraphic(GameData.ImageSprites["terrain"]);
                            var drawRectangle = terrainTilesRectangles.ElementAt(val1);

                            Tile tile = new Tile(GameData.ImageSprites["terrain"], new Vector2(x, y), drawRectangle, 64);
                            tiles.Add(tile);
                        }
                        if (grafic == "grass")
                        {
                            List<Rectangle> grassRectangles = SupportingFunctions.importCutGraphic(GameData.ImageSprites["grass"]);
                            var drawRectangle = grassRectangles.ElementAt(val1);
                            Tile tile = new Tile(GameData.ImageSprites["grass"], new Vector2(x, y), drawRectangle, 64);
                            tiles.Add(tile);

                        }
                        if (grafic == "leaves")
                        {
                            List<Rectangle> leavesRectangles = SupportingFunctions.importCutGraphic(GameData.ImageSprites["leavesPurple"]);
                            var drawRectangle = leavesRectangles.ElementAt(val1);

                            Tile tile = new Tile(GameData.ImageSprites["leavesPurple"], new Vector2(x, y), drawRectangle, 64);
                            tiles.Add(tile);
                        }
                        if (grafic == "crate")
                        {
                            CrateTile tile = new CrateTile(GameData.ImageSprites["crate"], new Vector2(x, y), null, null);
                            tiles.Add(tile);
                        }
                        if (grafic == "coins")
                        {
                            List<Texture2D> textures;
                            int value;
                            if (val1 == 0)
                            {
                                textures = GameData.AnimatedSprites["coins/gold"];
                                value = 3;
                            }
                            else
                            {
                                textures = GameData.AnimatedSprites["coins/silver"];
                                value = 1;
                            }
                            CoinTile tile = new CoinTile(textures[0], new Vector2(x, y), null, textures, null, value);
                            tiles.Add(tile);
                        }
                        if (grafic == "fg_palms")
                        {

                            PalmTreeTile tile;
                            if (val1 == 0)
                            {
                                tile = new PalmTreeTile(GameData.AnimatedSprites["env/palm_small"][0], new Vector2(x, y), null, GameData.AnimatedSprites["env/palm_small"], 38, null);
                            }
                            else
                            {
                                tile = new PalmTreeTile(GameData.AnimatedSprites["env/palm_large"][0], new Vector2(x, y), null, GameData.AnimatedSprites["env/palm_large"], 70, null);
                            }
                            tiles.Add(tile);
                        }
                        if (grafic == "bg_palms")
                        {
                            PalmTreeTile tile = new PalmTreeTile(GameData.AnimatedSprites["env/palm_bg"][0], new Vector2(x, y), null, GameData.AnimatedSprites["env/palm_bg"], 60, null);
                            tiles.Add(tile);
                        }
                        if (grafic == "constraints")
                        {
                            Tile tile = new Tile(GameData.ImageSprites["constraint"], new Vector2(x, y), null, null);

                            tiles.Add(tile);
                        }
                        if (grafic == "bones")
                        {
                            BoneTile tile = new BoneTile(GameData.AnimatedSprites["bone"][0], new Vector2(x, y), null, GameData.AnimatedSprites["bone"], null);
                            tiles.Add(tile);
                        }
                        if (grafic == "hearts")
                        {
                            HeartTile tile = new HeartTile(GameData.ImageSprites["singleHeart"], new Vector2(x, y), null, null);
                            tiles.Add(tile);
                        }

                    }

                    indexColumn++;
                }
                indexRow++;
            }
            return tiles;

        }

        private void playBackgoundMusic()
        {
            MediaPlayer.Volume = PlayerStats.MusicVolume;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(GameData.Songs["0"]);


        }

        //camera effect -> shift the world // TODO : FIX maybe dogo always in the middle?
       

        public void horizontalMovementCollision()
        {
            //temp variables
            int width = (int)(Player.Width * Player.Scale);
            int height = (int)(Player.Height * Player.Scale);
            //player horizontal movement
            Player.Rectangle = new Rectangle((int)(Player.Rectangle.X + Player.Direction.X * Player.Speed), Player.Rectangle.Y, width, height);
            //cycle through all the tiles we can collide with
            //terrain, crates ,foreground palms

            var collidableTiles = joinCollidableTiles();
            foreach (var tile in collidableTiles)
            {
                //if player collides
                if (Player.Rectangle.Intersects(tile.Rectangle))
                {
                    //if player is moving to the left
                    if (Player.Direction.X < 0)
                    {
                        //move the player on the right side of the object
                        Player.Rectangle = new Rectangle(tile.Rectangle.Right, Player.Rectangle.Y, width, height);
                        //else player is moving to the right
                    }
                    else if (Player.Direction.X > 0)
                    {
                        //move the player on the left side of the object
                        Player.Rectangle = new Rectangle((int)(tile.Rectangle.Left - Player.Width * Player.Scale), Player.Rectangle.Y, width, height);
                    }
                }
            }
        }
        public void verticalMovementCollision()
        {
            //apply vertial player movement
            Player.gravity();
            //temp variables
            int width = (int)(Player.Width * Player.Scale);
            int height = (int)(Player.Height * Player.Scale);

            var collidableTiles = joinCollidableTiles();

            foreach (var tile in collidableTiles)
            {

                //if player collides
                if (Player.Rectangle.Intersects(tile.Rectangle))
                {
                    //if palyer is moving down
                    if (Player.Direction.Y > 0)
                    {
                        //move the player on the top of the object
                        Player.Rectangle = new Rectangle(Player.Rectangle.X, tile.Rectangle.Top - (int)(Player.Height * Player.Scale), width, height);

                        //reset gravity -> otherwise it yeets to the ground
                        Player.Direction = new Vector2(Player.Direction.X, 0);

                        Player.OnGround = true;

                    }
                    else if (Player.Direction.Y < 0) //else player is moving up
                    {
                        //move the player on the bottom of the object
                        Player.Rectangle = new Rectangle(Player.Rectangle.X, tile.Rectangle.Bottom, width, height);

                        //fall down from the celling 
                        Player.Direction = new Vector2(Player.Direction.X, 0);

                        Player.OnCeiling = true;
                    }

                }
            }

            //player on the floor or player jumping or falling, the player is not on the floor anymore
            if (Player.OnGround && Player.Direction.Y < 0 || Player.Direction.Y > 1)
                Player.OnGround = false;
            if (Player.OnCeiling && Player.Direction.Y > 0)
                Player.OnCeiling = false;

        }
        public void enemyConstraintCollision()//controlls movement of enemies, reverse speed
        {
            foreach (Enemy x in Enemies)
            {
                if (x.Type == "horizontal")
                {
                    HorizontalEnemy enemy = (HorizontalEnemy)x;
                    foreach (Tile constraint in ConstraintTiles)
                    {
                        if (enemy.Rectangle.Intersects(constraint.Rectangle))
                        {
                            enemy.ReverseDirection();
                        }
                    }
                }
                if (x.Type == "boss")
                {
                    EnemyBoss enemy = (EnemyBoss)x;
                    bool stopped = false;
                    string dir = "none";
                    Rectangle rec = new Rectangle(1, 1, 1, 1);
                    foreach (Tile constraint in ConstraintTiles)
                    {
                        string direction = PhysicsEngine.collisionDirection(enemy.Rectangle, enemy.Direction, constraint.Rectangle);
                        if (direction != "none")
                        {
                            stopped = true;
                            rec = constraint.Rectangle;
                            dir = direction;
                        }
                    }
                    if (stopped)
                    {
                        enemy.stop(dir, rec);
                    }
                    else
                    {
                        enemy.Continue();
                    }
                }

            }
        }
        //TODO - create jump particles, maybe rather in the player yes?
        public void createJumpParticles()
        {

        }
        //game rules methods
        private void checkWin()
        {
            //if the player is at the end of the level -> colliding with the end game tile
            if (Player.Rectangle.Intersects(EndLevelTile.Rectangle) && (BonesCurrent >= BonesNeeded))
            {
                PlayerStats.CompletedLevels += 1;
                PlayerStats.Save();
                GameData.SoundEffects["win"].Play(volume: PlayerStats.SoundEffectsVolume, 0.0f, 0.0f);


                if (GameData.NumberOfLevels == PlayerStats.CompletedLevels + 1)
                {
                    _game.ChangeState(new ThankYouState(_game, _graphicsDevice, _content, _spriteBatch));

                }
                else
                {
                    _game.ChangeState(new NextLevelState(_game, _graphicsDevice, _content, _spriteBatch));
                }
            }
            else
            {
                EndLevelTile.ShowHint();
            }

        }
        private void checkDeath()
        {
            //if no health or out of bounds
            if ((Player.CurrentHealth <= 0) || (Player.Rectangle.Y > DeathLine))
            {
                //game over -> so switch to gameover screen?
                _game.ChangeState(new DeathState(_game, _graphicsDevice, _content, _spriteBatch));

            }
        }
        private void coinCollision()
        {
            //loop through coins in reverse so you can remove the elements on runtime
            for (int i = CoinsTiles.Count - 1; i >= 0; i--)
            {
                CoinTile coin = (CoinTile)CoinsTiles[i];
                //remove the coin and add the value
                if (Player.Rectangle.Intersects(coin.Rectangle))
                {
                    CoinsTiles.RemoveAt(i);
                    Coins += coin.Value;
                    if (coin.Value == 3)
                    {
                        GameData.SoundEffects["goldCoin"].Play(volume: PlayerStats.SoundEffectsVolume, 0.0f, 0.0f);
                    }
                    else
                    {
                        GameData.SoundEffects["silverCoin"].Play(volume: PlayerStats.SoundEffectsVolume, 0.0f, 0.0f);
                    }

                }
            }
        }
        private void boneCollision()
        {
            //if collided and pressed the down button
            for (int i = BoneTiles.Count - 1; i >= 0; i--)
            {
                BoneTile bone = (BoneTile)BoneTiles[i];
                //pickup the bone 
                if (Player.Rectangle.Intersects(bone.Rectangle) && Player.PlayerStatus =="dog/sniff")
                {
                    BoneTiles.RemoveAt(i);
                    BonesCurrent += 1;
                    GameData.SoundEffects["bone"].Play(volume: PlayerStats.SoundEffectsVolume, 0.0f, 0.0f);

                }
            }

        }
        //manages one loop animations -> if they have finished, removes from the list
        private void DrawingOneLoopAnimations()
        {
            //if collided add to health
            for (int i = OneLoopAnimations.Count - 1; i >= 0; i--)
            {
                OneLoopAnimation animation = (OneLoopAnimation)OneLoopAnimations[i];
                if (!animation.FinishAnimation)
                {
                    animation.Update();
                    animation.Draw(_spriteBatch);
                }
                else
                {
                    OneLoopAnimations.RemoveAt(i);
                }
            }
        }
        private void heartCollision()
        {
            //if collided add to health
            for (int i = HeartTiles.Count - 1; i >= 0; i--)
            {
                HeartTile heart = (HeartTile)HeartTiles[i];
                //pickup the bone 
                //TODO : play the crouch animation -> that is in player class
                if (Player.Rectangle.Intersects(heart.Rectangle))
                {
                    HeartTiles.RemoveAt(i);
                    GameData.SoundEffects["heart"].Play(volume: PlayerStats.SoundEffectsVolume, 0.0f, 0.0f);

                    Player.AddHealth();
                }
            }

        }
        //TODO  add hearts
        private void enemyCollision() // collisions with enemies and player
        {
            for (int i = Enemies.Count - 1; i >= 0; i--)
            {
                Enemy enemy = (Enemy)Enemies[i];
                //remove the coin and add the value
                if (Player.Rectangle.Intersects(enemy.Rectangle))
                {
                    int enemyCenterY = enemy.Rectangle.Center.Y;
                    int enemyTopY = enemy.Rectangle.Top;
                    int playerBottom = Player.Rectangle.Bottom;
                    if ((enemyTopY < playerBottom) && (playerBottom < enemyCenterY) && (Player.Direction.Y >= 0))
                    {
                        //enemyTakeDamage 
                        enemy.takeDamage();
                        GameData.SoundEffects["stomp"].Play(volume: PlayerStats.SoundEffectsVolume, 0.0f, 0.0f);
                        Player.Direction = new Vector2(0, -15); //jump up
                        if (enemy.CurrentHealth == 0)
                        {
                            //kill the enemy -> just remove from the list
                            Enemies.RemoveAt(i);
                            if (enemy.Type == "boss")//when boss is killed, it drops a bone where it died
                            {
                                EnemyBoss boss = (EnemyBoss)enemy;
                                //create the bonetile, add to the BoneTile list
                                Vector2 position = new Vector2(boss.Rectangle.X, boss.MinY);
                                BoneTile bone = new BoneTile(GameData.AnimatedSprites["bone"][0], position, null, GameData.AnimatedSprites["bone"], null);
                                BoneTiles.Add(bone);
                            }

                            //explosion animation
                            int x = enemy.Rectangle.X - enemy.Rectangle.Width / 2;
                            int y = enemy.Rectangle.Y - enemy.Rectangle.Height / 2;
                            OneLoopAnimation explosion = new OneLoopAnimation(GameData.AnimatedSprites["effects/explosion"], new Vector2(x, y), 0.5f);
                            OneLoopAnimations.Add(explosion);
                        }
                        //TODO : update enemy health bar gui


                    }
                    else
                    {
                        Player.takeDamage();


                    }
                }
            }
        }

        private dynamic joinCollidableTiles()//returns a list of collidable tiles
        {
            var temp = new List<Tile>(TerrainTiles.Count +
                                   CratesTiles.Count +
                                   FgPalmTiles.Count);
            temp.AddRange(TerrainTiles);
            temp.AddRange(CratesTiles);
            temp.AddRange(FgPalmTiles);

            return temp;
        }
        public void Update(GameTime gameTime)
        {
            _camera.Follow(Player);
            GameData.CameraMatrix = _camera.CameraMatrix;
            //from the center of the player and also relative to the device width/height
            Player.Update();
            horizontalMovementCollision();
            verticalMovementCollision();
            Gui.Update(_camera.CenterPosition);
#if ANDROID           
            Player.AndroidGui.Update(_camera.CenterPosition,gameTime);

#endif

        }
        //drawing and updating the level
        public void Draw(GameTime gameTime)
        {
#if DESKTOP
            _spriteBatch.Begin(transformMatrix:_camera.CameraMatrix);
#elif ANDROID
            _spriteBatch.Begin(transformMatrix: _camera.CameraMatrix * GameData.LevelScaleMatrix);
#endif


            //background images
            ScrollingBackground.Update();
            ScrollingBackground.Draw(_spriteBatch);
         
            //for background palms
            foreach (PalmTreeTile tile in BgPalmTiles)
            {
                tile.Update();
                tile.Draw(_spriteBatch);
            }
            //for leaves
            foreach (Tile tile in LeavesTiles)
            {
                tile.Update();
                tile.Draw(_spriteBatch);
            }
            //for level tiles
            foreach (Tile tile in TerrainTiles)
            {
                tile.Update();
                tile.Draw(_spriteBatch);
            }

            //for crates
            foreach (CrateTile tile in CratesTiles)
            {
                tile.Update();
                tile.Draw(_spriteBatch);
            }
            //for enemy constraints
            foreach (Tile constraint in ConstraintTiles)
            {
                constraint.Update();
            }

            //for foreground palms
            foreach (PalmTreeTile tile in FgPalmTiles)
            {
                tile.Update();
                tile.Draw(_spriteBatch);
            }
            //for enemies
            enemyConstraintCollision();
            foreach (Enemy enemy in Enemies)
            {
                enemy.Update( new Vector2(Player.Rectangle.X, Player.Rectangle.Y));
                enemy.Draw(_spriteBatch);
            }


            //for grass tiles
            foreach (Tile tile in GrassTiles)
            {
                tile.Update();
                tile.Draw(_spriteBatch);
            }


            //for coins
            foreach (CoinTile tile in CoinsTiles)
            {
                tile.Update();
                tile.Draw(_spriteBatch);
            }
            //for hearts
            foreach (HeartTile tile in HeartTiles)
            {
                tile.Update();
                tile.Draw(_spriteBatch);
            }
            //for bones
            foreach (BoneTile tile in BoneTiles)
            {
                tile.Update();
                tile.Draw(_spriteBatch);
            }

            //for one time animations -> explosions, check if there are finished or not
            DrawingOneLoopAnimations();

            //end level tile
            EndLevelTile.Update();
            EndLevelTile.Draw(_spriteBatch);
            //player
           // Player.Update();
            //horizontalMovementCollision();
            //verticalMovementCollision();
           
            Player.Draw();


            //gui
            Gui.Draw(_spriteBatch, Player.CurrentHealth, Player.MaxHealth, Coins, BonesCurrent, BonesNeeded);
#if ANDROID
            Player.AndroidGui.Draw(_spriteBatch,gameTime);
#endif
            checkDeath();
            checkWin();

            //collisions with player and interactable objects
            coinCollision();
            enemyCollision();
            boneCollision();
            heartCollision();

            //water
            Water.Draw(_spriteBatch);
            Water.DrawFront(_spriteBatch);


            //TODO : maybe add all the game rules and collisions into a controller?
            _spriteBatch.End();

        }


    }
}
