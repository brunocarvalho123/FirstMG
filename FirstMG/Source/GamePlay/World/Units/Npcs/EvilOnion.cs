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
    class EvilOnion : Npc
    {

        private MyTimer _moveTimer    = new MyTimer(800);
        private MyTimer _shooterTimer = new MyTimer(800);

        public EvilOnion(Vector2 a_position, Vector2 a_frames) 
            : base("Assets\\evil_onion", a_position, new Vector2(60,60), a_frames)
        {
            MovSpeed = 2f;
            MaxHSpeed = 3f;
        }

        public override void Update(Vector2 a_offset, MainChar a_mainChar, SquareGrid a_grid)
        {
            a_grid.GetSlotFromPixel(Position, Vector2.Zero).Filled = false;

            _shooterTimer.UpdateTimer();
            if (_shooterTimer.Test())
            {
                if (Globals.GetDistance(Position, a_mainChar.Position) < 500)
                {
                    //GameGlobals.PassProjectile(new CurlyLine(new Vector2(Position.X, Position.Y - (Dimension.Y / 2)), this, new Vector2(a_mainChar.Position.X, a_mainChar.Position.Y)));
                }
                _shooterTimer.Reset();
            }
            

            _moveTimer.UpdateTimer();
            if (_moveTimer.Test())
            {
                MovSpeed = -MovSpeed;
                _moveTimer.Reset();
            }

            //HSpeed += MovSpeed;

            base.Update(a_offset, a_mainChar, a_grid);

            if (Globals.GetDistance(Position, a_mainChar.Position) < 45)
            {
                a_mainChar.GetHit(1);
            }

            a_grid.GetSlotFromPixel(Position, Vector2.Zero).Filled = true;
        }

        public override void Draw(Vector2 a_offset)
        {
            base.Draw(a_offset);
        }
    }
}
