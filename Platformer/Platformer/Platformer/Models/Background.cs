using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Platformer.Sprites;
using Platformer.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Platformer.Models
{
    public class ScrollingBackground
    {
        private Texture2D TextureMid;
        private Texture2D TextureTop;

        public Texture2D TextureBottom { get; }
        private List<Vector3> Vectors { get; set; }//vector that stores x,y, flip

        public ScrollingBackground(Texture2D textureMid, Texture2D textureTop, Texture2D textureBottom, int levelWidth)
        {
            TextureMid = textureMid;
            TextureTop = textureTop;
            TextureBottom = textureBottom;
            int top = 0;// y top level of background
            int backgroundStart = -2 * GameData.LevelScreenWidth; // x position of starting backgound
            int backgroundTileWidth = TextureMid.Width;//width of the texture
            int backgroundTileXAmount = (int)((levelWidth + GameData.LevelScreenWidth) / backgroundTileWidth) + 2; // how many tiles do we need
            Trace.WriteLine(backgroundTileXAmount);

            Vectors = new List<Vector3>();
            for (int i = 0; i <= backgroundTileXAmount; i++)
            {
                int x = i * backgroundTileWidth + backgroundStart;
                int y = top;
                int flip = i % 2 == 0 ? 0 : 1; //0 dont flip/1 flip

                Vector3 vector = new Vector3(x, y, flip);
                Vectors.Add(vector);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Vector3 back in Vectors)
            {
                if (back.Z == 0)
                {

                    spriteBatch.Draw(TextureMid, new Vector2(back.X, back.Y), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    spriteBatch.Draw(TextureBottom, new Vector2(back.X, back.Y + TextureMid.Height), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);


                }
                else
                {
                    spriteBatch.Draw(TextureMid, new Vector2(back.X, back.Y), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);
                    spriteBatch.Draw(TextureBottom, new Vector2(back.X, back.Y + TextureMid.Height), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);

                }
                spriteBatch.Draw(TextureTop, new Vector2(back.X, back.Y - TextureTop.Height), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            }
        }
        public void Update()
        {
            
        }
    }




    public class Water
    {
        private ContentManager _content { get; set; }

        List<Texture2D> Textures { get; set; }
        private Texture2D WaterFront { get; set; }
        List<AnimatedTile> WaterAnimatedTiles { get; set; }
        public List<Vector2> Positions { get; private set; }

        public Water(ContentManager content, int levelWidth)
        {
            _content = content;
            LoadContent();

            int top = GameData.VerticalTileNumber * GameData.TileSize - Textures[0].Height + 10;// y top level of water
            int waterStart = -10 * GameData.TileSize; // x position of water
            int waterTileWidth = Textures[0].Width;//width of the texture
            int waterTileXAmount = (int)((levelWidth + GameData.LevelScreenWidth) / waterTileWidth); // how many tiles do we need

            //create animated water tiles
            WaterAnimatedTiles = new List<AnimatedTile>();
            for (int i = 0; i <= waterTileXAmount + 5; i++)
            {
                int x = i * waterTileWidth + waterStart;
                int y = top;
                AnimatedTile waterSprite = new AnimatedTile(Textures[0], new Vector2(x, y), null, Textures, null); //animation will happen in the animationTile class
                WaterAnimatedTiles.Add(waterSprite);
            }
            //create position of water front
            int waterFrontTileXAmount = (int)((levelWidth + GameData.LevelScreenWidth) / WaterFront.Width); // how many tiles do we need

            Positions = new List<Vector2>();
            for (int i = 0; i <= waterFrontTileXAmount; i++)
            {
                int x = i * WaterFront.Width + waterStart;
                int y = top + Textures[0].Height;

                Vector2 vector = new Vector2(x, y);
                Positions.Add(vector);
            }
        }
        public void LoadContent()
        {
            Textures = GameData.AnimatedSprites["decoration/water"];
            WaterFront = GameData.ImageSprites["waterFront"];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (AnimatedTile water in WaterAnimatedTiles)
            {
                water.Update();
                water.Draw(spriteBatch);
            }

        }
        public void DrawFront(SpriteBatch spriteBatch)
        {
            foreach (Vector2 vector in Positions)
            {
                spriteBatch.Draw(WaterFront, new Vector2(vector.X, vector.Y), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            }
        }


    }
    public class Clouds
    {
        public ContentManager _content { get; private set; }
        public int CloudNumber { get; private set; }
        public int Horizon { get; private set; }
        public List<Texture2D> Textures { get; private set; }
        public List<Tile> CloudTiles { get; private set; }

        public Clouds(ContentManager content, int cloudNumber, int horizon, int levelWidth)
        {

            _content = content;
            CloudNumber = cloudNumber;
            LoadContent();

            int minX = -GameData.LevelScreenWidth;
            int maxX = levelWidth + GameData.LevelScreenWidth;
            int minY = 0;
            int maxY = horizon * GameData.TileSize;

            //generate set number of clouds in the sky
            Random rnd = new Random();
            CloudTiles = new List<Tile>();
            for (int i = 0; i <= cloudNumber; i++)
            {
                int index = rnd.Next(0, Textures.Count);  // creates a number between 0 and length of textures
                var texture = Textures[index];
                int x = rnd.Next(minX, maxX + 1);
                int y = rnd.Next(minY, maxY + 1);
                Tile cloud = new Tile(texture, new Vector2(x, y), null, null);
                CloudTiles.Add(cloud);

            }
        }
        public void LoadContent()
        {
            Textures = GameData.AnimatedSprites["decoration/clouds"];
        }
        public void Draw(SpriteBatch spriteBatch, int worldShiftX, int worldShiftY)
        {
            foreach (Tile cloud in CloudTiles)
            {
                cloud.Update();
                cloud.Draw(spriteBatch);
            }
        }
        public void Update()
        {

        }


    }
}
