using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Platformer.Utilities;


using System.Diagnostics;
using System.Reflection;
using System.IO.IsolatedStorage;

namespace Platformer
{
    //class for general supporting functions
    public static class SupportingFunctions
    {
        //returna list rectanglov tilov, shanjene v zaporedju, da mactchajo id -> sepravi od 0 naprej
        public static List<Rectangle> importCutGraphic(Texture2D atlas)//loads the sprite atlas that will be cut
        {
            List<Rectangle> rectangles = new List<Rectangle>();

            int tileNumX = (int)(atlas.Width / GameData.TileSize);
            int tileNumY = (int)(atlas.Height / GameData.TileSize);

            for(int row =0;row<tileNumX; row++)
            {
                for(int col = 0; col < tileNumY; col++)
                {
                    int x = col * GameData.TileSize;
                    int y = row * GameData.TileSize;

                    Rectangle rec = new Rectangle(x, y, GameData.TileSize, GameData.TileSize);
                    rectangles.Add(rec);
                }
            }
            return rectangles;

        }
#if DESKTOP
        public static List<string[]> ImportCvsLayout(string path)///parses a cvs file
        {
            //this is a retarded solution -> basically to accuratly read the csv files
            //you have to move up into the project, because you have to give the TextFieldParser the full path
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            dir += "//..//..//..//";
            using (Microsoft.VisualBasic.FileIO.TextFieldParser parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(dir+path))
            {
                List<string[]> terrainMap = new List<string[]>();
                parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    //Processing row
                    string[] row = parser.ReadFields();
                    terrainMap.Add(row);
                    foreach (string field in row)
                    {
                        //TODO: Process field
                    }
                }
                return terrainMap;
            }
        }

         //returns a list of images in that folder
        public static List<Texture2D> ImportFolder(string folderName ,string fullPath,ContentManager content)
        {
            List<Texture2D> temp = new List<Texture2D>();
            DirectoryInfo d = new DirectoryInfo(fullPath);
            FileInfo[] Files = d.GetFiles("*.png"); //Getting png files
            Trace.Write("{ " + '"'+folderName + '"'+ " , new string[] {");
            foreach (FileInfo file in Files)
            {
                //cut out .png
                string[] tempString = file.Name.Split('.');
                Trace.Write('"'+file.Name+'"'+" , ");
                string fileName = tempString[0];
                string finalName = folderName + '/' + fileName;
                temp.Add(content.Load<Texture2D>(finalName));
            }
            Trace.WriteLine("}}, \n");
            
            return temp;

        }
#endif

#if ANDROID
        public static List<string[]> ImportCvsLayout(string path)///parses a cvs file
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            dir += "//..//..//..//";
            StreamReader sr = new StreamReader(dir+path);
            List<string[]> terrainMap = new List<string[]>();

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] row = line.Split(',');
                terrainMap.Add(row);
            }
            return terrainMap;

        }

        //returns a list of images in that folder
        public static List<Texture2D> ImportFolder(string key,string[] files, ContentManager content)
        {
            List<Texture2D> temp = new List<Texture2D>();
            foreach(string file in files)
            {
                temp.Add(content.Load<Texture2D>("game/"+key+"/"+file));
            }           
            return temp;
        }

#endif



            //TODO separate the loading process
            //loads the background of main menu and settings
            public static Dictionary<string, Texture2D> LoadBackground(ContentManager content)
        {
            Dictionary<string,Texture2D> background = new Dictionary<string, Texture2D>();
            background.Add("background", content.Load< Texture2D >("menu/Additional Sky"));
            //background.Add("background1", content.Load<Texture2D>("menu/background1"));
            background.Add("bigclouds", content.Load<Texture2D>("menu/Big Clouds"));
            background.Add("smallcloud1", content.Load<Texture2D>("menu/Small Cloud 1"));
            background.Add("smallcloud2", content.Load<Texture2D>("menu/SmallCloud2"));
            background.Add("smallcloud3", content.Load<Texture2D>("menu/SmallCloud3"));


          




            return background;

        }
        //draws the background of mainmenu and settings; returns the value of layer
        public static void DrawBackground(Dictionary<string, Texture2D> images,SpriteBatch spriteBatch,int width, int height )
        {
             //TODO -> Maybe change the loading and drawing of the background like idk move it somewhere or smth

            Rectangle destinationRectangle = new Rectangle(0, 0, width,height);
            //background clouds
            //_graphicsDevice.Clear(new Color(142,202,230));
            spriteBatch.Draw(images["background"], destinationRectangle, Color.White);
            spriteBatch.Draw(images["smallcloud1"], new Vector2(30, 300), null, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
            spriteBatch.Draw(images["smallcloud2"], new Vector2(width-120, 150), null, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
            spriteBatch.Draw(images["smallcloud3"], new Vector2(150, height-88), null, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
            spriteBatch.Draw(images["smallcloud3"], new Vector2(377, 234), null, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.FlipHorizontally, 0f);
            spriteBatch.Draw(images["smallcloud1"], new Vector2(width-200, 100), null, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.FlipHorizontally, 0f);
            spriteBatch.Draw(images["smallcloud2"], new Vector2(width - 164, height-88), null, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.FlipHorizontally, 0f);
            spriteBatch.Draw(images["smallcloud3"], new Vector2(133, height/2-120), null, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);

     
        }
        public static void DrawJungleBackground(Dictionary<string, Texture2D> images, SpriteBatch spriteBatch, int width, int height)
        {
            Rectangle destinationRectangle = new Rectangle(0, 0, width, height);
            spriteBatch.Draw(images["plx1"], destinationRectangle, Color.White);
            spriteBatch.Draw(images["plx2"], destinationRectangle, Color.White);
            spriteBatch.Draw(images["plx3"], destinationRectangle, Color.White);
            spriteBatch.Draw(images["plx4"], destinationRectangle, Color.White);
            spriteBatch.Draw(images["plx5"], destinationRectangle, Color.White); 
        }
        public static void printStringArray(string[] array)
        {
            Trace.Write("[ ");
            foreach(var a in array)
            {
                Trace.Write(a + " ,");
            }
            Trace.Write("] \n");
        }


    }
}
