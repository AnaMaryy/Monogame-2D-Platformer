using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platformer.Sprites
{
    public class OneLoopAnimation
    {
        public float FrameIndex { get; set; } //pick one of the animation frames from a list
        public float AnimationSpeed { get; set; }//how fast the animations updates
        public List<Texture2D> AnimationFrames { get; set; }
        public Texture2D CurrentImage { get; set; }
        private Rectangle Rectangle { get; set; }
        //width and height of player
        private int Width { get; set; }
        private int Height { get; set; }
        private float Scale { get; set; }
        public bool FinishAnimation { get; set; }
        public OneLoopAnimation(List<Texture2D> animationFrames, Vector2 position, float scale)
        {

            FrameIndex = 0f;
            AnimationFrames = animationFrames;
            CurrentImage = AnimationFrames[(int)FrameIndex];
            Rectangle = new Rectangle((int)position.X, (int)position.Y, CurrentImage.Width, CurrentImage.Height);
            AnimationSpeed = 0.6f;
            Width = CurrentImage.Width;
            Height = CurrentImage.Height;
            Scale = scale;
            FinishAnimation = false;

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(CurrentImage, new Vector2(Rectangle.X, Rectangle.Y), null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);

        }
        public void Animate()
        {
            FrameIndex += AnimationSpeed;
            if (FrameIndex >= AnimationFrames.Count)
            {
                FinishAnimation = true;
            }
            else
            {
                CurrentImage = AnimationFrames[(int)FrameIndex];
            }
        }
        public void Update()
        {
            Animate();
        }
    }
}
