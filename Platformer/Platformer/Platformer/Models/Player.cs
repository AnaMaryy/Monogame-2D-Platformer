using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Platformer.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Platformer.Models
{
    public class Player
    {
        //resources
        public Dictionary<string, List<Texture2D>> Animations { get; set; }
        public Dictionary<string, List<Texture2D>> RunParticles { get; set; }
        public Dictionary<string, SoundEffect> SoundEffects { get; set; }
        //TODO: fix the player size -> resize the images and load the new ones

        //player setup
        private SpriteBatch _spriteBatch { get; set; }
        private ContentManager Content { get; set; }
        public Texture2D Texture { get; set; } //the ones that actually get rendered every update
        public Rectangle Rectangle { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public float Scale { get; set; }

        //player movement
        public int Speed { get; set; }
        public int SpeedY { get; set; }
        public Vector2 Direction { get; set; }
        public float Gravity { get; set; }

        public int JumpSpeed { get; set; }//jumping

        //player animation
        public float FrameIndex { get; set; } //pick one of the animation frames from a list
        public float AnimationSpeed { get; set; }//how fast the animations updates

        //player status
        public string PlayerStatus { get; set; }
        public Boolean FacingRight { get; set; }
        public Boolean OnGround { get; set; }
        public Boolean OnCeiling { get; set; }
        public Boolean OnLeft { get; set; }
        public Boolean OnRight { get; set; }

        //dust animation
        public float DustFrameIndex { get; set; }
        public float DustAnimationSpeed { get; set; }


        public Player(Vector2 position, ContentManager content, SpriteBatch spriteBatch)
        {
            //Rectangle= new Rectangle((int)Position.X, (int)Position.Y, (int)(Width * Scale), (int)(Height * Scale));
            Content = content;
            _spriteBatch = spriteBatch;

            Direction = new Vector2(0, 0);
            Gravity = 0.8f;
            JumpSpeed = -17;
            Speed = 5;
            SpeedY = 1;
            Scale = 1.5f;
            Width = 32;
            Height = 28;
            //Vector2 Position = new Vector2(position.X + (GameData.TileSize / 2) - Math.Abs(GameData.TileSize / 2 - Texture.Width * Scale), position.Y + (GameData.TileSize - Texture.Height * Scale));
            Vector2 Position = new Vector2(position.X + (GameData.TileSize / 2) - Math.Abs(GameData.TileSize / 2 - Width * Scale), position.Y + (GameData.TileSize - Height * Scale));

            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)(Width * Scale), (int)(Height * Scale));


            FrameIndex = 0f;
            Texture = Animations["idle1"][0];
            PlayerStatus = "idle1";
            AnimationSpeed = 0.20f;
            //Texture = content.Load<Texture2D>("../../../Content/dog/sample");

            FacingRight = true;
            OnGround = false;
            OnCeiling = false;
            OnLeft = false;
            OnRight = false;

            //dust particles
            DustFrameIndex = 0;
            DustAnimationSpeed = 0.25f;
            //JumpDustParticles = new Particles(position, (int)(Width * Scale), (int)(Height * Scale), "jump", Content);





        }



    }
}