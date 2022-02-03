using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Platformer.Utilities
{
    public class SaveStats
    {
        public float MusicVolume { get; set; }
        public float SoundEffectsVolume { get; set; }
        public int CompletedLevels { get; set; }

    }
    //class used for saving game state
    public static class PlayerStats
    {
        #region Attributes

        public static float MusicVolume { get; set; } = 0.1f;
        public static float SoundEffectsVolume { get; set; } = 0.1f;
        public static int CompletedLevels { get; set; } = -1; //Number of completed levels

        #endregion
        private const string PATH = "stats.json";

        #region Methods

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


            string serializedText = JsonSerializer.Serialize<SaveStats>(obj);

            Trace.WriteLine("Saved:" + serializedText);
            //path for saving

            File.WriteAllText(PATH, serializedText);


        }

        public static void Load()//load only on start
        {
            var deserializedData = File.ReadAllText(PATH);
            var data = JsonSerializer.Deserialize<SaveStats>(deserializedData);
            //set the class ? maybe faster way to do this in c?
            Trace.WriteLine("loaded:" + data.MusicVolume + "  " + data.SoundEffectsVolume);

            MusicVolume = data.MusicVolume;
            SoundEffectsVolume = data.SoundEffectsVolume;
            CompletedLevels = data.CompletedLevels;


        }
        #endregion
    }
}
