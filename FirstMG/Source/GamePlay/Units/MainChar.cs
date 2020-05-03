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
        public MainChar(string a_path, Vector2 a_position, Vector2 a_dimension) : base(a_path, a_position, a_dimension)
        {
            Speed = 3.0f;
            MaxJump = 80;
        }

        public override void Update(Vector2 a_offset)
        {
            bool checkScroll = false;
            if (Globals.MyKeyboard.GetPress("A"))
            {
                checkScroll = true;
                Position = new Vector2(Position.X - Speed, Position.Y);
            }

            if (Globals.MyKeyboard.GetPress("D"))
            {
                checkScroll = true;
                Position = new Vector2(Position.X + Speed, Position.Y);
            }

            if (Globals.MyKeyboard.GetNewPress("Space"))
            {
                if (Jumping == false && FloorDistance <= 0)
                {
                    Jumping = true;
                }
            }

            if (checkScroll) GameGlobals.CheckScroll(Position);

            //Rotation = Globals.RotateTowards(Position, Globals.NewVector(Globals.MyMouse.NewMousePos) - a_offset);

            if (Globals.MyMouse.LeftClick())
            {
                
                GameGlobals.PassProjectile(new CurlyLine(new Vector2(Position.X, Position.Y-(Dimension.Y/2) ), this, new Vector2(Globals.MyMouse.NewMousePos.X, Globals.MyMouse.NewMousePos.Y + (Dimension.Y / 2)) - a_offset));
            }

            if (Globals.MyMouse.RightClick())
            {
                GameGlobals.PassNpc(new FirstEnemy(Globals.NewVector(Globals.MyMouse.NewMousePos) - a_offset));
            }

            base.Update(a_offset);
        }

        public override void Draw(Vector2 a_offset)
        {
            base.Draw(a_offset);
        }
    }
}
