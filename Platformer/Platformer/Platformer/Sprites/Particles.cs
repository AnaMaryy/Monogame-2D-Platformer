using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Platformer.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Platformer.Sprites
{
    //takes care of the logic for particle animation
    public class Particles : IDisposable
    {

        public float FrameIndex { get; set; } //pick one of the animation frames from a list
        public float AnimationSpeed { get; set; }//how fast the animations updates
        public List<Texture2D> AnimationFrames { get; set; }
        public Texture2D CurrentImage { get; set; }
        private ContentManager Content { get; set; }
        private Rectangle Rectangle { get; set; }
        //width and height of player
        private int Width { get; set; }
        private int Height { get; set; }


        public Particles(Vector2 position, int width, int height, string type, ContentManager content)
        {
            Rectangle = new Rectangle((int)position.X, (int)position.Y, width, height);
            Content = content;
            FrameIndex = 0f;
            AnimationSpeed = 0.6f;
            Width = width;
            Height = height;

            if (type == "jump")
                AnimationFrames = GameData.AnimatedSprites["dog/dust/jump"];
            if (type == "fall")
                AnimationFrames = GameData.AnimatedSprites["dog/dust/fall"];

            CurrentImage = AnimationFrames[(int)FrameIndex];

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(CurrentImage, new Vector2(Rectangle.X, Rectangle.Y), Color.White);
        }
        public void Dispose()// Public implementation of Dispose pattern callable by consumers.
        {
            //Dispose(true);
            //also set the class to null after
            //this = null;
            GC.SuppressFinalize(this);
        }
        public void Animate()
        {
            FrameIndex += AnimationSpeed;
            if (FrameIndex >= AnimationFrames.Count)
            {
                FrameIndex = 0;
            }
            else
            {
                CurrentImage = AnimationFrames[(int)FrameIndex];
            }
        }

        public void Update(int x_shift)
        {
            Animate();
            //when the world shifts also the animation changes position
            Rectangle = new Rectangle(Rectangle.X + x_shift, Rectangle.Y, Width, Height);

        }
        //TODO  one loop animation, draw method, layer das najvisji, potem pa se update za world shift
        //v glavnem filu od igrce bos mela dicrionary z temi one loop animacijami npr za particle
        //v playerju imas dict one loop animacije za razne stvari itd....
        // isto v enemy
    }
}
