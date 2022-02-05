using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Platformer.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
namespace Platformer.Models
{
    public class Camera
    {
        public Matrix CameraMatrix { get; private set; }
        public Vector2 CenterPosition { get; set; }

        
        public void Follow(Player target)
        {
            var position = Matrix.CreateTranslation(
              -target.Rectangle.X - (target.Width / 2),
              -target.Rectangle.Y - (target.Height / 2),
              0);

            var offset = Matrix.CreateTranslation(
                GameData.LevelScreenWidth / 2,
                GameData.LevelScreenHeight / 2,
                0);

            CameraMatrix = position * offset;
            CenterPosition = new Vector2(target.Rectangle.X - (target.Width / 2), target.Rectangle.Y - (target.Height / 2));
        }
    }
}
