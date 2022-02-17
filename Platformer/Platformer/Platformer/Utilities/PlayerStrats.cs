using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Text.Json;

namespace Platformer.Utilities
{
    public class SaveStats
    {
        public float MusicVolume { get; set; }
        public float SoundEffectsVolume { get; set; }
        public int CompletedLevels { get; set; }
        public Dictionary<string, List<int>> HighScores { get; set; } //{level_number: [coins, time_in_seconds] }

    }
    //class used for saving game state
    public static class PlayerStats
    {
        #region Attributes

        public static float MusicVolume { get; set; } = 0.1f;
        public static float SoundEffectsVolume { get; set; } = 0.1f;
        public static int CompletedLevels { get; set; } = 1; //Number of completed levels
        public static Dictionary<string, List<int>> HighScores { get; set; } = new Dictionary<string, List<int>>(); 


        #endregion
        private const string PATH = "stats.json";

        #region Methods
        public static void AddToHighscoreDictionary(string key, List<int> value)
        {
            if(HighScores == null)
            {
                HighScores = new Dictionary<string, List<int>>();
            }
            if ( HighScores.ContainsKey(key))
            {
                //save the better result
                if(value[0]>= HighScores[key][0] || value[0] >= HighScores[key][0] && value[1] < HighScores[key][1])
                {
                    HighScores[key] = value;
                }
            }
            else
            {
                HighScores.Add(key, value);
            }
        }
#if DESKTOP
        public static bool SaveExist()//checks if the save file exists or not
        {
            return File.Exists(PATH);
        }
        public static void Save()//save when changed settings or only when exiting :)
        {
            SaveStats obj = new SaveStats();//create an instance of the savable class
            obj.MusicVolume = MusicVolume;
            obj.SoundEffectsVolume = SoundEffectsVolume;
            obj.CompletedLevels = CompletedLevels;
            obj.HighScores =HighScores;


            string serializedText = JsonSerializer.Serialize<SaveStats>(obj);

            //path for saving
            File.WriteAllText(PATH, serializedText);
         

        }

        public static void Load()//load only on start
        {
            var deserializedData = File.ReadAllText(PATH);
            var data = JsonSerializer.Deserialize<SaveStats>(deserializedData);
            //set the class ? maybe faster way to do this in c?

            MusicVolume = data.MusicVolume;
            SoundEffectsVolume = data.SoundEffectsVolume;
            CompletedLevels = data.CompletedLevels;
            HighScores = data.HighScores;


        }
#elif ANDROID
        public static bool SaveExist()//checks if the save file exists or not
        {
            var store = IsolatedStorageFile.GetUserStoreForApplication();
            return store.FileExists("stats.json");
        }
        public static void Save()
        {
            IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForApplication();
            StreamWriter writer = new StreamWriter(new IsolatedStorageFileStream("stats.json", FileMode.OpenOrCreate, savegameStorage));

            //json serialize the data
            SaveStats obj = new SaveStats();//create an instance of the savable class
            obj.MusicVolume = MusicVolume;
            obj.SoundEffectsVolume = SoundEffectsVolume;
            obj.CompletedLevels = CompletedLevels;
            obj.HighScores = HighScores;



            string serializedText = JsonSerializer.Serialize<SaveStats>(obj);
            writer.WriteLine(serializedText);
            writer.Close();

          
        }
        public static void Load()
        {
            IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForApplication();
            try
            {
                StreamReader reader = new StreamReader(new IsolatedStorageFileStream("stats.json", FileMode.Open, savegameStorage));
                String content = reader.ReadToEnd();
                reader.Close();

                var data = JsonSerializer.Deserialize<SaveStats>(content);
                //set the class ? maybe faster way to do this in c?
                Trace.WriteLine("loaded:" + data.MusicVolume + "  " + data.SoundEffectsVolume);

                MusicVolume = data.MusicVolume;
                SoundEffectsVolume = data.SoundEffectsVolume;
                CompletedLevels = data.CompletedLevels;
                HighScores = data.HighScores;

               


            }
            catch
            {
                Trace.WriteLine( "the file was not found");
            }
        }
#endif
        #endregion
    }
}
