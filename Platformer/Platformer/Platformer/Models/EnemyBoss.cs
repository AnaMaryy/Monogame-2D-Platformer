using Platformer.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Platformer.Models
{
    public class EnemyBoss : Enemy
    {
        //enemy Boss is idle until the player is close enough -> starts to follow the player 
        public Vector2 Direction { get; set; }
        public bool Moving { get; set; }
        public bool Jumping { get; set; }
        public string JumpingState { get; set; } //jump or fall
        public int JumpSpeed { get; set; }
        public int MaxY { get; set; }//max Y value for jumping
        public int Speed { get; set; }
        public float Gravity { get; set; }
        public bool FacingRight { get; set; }
        public bool Stop { get; set; }
        public int MinY { get; set; } //will not go below the groundY
        public EnemyBoss(Vector2 position, Dictionary<string, List<Texture2D>> textures, float scale, string type) : base(position, textures, scale, type)
        {
            MaxHealth = 3;
            MaxY = (int)(position.Y - 1.5f * GameData.TileSize);
            //MinY=(int)(position.Y + GameData.TileSize - (int)(Height * scale));
            MinY = (int)(position.Y);

            CurrentHealth = 3;
            Speed = 3;
            Stop = false;
            Jumping = false;
            JumpSpeed = 4;
            Gravity = 1f;
            JumpingState = "jump";
            Moving = false;
            FacingRight = false;
            Direction = new Vector2(0, 0);
        }
        public void CheckPlayerProximity(Vector2 playerPosition)//changes the value of moving if player is close; 
        {
            int movingDistance = GameData.TileSize * 3;
            int jumpingDistance = GameData.TileSize * 2;
            int maxShootingDistance = GameData.TileSize * 6;
            int minShootingDistance = GameData.TileSize * 6;

            if ((Rectangle.X - movingDistance <= playerPosition.X && playerPosition.X <= Rectangle.X + movingDistance) //player in range
                && !(Rectangle.X - GameData.TileSize / 2 <= playerPosition.X && playerPosition.X <= Rectangle.X + GameData.TileSize / 2)) //player above enemy
            {
                Moving = true; //run
            }
            else
            {
                Moving = false; // idle
            }
            //jumping 
            if ((Rectangle.X - jumpingDistance <= playerPosition.X && playerPosition.X <= Rectangle.X + jumpingDistance))
            {
                Jumping = true;
            }
            else
            {
                Jumping = false;
            }
            //shooting


        }
        public void Alert() //the enemy "woke up" animation
        {

        }
        private void gravity()
        {
            if (Jumping && OffsetY > Rectangle.Y || !Jumping && OffsetY > Rectangle.Y)
            {
                Direction = new Vector2(Direction.X, (int)Math.Round(Direction.Y + Gravity));
                Rectangle = new Rectangle(Rectangle.X, (int)(Rectangle.Y + Direction.Y), (int)(Width * Scale), (int)(Height * Scale));

            }
            else if (!Jumping && Math.Abs(OffsetY - Rectangle.Y) <= 20) //if not jumping anymore move to the corrent position
            {
                Direction = new Vector2(Direction.X, 0);
                Rectangle = new Rectangle(Rectangle.X, OffsetY, (int)(Width * Scale), (int)(Height * Scale));
            }
            else
            {
                Direction = new Vector2(Direction.X, 0);
            }

        }
        public void MoveX(Vector2 playerPosition) //moves in the x direction of towards player
        {
            if (Moving && !Stop)
            {
                //where relative to boss is player? left or right?
                if (playerPosition.X < Rectangle.X)//left
                {
                    Direction = new Vector2(-1, Direction.Y);
                    FacingRight = false;
                }
                else if (playerPosition.X == Rectangle.X)
                {
                    Direction = new Vector2(0, Direction.Y);
                }
                else
                {
                    Direction = new Vector2(1, Direction.Y);
                    FacingRight = true;
                }
            }
            else
            {
                Direction = new Vector2(0, Direction.Y);

            }



        }
        private void getJumpingState() //switches the states
        {
            if (Jumping)
            {
                if (JumpingState == "jump" && Rectangle.Y <= MaxY)
                {
                    JumpingState = "fall";

                }
                else if (JumpingState == "fall" && Rectangle.Y >= MinY)
                {
                    JumpingState = "jump";
                }
            }


        }
        private void Jump()
        {
            if (JumpingState == "jump" && Jumping)
            {
                Direction = new Vector2(Direction.X, -JumpSpeed);
            }

        }


        public void stop(string direction, Rectangle tileRectangle)
        {
            Stop = true;
            if (direction == "right")
            {
                //move the enemy on the left side of the object
                //TODO : fix the animation and yess
                Rectangle = new Rectangle(tileRectangle.Left - (int)(Width * Scale) + 1, Rectangle.Y, (int)(Texture.Width * Scale), (int)(Texture.Height * Scale));
            }
            else if (direction == "left")
            {

                Rectangle = new Rectangle(tileRectangle.Right - 1, Rectangle.Y, (int)(Texture.Width * Scale), (int)(Texture.Height * Scale));
            }

        }
        public void Continue()
        {
            Stop = false;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            int opacity = cosWave();
            if (FacingRight)
            {
                spriteBatch.Draw(Texture, new Vector2(Rectangle.X, Rectangle.Y), null, Color.White * opacity, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(Texture, new Vector2(Rectangle.X, Rectangle.Y), null, Color.White * opacity, 0f, Vector2.Zero, Scale, SpriteEffects.FlipHorizontally, 0f);
            }
        }
        public override void Update( Vector2 playerPosition)
        {
            ImmortalTimer();
            CheckPlayerProximity(playerPosition);
            getJumpingState();
            Animate();

            gravity();
            MoveX(playerPosition);
            Jump();

            Rectangle = new Rectangle((int)(Rectangle.X + Direction.X * Speed) , (int)(Rectangle.Y + Direction.Y) , (int)(Texture.Width * Scale), (int)(Texture.Height * Scale));

        }
    }
}
