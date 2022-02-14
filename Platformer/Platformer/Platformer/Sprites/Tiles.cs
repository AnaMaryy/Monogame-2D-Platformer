using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Platformer.Sprites
{
    public class Tile
    {
        public Rectangle Rectangle;
        public Vector2 Position;
        public Texture2D Texture;
        public int TileWidth;
        public int TileHeight;
        public Rectangle? DrawRectangle { get; set; } //rectangle for drawing out of the atlas, if null the texture is not in an atlas

        public Tile(Texture2D image, Vector2 position, Rectangle? drawRectangle, int? tilesize)
        {
            DrawRectangle = drawRectangle;
            if (tilesize == null)
            {
                TileWidth = image.Width;
                TileHeight = image.Height;
            }
            else
            {
                TileWidth = (int)tilesize;
                TileHeight = (int)tilesize;
            }
            Position = position;
            Texture = image;
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, TileWidth, TileHeight);
        }
        public void Draw(SpriteBatch spriteBatch) //draws based on atlas
        {
            if (DrawRectangle != null)//draw from atlas or not
            {
                spriteBatch.Draw(Texture, new Vector2(Rectangle.X, Rectangle.Y), DrawRectangle, Color.White);

            }
            else
            {
                spriteBatch.Draw(Texture, new Vector2(Rectangle.X, Rectangle.Y), Color.White);
            }

        }

        //sifts the tiles based on the desired speed
        public void Update()
        {
        }

    }
    public class CrateTile : Tile //crate is smaller than 64x64, youll have to change the draw postion
    {

        public CrateTile(Texture2D image, Vector2 position, Rectangle? drawRectangle, int? tilesize) : base(image, position, drawRectangle, tilesize)
        {
            int offsetY = (int)(position.Y + GameData.TileSize - image.Height);
            Rectangle = new Rectangle(Rectangle.X, offsetY, TileWidth, TileHeight);
        }

    }
    public class AnimatedTile : Tile
    {
        public float FrameIndex { get; set; }
        public float AnimationSpeed { get; private set; }
        public List<Texture2D> Textures { get; set; }
        //TODO : fix the constructor maybe? remove the image, BUT PROBLEM : this is used in the base constructor so yeah
        public AnimatedTile(Texture2D image, Vector2 position, Rectangle? drawRectangle, List<Texture2D> textures, int? tilesize) : base(image, position, drawRectangle, tilesize)
        {
            FrameIndex = 0f;
            AnimationSpeed = 0.15f;
            Textures = textures;

        }
        public void Animate()
        {
            FrameIndex += AnimationSpeed;
            if (FrameIndex >= Textures.Count)
            {
                FrameIndex = 0;
            }
            Texture = Textures[(int)FrameIndex];
        }
        public new void Update()
        {
            Animate();
        }
    }
    public class CoinTile : AnimatedTile
    {
        public int Value { get; set; }
        public CoinTile(Texture2D image, Vector2 position, Rectangle? drawRectangle, List<Texture2D> textures, int? tilesize, int value) : base(image, position, drawRectangle, textures, tilesize)
        {
            int offsetX = (int)(position.X + GameData.TileSize / 2 - image.Width / 2);
            int offsetY = (int)(position.Y + GameData.TileSize / 2 - image.Height / 2);
            Rectangle = new Rectangle(offsetX, offsetY, TileWidth, TileHeight);
            Value = value;
        }
    }
    public class PalmTreeTile : AnimatedTile
    {
        public PalmTreeTile(Texture2D image, Vector2 position, Rectangle? drawRectangle, List<Texture2D> textures, int offset, int? tilesize) : base(image, position, drawRectangle, textures, tilesize)
        {
            int offsetY = (int)(position.Y - offset);
            Rectangle = new Rectangle(Rectangle.X, offsetY, TileWidth, TileHeight);
        }

    }
    public class BoneTile : AnimatedTile
    {
        public BoneTile(Texture2D image, Vector2 position, Rectangle? drawRectangle, List<Texture2D> textures, int? tilesize) : base(image, position, drawRectangle, textures, tilesize)
        {
            int offsetY = Rectangle.Y + GameData.TileSize - image.Height;
            Rectangle = new Rectangle(Rectangle.X, offsetY, TileWidth, TileHeight);
        }
        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Vector2(Rectangle.X, Rectangle.Y), null, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
        }
    }
    public class HeartTile : Tile
    {

        public HeartTile(Texture2D image, Vector2 position, Rectangle? drawRectangle, int? tilesize) : base(image, position, drawRectangle, tilesize)
        {
            //offset to be in the middle of the tile 64x 64
            int offsetX = (int)((position.X + GameData.TileSize / 2) - image.Width / 2);
            int offsetY = (int)((position.Y + GameData.TileSize / 2) - image.Height / 2);
            Rectangle = new Rectangle(offsetX, offsetY, TileWidth, TileHeight);


        }
        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Vector2(Rectangle.X, Rectangle.Y), null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

        }
    }
    public class HumanTile : AnimatedTile
    {
        public float Scale { get; set; }
        private float BubbleScale { get; set; }

        public int PositionY { get; set; }
        //timer
        public Timer TalkTimer { get; set; }
        public bool Win { get; set; }
        public Vector2 CagePosition { get; set; }

        public HumanTile(Texture2D image, Vector2 position, Rectangle? drawRectangle, List<Texture2D> textures, int? tilesize) : base(image, position, drawRectangle, textures, tilesize)
        {
            Scale = 1.5f;
            BubbleScale = 0.7f;
            PositionY = (int)position.Y;
            int offsetY = (int)(position.Y + GameData.TileSize - image.Height * Scale);
            Rectangle = new Rectangle((int)position.X, offsetY, TileWidth, TileHeight);

            //cage position
            var x = Rectangle.X - 30;
            var y = Rectangle.Y + image.Height * Scale - GameData.ImageSprites["cage"].Height+1;
            CagePosition = new Vector2(x, y);

            Win = false;
            TalkTimer = new Timer(2f);
           
        }
        public new void Draw(SpriteBatch spriteBatch)
        {
            //human
            spriteBatch.Draw(Texture, new Vector2(Rectangle.X, Rectangle.Y - 13), null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.FlipHorizontally, 0f);
            //cage

            spriteBatch.Draw(GameData.ImageSprites["cage"], CagePosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            //bubble
            if (TalkTimer.Wait)
            {

                var x = Rectangle.X- GameData.ImageSprites["bubbleWin"].Width*BubbleScale;
                var y = Rectangle.Y - GameData.ImageSprites["bubbleWin"].Height * BubbleScale;
                Vector2 position = new Vector2(x, y);
                if (Win)
                {
                    spriteBatch.Draw(GameData.ImageSprites["bubbleWin"], position, null, Color.White, 0f, Vector2.Zero, BubbleScale, SpriteEffects.None, 0f);
                }
                else
                {
                    spriteBatch.Draw(GameData.ImageSprites["bubbleFail"], position, null, Color.White, 0f, Vector2.Zero, BubbleScale, SpriteEffects.None, 0f);
                }
            }
           

        }

        public void Update()
        {
            Animate();
            TalkTimer.Update();

        }


    }
    public class InstructionTile : Tile
    {
        public float Scale { get; set; }

        public InstructionTile(Texture2D image, Vector2 position, Rectangle? drawRectangle, int? tilesize) : base(image, position, drawRectangle, tilesize)
        {
            Scale = 0.8f;

            int offsetY = (int)((position.Y + 1.2f*GameData.TileSize) - image.Height*Scale);
            Rectangle = new Rectangle((int)position.X, offsetY, (int)(TileWidth * Scale), (int)(TileHeight * Scale));

        }
        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Vector2(Rectangle.X, Rectangle.Y), null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);

        }

    }
}
