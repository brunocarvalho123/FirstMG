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
    class MainChar : Engine.Asset2D
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

            //Rotation = Globals.RotateTowards(Position, new Vector2(Globals.MyMouse.NewMousePos.X, Globals.MyMouse.NewMousePos.Y));

            base.Update();
        }

        public override void Draw(Vector2 a_offset)
        {
            base.Draw(a_offset);
        }
    }
}
