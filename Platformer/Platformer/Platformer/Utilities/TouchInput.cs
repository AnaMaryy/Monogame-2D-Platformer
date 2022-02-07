using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platformer.Utilities
{
    //class for handling touch input -Y > for android
    public static class TouchInput
    {
        //check if the touch is interesting with target Rectangle -> like a button
        public static bool CheckTouch(Rectangle target, TouchCollection touchCollection)
        {
            if (touchCollection.Count > 0)
            {
                foreach (var touch in touchCollection)
                {
                    var scaledTouchPosition = Vector2.Transform(touch.Position, Matrix.Invert(GameData.MenuScaleMatrix));

                    if (target.Contains(scaledTouchPosition))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        //checks the touch input of the game gui buttons
        public static bool CheckTouchGameGui(Rectangle target, TouchCollection touchCollection)
        {
            if (touchCollection.Count > 0)
            {
                foreach (var touch in touchCollection)
                {
                    var scaledTouchPosition = Vector2.Transform(touch.Position, Matrix.Invert(GameData.CameraMatrix *GameData.LevelScaleMatrix));

                    if (target.Contains(scaledTouchPosition))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        //used for slider -> also returns the position of the touch
        public static Vector2? CheckTouchSlider(Rectangle target, TouchCollection touchCollection)
        {
            if (touchCollection.Count > 0)
            {
                foreach (var touch in touchCollection)
                {
                    var scaledTouchPosition = Vector2.Transform(touch.Position, Matrix.Invert(GameData.MenuScaleMatrix));

                    if (target.Contains(scaledTouchPosition))
                    {
                        return scaledTouchPosition;
                    }
                }
            }
            return null;
        }
    }
}
