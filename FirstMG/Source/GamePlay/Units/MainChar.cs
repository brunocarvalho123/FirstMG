#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMG.Source.Engine;
using Microsoft.Xna.Framework;
#endregion

namespace FirstMG.Source.GamePlay
{
    class MainChar : Unit
    {

        private float _speed;

        public MainChar(string a_path, Vector2 a_position, Vector2 a_dimension) : base(a_path, a_position, a_dimension)
        {
            _speed = 2.0f;
        }

        public override void Update()
        {
            if (Globals.MyKeyboard.GetPress("A"))
            {
                Position = new Vector2(Position.X - _speed, Position.Y);
            }

            if (Globals.MyKeyboard.GetPress("D"))
            {
                Position = new Vector2(Position.X + _speed, Position.Y);
            }

            //Rotation = Globals.RotateTowards(Position, Globals.NewVector(Globals.MyMouse.NewMousePos));

            if (Globals.MyMouse.LeftClick())
            {
                GameGlobals.PassProjectile(new CurlyLine(new Vector2(Position.X, Position.Y-(Dimension.Y/2) ), this, Globals.NewVector(Globals.MyMouse.NewMousePos)));
            }

            base.Update();
        }

        public override void Draw(Vector2 a_offset)
        {
            base.Draw(a_offset);
        }
    }
}
