#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion


namespace FirstMG.Source.Engine
{

    public delegate void PassObject(object i);
    public delegate object PassObjectAndReturn(object i);

    class Globals
    {
        private static ContentManager _content;
        private static SpriteBatch _spriteBatch;
        private static Effect _normalEffect;
        private static Input.MyKeyboard _keyboard;
        private static Input.MyMouseControl _mouse;
        private static int _screenWidth;
        private static int _screenHeight;
        private static GameTime _gameTime;

        public static ContentManager MyContent
        {
            get { return _content; }
            set { _content = value; }
        }
        public static SpriteBatch MySpriteBatch
        {
            get { return _spriteBatch; }
            set { _spriteBatch = value; }
        }
        public static Effect NormalEffect
        {
            get { return _normalEffect; }
            set { _normalEffect = value; }
        }
        public static Input.MyKeyboard MyKeyboard
        {
            get { return _keyboard; }
            set { _keyboard = value; }
        }
        public static Input.MyMouseControl MyMouse
        {
            get { return _mouse; }
            set { _mouse = value; }
        }
        public static int ScreenWidth
        {
            get { return _screenWidth; }
            set { _screenWidth = value; }
        }
        public static  int ScreenHeight
        {
            get { return _screenHeight; }
            set { _screenHeight = value; }
        }
        public static GameTime GlobalGameTime
        {
            get { return _gameTime; }
            set { _gameTime = value; }
        }




        public static Vector2 NewVector(Vector2 a_vector2)
        {
            return new Vector2(a_vector2.X, a_vector2.Y);
        }

        public static float GetDistance(Vector2 a_pos, Vector2 a_target)
        {
            return (float)Math.Sqrt(Math.Pow(a_pos.X - a_target.X, 2) + Math.Pow(a_pos.Y - a_target.Y, 2));
        }

        public static Vector2 RadialMovement(Vector2 a_pos, Vector2 a_focus, float a_speed)
        {
            float distance = Globals.GetDistance(a_pos, a_focus);

            if (distance <= a_speed)
            {
                return a_focus - a_pos;
            } else
            {
                return (a_focus - a_pos) * (a_speed / distance);
            }
        }

        public static float RotateTowards(Vector2 a_pos, Vector2 a_focus)
        {

            float h, sineTheta, angle;
            if (a_pos.Y - a_focus.Y != 0)
            {
                h = (float)Math.Sqrt(Math.Pow(a_pos.X - a_focus.X, 2) + Math.Pow(a_pos.Y - a_focus.Y, 2));
                sineTheta = (float)(Math.Abs(a_pos.Y - a_focus.Y) / h); //* ((item.a_pos.Y-a_focus.Y)/(Math.Abs(item.a_pos.Y-a_focus.Y))));
            }
            else
            {
                h = a_pos.X - a_focus.X;
                sineTheta = 0;
            }

            angle = (float)Math.Asin(sineTheta);

            // Drawing diagonial lines here.
            //Quadrant 2
            if (a_pos.X - a_focus.X > 0 && a_pos.Y - a_focus.Y > 0)
            {
                angle = (float)(Math.PI * 3 / 2 + angle);
            }
            //Quadrant 3
            else if (a_pos.X - a_focus.X > 0 && a_pos.Y - a_focus.Y < 0)
            {
                angle = (float)(Math.PI * 3 / 2 - angle);
            }
            //Quadrant 1
            else if (a_pos.X - a_focus.X < 0 && a_pos.Y - a_focus.Y > 0)
            {
                angle = (float)(Math.PI / 2 - angle);
            }
            else if (a_pos.X - a_focus.X < 0 && a_pos.Y - a_focus.Y < 0)
            {
                angle = (float)(Math.PI / 2 + angle);
            }
            else if (a_pos.X - a_focus.X > 0 && a_pos.Y - a_focus.Y == 0)
            {
                angle = (float)Math.PI * 3 / 2;
            }
            else if (a_pos.X - a_focus.X < 0 && a_pos.Y - a_focus.Y == 0)
            {
                angle = (float)Math.PI / 2;
            }
            else if (a_pos.X - a_focus.X == 0 && a_pos.Y - a_focus.Y > 0)
            {
                angle = (float)0;
            }
            else if (a_pos.X - a_focus.X == 0 && a_pos.Y - a_focus.Y < 0)
            {
                angle = (float)Math.PI;
            }

            return angle;
        }
    }
}
