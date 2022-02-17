using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platformer.Utilities
{
    //all the gamedata from classes and settings is stored here
    public static class GameData

    {
        public static Dictionary<string, string> switchLevels
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "0","level_0" },
                };
            }
        }
        public static Dictionary<string, string> level_0
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "bg_palms","LevelData/0/level_0_bg_palms.csv" },
                    { "coins","LevelData/0/level_0_coins.csv" },
                    { "bones","LevelData/0/level_0_bones.csv" },
                    { "hearts","LevelData/0/level_0_hearts.csv" },
                    { "constraints","LevelData/0/level_0_constraints.csv" },
                    { "leaves","LevelData/0/level_0_leaves.csv" },
                    { "enemies","LevelData/0/level_0_enemies.csv" },
                    { "fg_palms","LevelData/0/level_0_fg_palms.csv" },
                    { "grass","LevelData/0/level_0_grass.csv" },
                    { "crate","LevelData/0/level_0_crate.csv"},
                    { "player","LevelData/0/level_0_player.csv" },
                    { "terrain","LevelData/0/level_0_terrain.csv" },
                    { "instructions","LevelData/0/level_0_instructions.csv" },

                };
            }
        }
        public static Dictionary<string, string> level_1
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "bg_palms","LevelData/1/level_1_bg_palms.csv" },
                    { "coins","LevelData/1/level_1_coins.csv" },
                    { "bones","LevelData/1/level_1_bones.csv" },
                    { "hearts","LevelData/1/level_1_hearts.csv" },
                    { "constraints","LevelData/1/level_1_constraints.csv" },
                    { "leaves","LevelData/1/level_1_leaves.csv" },
                    { "enemies","LevelData/1/level_1_enemies.csv" },
                    { "fg_palms","LevelData/1/level_1_fg_palms.csv" },
                    { "grass","LevelData/1/level_1_grass.csv" },
                    { "crate","LevelData/1/level_1_crate.csv"},
                    { "player","LevelData/1/level_1_player.csv" },
                    { "terrain","LevelData/1/level_1_terrain.csv" },
                    { "instructions","LevelData/1/level_1_instructions.csv" },

                };
            }
        }
        public static Dictionary<string, string> level_2
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "bg_palms","LevelData/2/level_2_bg_palms.csv" },
                    { "coins","LevelData/2/level_2_coins.csv" },
                    { "bones","LevelData/2/level_2_bones.csv" },
                    { "hearts","LevelData/2/level_2_hearts.csv" },
                    { "constraints","LevelData/2/level_2_constraints.csv" },
                    { "leaves","LevelData/2/level_2_leaves.csv" },
                    { "enemies","LevelData/2/level_2_enemies.csv" },
                    { "fg_palms","LevelData/2/level_2_fg_palms.csv" },
                    { "grass","LevelData/2/level_2_grass.csv" },
                    { "crate","LevelData/2/level_2_crate.csv"},
                    { "player","LevelData/2/level_2_player.csv" },
                    { "terrain","LevelData/2/level_2_terrain.csv" },
                    { "instructions","LevelData/2/level_2_instructions.csv" },

                };
            }
        }



        public static int NumberOfLevels { get { return 3; } }

        //sprites
        public static Dictionary<string, Texture2D> ImageSprites { get; set; }
        public static Dictionary<string, List<Texture2D>> AnimatedSprites { get; set; }
        public static Dictionary<string, SoundEffect> SoundEffects { get; set; }
        public static Dictionary<string, SpriteFont> Fonts { get; set; }

        public static Dictionary<string, Song> Songs { get; set; }
        public static Dictionary<string, string[]> NamesAnimatedSprites  // this is a retarded solution -> android cannot get file names from directory, therfore all file names will be listed as values , the key is the folder name : )
        {
            get
            {
                return new Dictionary<string, string[]>
                {
                    
               { "enemies/enemyHorizontal/run" , new string[] {"Meow-Knight_Run_1" , "Meow-Knight_Run_2" , "Meow-Knight_Run_3" , "Meow-Knight_Run_4"  }},


               { "enemies/enemyVertical/jump" , new string[] {"Meow-Knight_Jump1" , "Meow-Knight_Jump2" , "Meow-Knight_Jump3" , "Meow-Knight_Jump4"  }},

               
               { "enemies/enemyBoss/idle" , new string[] {"idle1" , "idle2" , "idle3" , "idle4"  }},

               { "coins/gold" , new string[] {"0" , "1" , "2" , "3"  }},

               { "coins/silver" , new string[] {"0" , "1" , "2" , "3"  }},

               { "env/palm_small" , new string[] {"small_1" , "small_2" , "small_3" , "small_4"  }},

               { "env/palm_large" , new string[] {"large_1" , "large_2" , "large_3" , "large_4"  }},

               { "env/palm_bg" , new string[] {"bg_palm_1" , "bg_palm_2" , "bg_palm_3" , "bg_palm_4"  }},


               { "bone" , new string[] {"pixil-frame-0" , "pixil-frame-1" , "pixil-frame-2" , "pixil-frame-3" , "pixil-frame-4" , "pixil-frame-5" }},

               { "hearts" , new string[] {"heart1" , "heart3"  }},

               { "effects/explosion" , new string[] {"1" , "2" , "3" , "4" , "5" , "6" , "7"  }},


               { "human/idle" , new string[] {"FinnSprite" , "FinnSprite1" , "FinnSprite3"  }},
                    

               { "decoration/water" , new string[] {"1" , "2" , "3" , "4"  }},

               { "decoration/clouds" , new string[] {"1" , "2" , "3"   }},

               { "dog/jump" , new string[] {"jump_1", "jump_2","jump_3","jump_4","jump_5","jump_6", }},


               { "dog/fall" , new string[] {"fall_1" , "fall_2" ,"fall_3" ,"fall_4" ,"fall_5"  }},

               { "dog/sniff" , new string[] {"sniff1","sniff2","sniff4" }},


               { "dog/idle1" , new string[] {"idle_1","idle_2","idle_3","idle_4","idle_5"  }},

               { "dog/run" , new string[] {"run_1","run_2","run_3","run_4","run_5","run_6","run_7","run_8",   }},

               { "dog/dust/run" , new string[] {"Run01","Run02","Run03","Run04","Run05"  }},

               { "dog/dust/jump" , new string[] {"Jump 01","Jump 02","Jump 03","Jump 04","Jump 05"  }},

               { "dog/dust/fall" , new string[] {"Fall 01", "Fall 02", "Fall 03", "Fall 04", "Fall 05"  }},

             


                };
            }
        }
        // for loading level data
        public static int VerticalTileNumber { get; set; } = 11;
        public static int TileSize { get { return 64; } }
        public static int AndroidScreenHeight { get; set; }
        public static int AndroidScreenWidth { get; set; }
        public static Matrix MenuScaleMatrix {get;set;}
        public static Matrix LevelScaleMatrix {get;set;}

#if DESKTOP
        public static int LevelScreenHeight { get { return VerticalTileNumber * TileSize; } }
        public static int LevelScreenWidth { get { return 800; } }

#elif ANDROID
        public static int LevelScreenHeight { get { return 480; } }
        public static int LevelScreenWidth { get { return 900; } }

#endif
        public static int InitialScreenWidth { get { return 800; } }
        public static int InitialScreenHeight { get { return 480; } }

        public static Matrix CameraMatrix { get; set; }

    }

}
