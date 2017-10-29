using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4
{
    class Controls
    {
        static KeyboardState keyboardState, previousKeyboardState;
        static MouseState mouseState, previousMouseState;
        public Vector2 Speed = Vector2.Zero;
        public static Vector2 MouseDirection = Vector2.Zero;

        const int Up = -1;
        const int Down = 1;
        const int Left = -1;
        const int Right = 1;
        

        public static Vector2 mousePosition {  get{ return new Vector2(mouseState.X, mouseState.Y); } }

        public static void Update()
        {
            previousKeyboardState = keyboardState;
            previousMouseState = mouseState;

            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
        }

        public static bool WasKeyPressed(Keys key)
        {
            return previousKeyboardState.IsKeyUp(key) && keyboardState.IsKeyDown(key);
        }
        public static bool WasMousePressed()
        {
            return mouseState.RightButton == ButtonState.Pressed;
        }
        public static Vector2 MovementDirectionLR()
        {
            Vector2 Direction = Vector2.Zero;
            if (keyboardState.IsKeyDown(Keys.A) == true)
                Direction.X += Left;
            if (keyboardState.IsKeyDown(Keys.D) == true)
                Direction.X += Right;
            //if (keyboardState.IsKeyDown(Keys.W) == true)
            //    Direction.Y += Up;
            //if (keyboardState.IsKeyDown(Keys.S) == true)
            //    Direction.Y += Down;


            if (Direction.LengthSquared() > 1)
                Direction.Normalize();

            return Direction;
        }
        public static Vector2 MovementDirectionUD()
        {
            //if (keyboardState.IsKeyDown(Keys.A) == true)
            //    Direction.X += Left;
            //if (keyboardState.IsKeyDown(Keys.D) == true)
            //    Direction.X += Right;
            if (keyboardState.IsKeyDown(Keys.W) == true)
                Player.DirectionU.Y += Up;

            if (Player.DirectionU.LengthSquared() > 1)
                Player.DirectionU.Normalize();

            return Player.DirectionU;
        }

        public static Vector2 GetAimDirection()
        {
            return GetMouseAimDirection();
        }
        private static Vector2 GetMouseAimDirection()
        {
            Vector2 direction = mousePosition - Player.Instance.Position;

            if (direction == Vector2.Zero)
                return Vector2.Zero;
            else
                return Vector2.Normalize(direction);
        }
        //public static bool BoolJump()
        //{
        //    return (keyboardState.IsKeyDown(Keys.W) == true && keyboardState.IsKeyDown(Keys.W) == false);
        //}
        //public static Vector2 Jump()
        //{
        //    Vector2 Direction = Vector2.Zero;
        //    if(keyboardState.IsKeyDown(Keys.W) == true && keyboardState.IsKeyDown(Keys.W) == false)
        //        Direction.Y += Up;
        //    if (Direction.LengthSquared() > 1)
        //        Direction.Normalize();
        //    return Direction;
        //}
        //public static Vector2 Fall()
        //{
        //    Vector2 Direction = Vector2.Zero;
        //    if (keyboardState.IsKeyDown(Keys.W) == true && keyboardState.IsKeyDown(Keys.W) == false)
        //        Direction.Y += Down;
        //    if (Direction.LengthSquared() > 1)
        //        Direction.Normalize();
        //    return Direction;
        //}
        //public static Vector2 MouseAimDirection()
        //{
        //    if (WasMousePressed())
        //    {
        //        Vector2 direction = mousePosition - Player.Instance.Position;
        //    }

        //    if (MouseDirection == Vector2.Zero && WasMousePressed())
        //        return Vector2.Zero;
        //    else
        //        return Vector2.Normalize(MouseDirection);
        //}

    }
}
