using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Platformer.GUI;
using Platformer.Sprites;
using Platformer.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Platformer.Models
{
    public class Player
    {
       
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
        public Particles JumpDustParticles { get; private set; }

        //Health of player
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public bool Immortal { get; private set; }
        public int ImmortalDuration { get; private set; }
        public int HurtTime { get; private set; }

        //android input gui
#if ANDROID 
        public AndroidGui AndroidGui { get; set; }
#endif
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
            Texture = GameData.AnimatedSprites["dog/idle1"][0];
            PlayerStatus = "dog/idle1";
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

            //gameplay rules
            MaxHealth = 5;
            CurrentHealth = 5;
            Immortal = false;
            ImmortalDuration = (int)(60 * 1.5f);//immortal for 1.5 s
            HurtTime = 0;
#if ANDROID
            //andorid gui
            AndroidGui = new AndroidGui();
#endif
        }

        public void gravity()
        {
            Direction = new Vector2(Direction.X, (int)Math.Round(Direction.Y + Gravity));
            Rectangle = new Rectangle(Rectangle.X, (int)(Rectangle.Y + Direction.Y * SpeedY), (int)(Width * Scale), (int)(Height * Scale));
        }
        public void jump()
        {
            Direction = new Vector2(Direction.X, JumpSpeed);
            GameData.SoundEffects["jump"].Play(volume: PlayerStats.SoundEffectsVolume, 0.0f, 0.0f);
        }
        public void RunDustAnimation()
        {
            if (PlayerStatus == "dog/run" && OnGround)
            {
                DustFrameIndex += DustAnimationSpeed;
                if (DustFrameIndex >= GameData.AnimatedSprites["dog/dust/run"].Count)
                {
                    DustFrameIndex = 0;
                }


                Texture2D dustParticleTexture = GameData.AnimatedSprites["dog/dust/run"][(int)DustFrameIndex];
                //draw the animation

                //player moving to the left or right
                if (FacingRight)
                {
                    //bottomleft
                    Vector2 dustPostion = new Vector2(Rectangle.X, Rectangle.Y + Height) - new Vector2(30, 10);
                    _spriteBatch.Draw(dustParticleTexture, dustPostion, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
                }
                else
                {   //bottomright  -offset
                    Vector2 dustPostion = new Vector2(Rectangle.X + Width, Rectangle.Y + Height) - new Vector2(20, 10);
                    _spriteBatch.Draw(dustParticleTexture, dustPostion, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.FlipHorizontally, 0f);

                }
            }
        }
        public void getStatus() //return status of player, used for animations
        {
            if (Direction.Y < 0)
            {
                PlayerStatus = "dog/jump";
            }
            else if (Direction.Y > 1)//falls at a certain speed; Direction.Y > Gravity
            {
                PlayerStatus = "dog/fall";
            }
            else
            {
                if (Direction.X != 0)
                {
                    PlayerStatus = "dog/run";
                }
                else
                {
                    PlayerStatus = "dog/idle1";
                }
            }
#if DESKTOP
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && OnGround)
            {
                PlayerStatus = "dog/sniff";
            }
#elif ANDROID
            if (AndroidGui.DownKey && OnGround)
            {
                PlayerStatus = "dog/sniff";
            }
#endif
        }
        public void Animate()
        {
            List<Texture2D> animation = GameData.AnimatedSprites[PlayerStatus];

            //loop over the frame index
            FrameIndex += AnimationSpeed;
            if (FrameIndex >= animation.Count)
                FrameIndex = 0f;


            Texture = animation[(int)FrameIndex];

        }

#if ANDROID
        public void getAndroidInput()
        {
            
            //left, right movement
            if (AndroidGui.LeftKey)
            {
                Direction = new Vector2(-1, Direction.Y);
                FacingRight = false;
            }
            else if (AndroidGui.RightKey)
            {
                Direction = new Vector2(1, Direction.Y);
                FacingRight = true;
            }
            else
            {
                Direction = new Vector2(0, Direction.Y);
            }
            //shiff -> cant move
            if (AndroidGui.DownKey && OnGround)
            {
                Direction = new Vector2(0, 0);
            }

            //jump 
            if (AndroidGui.UpKey && OnGround)
            {
                jump();
            }
        }
#endif
        public void getInput()
        {
            //left, right movement
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                Direction = new Vector2(-1, Direction.Y);
                FacingRight = false;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                Direction = new Vector2(1, Direction.Y);
                FacingRight = true;
            }
            else
            {
                Direction = new Vector2(0, Direction.Y);
            }
            //shiff -> cant move
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && OnGround)
            {
                Direction = new Vector2(0, 0);
            }

            //jump 
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && OnGround)
            {
                jump();
            }
        }

        public void takeDamage()
        {
            if (!Immortal)
            {
                Immortal = true;
                HurtTime = 1;
                changeHealth(-1);
            }
        }
        private void ImmortalTimer()// counts down the tier
        {
            if (Immortal)
            {
                HurtTime++;
                if (HurtTime >= ImmortalDuration)
                {
                    HurtTime = 0;
                    Immortal = false;
                }
            }
        }
        private int cosWave()
        {
            if (Immortal)
            {
                double value = Math.Cos(HurtTime);
                if (value < 0)
                {
                    return 0;
                }
            }
            return 1;

        }
        public void AddHealth()
        {
            //check if health is at max
            if (!(CurrentHealth >= MaxHealth))
            {
                CurrentHealth += 1;
            }
        }
        private void changeHealth(int health)
        {
            CurrentHealth += health;
            GameData.SoundEffects["hit"].Play(volume: PlayerStats.SoundEffectsVolume, 0.0f, 0.0f);

        }
        public void Update()
        {
#if DESKTOP
            getInput();
#elif ANDROID
            getAndroidInput();
#endif
            getStatus();
            Animate();
            
            ImmortalTimer();
            //Rectangle = new Rectangle((int)(Rectangle.X + Direction.X * Speed), Rectangle.Y,(int)(Width*Scale), (int)(Height*Scale));

        }
        public void Draw()
        {
            RunDustAnimation();
            int opacity = cosWave();
            if (FacingRight)
            {
                _spriteBatch.Draw(Texture, new Vector2(Rectangle.X, Rectangle.Y), null, Color.White * opacity, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
            }
            else
            {
                _spriteBatch.Draw(Texture, new Vector2(Rectangle.X, Rectangle.Y), null, Color.White * opacity, 0f, Vector2.Zero, Scale, SpriteEffects.FlipHorizontally, 0f);
            }

        }
    }
}