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
        private Engine.MyTimer _staminaTimer = new Engine.MyTimer(1000);
        public MainChar(string a_path, Vector2 a_position, Vector2 a_dimension) : base(a_path, a_position, a_dimension)
        {
            Health = 5.0f;
            HealthMax = Health;
            Stamina = 10.0f;
            StaminaMax = Stamina;

            Speed = 3.0f;
            MaxJump = 80;
        }


        public void NormalAttack(Vector2 a_offset)
        {
            if (Stamina > 0)
            {
                Stamina--;
                GameGlobals.PassProjectile(new CurlyLine(new Vector2(Position.X, Position.Y - (Dimension.Y / 2)), this, new Vector2(Globals.MyMouse.NewMousePos.X, Globals.MyMouse.NewMousePos.Y + (Dimension.Y / 2)) - a_offset));
            }
        }

        public void RechargeStamina (float a_value)
        {
            if (Stamina >= StaminaMax) return;

            float stDiff = StaminaMax - Stamina;
            if (a_value < stDiff)
            {
                Stamina += a_value;
            } else if (a_value >= stDiff)
            {
                Stamina += stDiff;
            }
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

            _staminaTimer.UpdateTimer();
            if (_staminaTimer.Test())
            {
                RechargeStamina(1);
                _staminaTimer.Reset();
            }

            if (checkScroll) GameGlobals.CheckScroll(Position);

            //Rotation = Globals.RotateTowards(Position, Globals.NewVector(Globals.MyMouse.NewMousePos) - a_offset);

            if (Globals.MyMouse.LeftClick())
            {
                NormalAttack(a_offset);
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
