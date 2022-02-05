using Microsoft.Xna.Framework;


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Platformer.Physics
{
    public static class PhysicsEngine
    {

        public static string collisionDirection(Rectangle reactangle1, Vector2 Direction, Rectangle rectangle2) // returns string top,bottom, left,none , the first rectangle is touching the second one
        {
            //collided on the right of player
            if (
                   reactangle1.Right + Direction.X > rectangle2.Left &&
                   reactangle1.Left < rectangle2.Left &&
                   reactangle1.Bottom > rectangle2.Top &&
                   reactangle1.Top < rectangle2.Bottom)
            {
                return "right";
            }

            //collided on the left of the player
            else if (
                  reactangle1.Left + Direction.X < rectangle2.Right &&
                   reactangle1.Right > rectangle2.Right &&
                   reactangle1.Bottom > rectangle2.Top &&
                   reactangle1.Top < rectangle2.Bottom)
            {
                return "left";
            }

            //collided on the bottom of the player
            else if (
                  reactangle1.Bottom + Direction.Y > rectangle2.Top &&
                    reactangle1.Top < rectangle2.Top &&
                    reactangle1.Right > rectangle2.Left &&
                    reactangle1.Left < rectangle2.Right)
            {
                return "bottom";
            }
            //collided on the top of the player
            else if (

                 reactangle1.Top + Direction.Y < rectangle2.Bottom &&
                  reactangle1.Bottom > rectangle2.Bottom &&
                  reactangle1.Right > rectangle2.Left &&
                  reactangle1.Left < rectangle2.Right)
            {
                return "top";
            }
            return "none";

        }
    }
}
