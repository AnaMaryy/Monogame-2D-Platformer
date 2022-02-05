﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Platformer.Models
{
    public abstract class Enemy
    {
        public Rectangle Rectangle { get; set; }
        public float FrameIndex { get; set; }
        public float AnimationSpeed { get; set; }

        public Dictionary<string, List<Texture2D>> Textures { get; private set; }
        public Texture2D Texture { get; set; }
        public float Scale { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string State { get; set; }
        public int OffsetY { get; set; }
        //Health of enemy
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public bool Immortal { get; private set; }
        public int ImmortalDuration { get; private set; }
        public int HurtTime { get; private set; }

        public string Type { get; set; }//type of enemy -> used for casting
        public Enemy(Vector2 position, Dictionary<string, List<Texture2D>> textures, float scale, string type)
        {
            State = textures.Keys.First();
            Height = textures[State][0].Height;
            Width = textures[State][0].Width;
            Type = type;

            //offset
            OffsetY = (int)(position.Y + GameData.TileSize - (int)(Height * scale));
            Rectangle = new Rectangle((int)position.X, OffsetY, (int)(Width * scale), (int)(Height * scale));

            //rectangle width and height -> differes froma actual sprites for collision
            FrameIndex = 0f;
            Scale = 0.8f;
            AnimationSpeed = 0.12f;
            Textures = textures;
            Texture = textures[State][0];

            //health
            MaxHealth = 1;
            CurrentHealth = 1;
            Immortal = false;
            ImmortalDuration = (int)(60 * 1.5f);//immortal for 1.5 s
            HurtTime = 0;
        }
        public void Animate()
        {
            List<Texture2D> animation = Textures[State];
            //loop over the frame index
            FrameIndex += AnimationSpeed;
            if (FrameIndex >= animation.Count)
                FrameIndex = 0f;


            Texture = animation[(int)FrameIndex];

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
        public void ImmortalTimer()// counts down the tier
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
        public int cosWave()
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
        public void changeHealth(int health)
        {
            CurrentHealth += health;
        }
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update( Vector2 playerPosition);

    }
    public class HorizontalEnemy : Enemy
    {
        public int Speed { get; private set; }
        public HorizontalEnemy(Vector2 position, Dictionary<string, List<Texture2D>> textures, float scale, string type) : base(position, textures, scale, type)
        {


            Random rnd = new Random();
            Speed = rnd.Next(3, 6);
        }
        private void Move()
        {
            Rectangle = new Rectangle(Rectangle.X + Speed, Rectangle.Y, (int)(Width * Scale), (int)(Height * Scale));

        }
        public void ReverseDirection()
        {
            Speed = Speed * -1;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Speed > 0)// moving to the rigt
            {
                spriteBatch.Draw(Texture, new Vector2(Rectangle.X, Rectangle.Y), null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(Texture, new Vector2(Rectangle.X, Rectangle.Y), null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.FlipHorizontally, 0f);

            }
        }
        public override void Update(Vector2 playerPosition)
        {
            Animate();
            Move();

        }
    }

    public class VerticalEnemy : Enemy // jumps
    {
        public int JumpSpeed { get; private set; }
        public int StartingY { get; set; }
        public Vector2 Direction { get; set; }
        public int EndingY { get; set; }
        public float Gravity { get; set; }
        public VerticalEnemy(Vector2 position, Dictionary<string, List<Texture2D>> textures, float scale, string type) : base(position, textures, scale, type)
        {
            Random rnd = new Random();
            JumpSpeed = rnd.Next(2, 5);
            Trace.WriteLine("jumpSpeed" + JumpSpeed);
            Gravity = 0.8f;
            State = "jump";
            StartingY = (int)position.Y;
            Direction = new Vector2(0, 0);
            EndingY = Rectangle.Y - rnd.Next(1, 3) * GameData.TileSize;
        }
        private void getState() //switches the states
        {
            if (State == "jump" && Rectangle.Y <= EndingY)
            {
                State = "fall";

            }
            else if (State == "fall" && Rectangle.Y >= StartingY)
            {
                State = "jump";
            }
        }
        //TODO : better jumping
        private void gravity()
        {

            Direction = new Vector2(Direction.X, (int)Math.Round(Direction.Y + Gravity));
            Rectangle = new Rectangle(Rectangle.X, (int)(Rectangle.Y + Direction.Y), (int)(Width * Scale), (int)(Height * Scale));

        }
        private void Jump()
        {
            if (State == "jump")
            {
                Direction = new Vector2(Direction.X, -JumpSpeed);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(Texture, new Vector2(Rectangle.X, Rectangle.Y), null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.FlipHorizontally, 0f);

        }
        public override void Update( Vector2 playerPosition)
        {
            getState();
            gravity();
            Jump();
            Rectangle = new Rectangle(Rectangle.X , (int)(Rectangle.Y + Direction.Y) , (int)(Texture.Width * Scale), (int)(Texture.Height * Scale));
            Animate();

        }
    }

    public class EnemyHealthGui // made with primitives
    {

    }
}