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
        public MainChar(string a_path, Vector2 a_position, Vector2 a_dimension, Vector2 a_frames) : base(a_path, a_position, a_dimension, a_frames)
        {
            Health = 5.0f;
            HealthMax = Health;
            Stamina = 10.0f;
            StaminaMax = Stamina;

            Speed = 4.5f;
            JumpSpeed = 7.0f;
            MaxJump = 120.0f;

            FrameAnimations = false;
            //CurrentAnimation = 0;
            //FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 0), 4, 66, 0, "Walk"));
            //FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 0), 1, 66, 0, "Stand"));
        }


        public void NormalAttack(Vector2 a_offset)
        {
            if (Stamina > 0)
            {
                Stamina--;
                GameGlobals.PassProjectile(new CurlyLine(new Vector2(Position.X, Position.Y - (Dimension.Y / 2)), this, new Vector2(Globals.MyMouse.NewMousePos.X, Globals.MyMouse.NewMousePos.Y + (Dimension.Y / 2)) - a_offset));
            }
        }

        public void MoveLeft()
        {
            Position = new Vector2(Position.X - Speed, Position.Y);
        }

        public void MoveRight()
        {
            Position = new Vector2(Position.X + Speed, Position.Y);
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

        public override void Update(Vector2 a_offset, List<Terrain> a_terrains)
        {
            bool checkScroll = false;
            if (Globals.MyKeyboard.GetPress("A"))
            {
                checkScroll = true;
                MoveLeft();
            }

            if (Globals.MyKeyboard.GetPress("D"))
            {
                checkScroll = true;
                MoveRight();
            }

            if (Globals.MyKeyboard.GetNewPress("Space"))
            {
                if (Jumping == false && OnTerrain(a_terrains).Item2 == true)
                {
                    InitialYPos = Position.Y;
                    Jumping = true;
                }
            }

            _staminaTimer.UpdateTimer();
            if (_staminaTimer.Test())
            {
                RechargeStamina(1);
                _staminaTimer.Reset();
            }

            if (checkScroll)
            {
                GameGlobals.CheckScroll(Position);
                SetAnimationByName("Stand");
            }
            else
            {
                SetAnimationByName("Stand");
            }

            //Rotation = Globals.RotateTowards(Position, Globals.NewVector(Globals.MyMouse.NewMousePos) - a_offset);

            if (Globals.MyMouse.LeftClick())
            {
                NormalAttack(a_offset);
            }

            if (Globals.MyMouse.RightClick())
            {
                GameGlobals.PassNpc(new FirstEnemy(Globals.NewVector(Globals.MyMouse.NewMousePos) - a_offset, new Vector2(1,1)));
            }

            base.Update(a_offset, a_terrains);
        }

        public override void Draw(Vector2 a_offset)
        {
            base.Draw(a_offset);
        }
    }
}
