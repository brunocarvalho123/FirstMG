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
        private TimeSpan _jumpTimer = TimeSpan.FromMilliseconds(151);
        private Engine.MyTimer _staminaTimer = new Engine.MyTimer(1000);
        private bool _wasOnGround = false;
        private TimeSpan _extraGroundTimer = TimeSpan.FromMilliseconds(76);

        public MainChar(string a_path, Vector2 a_position, Vector2 a_dimension, Vector2 a_frames) : base(a_path, a_position, a_dimension, a_frames)
        {
            Health     = 5.0f;
            HealthMax  = Health;
            Stamina    = 10.0f;
            StaminaMax = Stamina;

            MovSpeed  = 1.1f;
            JumpSpeed = 15.0f;


            FrameAnimations = false;
            CurrentAnimation = 0;
            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 0), 1, 20, 0, "Idle"));
            //FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 1), 6, 128, 0, "Run"));
            //FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 3), 3, 64, 0, "Jump"));
            //FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 2), 3, 64, 0, "Fall"));
        }


        public void NormalAttack(Vector2 a_offset)
        {
            //if (Stamina > 0)
            //{
            //    Stamina--;
            //    GameGlobals.PassProjectile(new CurlyLine(new Vector2(Position.X, Position.Y - (Dimension.Y / 2)), this, new Vector2(Globals.MyMouse.NewMousePos.X, Globals.MyMouse.NewMousePos.Y + (Dimension.Y / 2)) - a_offset));
            //}
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

        public override void Update(Vector2 a_offset, SquareGrid a_grid)
        {
            bool checkScroll = false;
            //a_grid.GetSlotFromPixel(Position, Vector2.Zero).Filled = false;
            bool pressedSpace = Globals.MyKeyboard.GetPress("Space");

            if (VSpeed != 0)
            {
                checkScroll = true;
            }

            if (Globals.MyKeyboard.GetPress("A") || Globals.MyKeyboard.GetPress("Left"))
            {
                HSpeed -= MovSpeed;
            }

            if (Globals.MyKeyboard.GetPress("D") || Globals.MyKeyboard.GetPress("Right"))
            {
                HSpeed += MovSpeed;
            }


            if (Globals.MyKeyboard.GetNewPress("Space"))
            {
                _jumpTimer = TimeSpan.FromMilliseconds(0);
            }
            if (!pressedSpace && _wasOnGround && !OnGround)
            {
                _extraGroundTimer = TimeSpan.FromMilliseconds(0);
            }

            _jumpTimer        += Globals.GlobalGameTime.ElapsedGameTime;
            _extraGroundTimer += Globals.GlobalGameTime.ElapsedGameTime;

            if (_jumpTimer.TotalMilliseconds < 150 && (_extraGroundTimer.TotalMilliseconds < 75 || OnGround)) 
            {
                VSpeed = -JumpSpeed;
            }
            if (!pressedSpace && VSpeed < 0 && !OnGround)
            {
                VSpeed *= 0.8f;
            }

            _staminaTimer.UpdateTimer();
            if (_staminaTimer.Test())
            {
                RechargeStamina(1);
                _staminaTimer.Reset();
            }

            if (OnGround && HSpeed == 0)
            {
                //SetAnimationByName("Idle");
            }
            else if (OnGround && HSpeed != 0)
            {
                checkScroll = true;
                //SetAnimationByName("Run");
            }
            else if (VSpeed > 0)
            {
                //SetAnimationByName("Fall");
            }
            else if (VSpeed < 0)
            {
                //SetAnimationByName("Jump");
            }

            if (Globals.MyMouse.LeftClick())
            {
                NormalAttack(a_offset);
            }

            if (Globals.MyMouse.RightClick())
            {
                Vector2 tmpLocation = a_grid.GetLocationFromPixel(Globals.NewVector(Globals.MyMouse.NewMousePos) - a_offset, Vector2.Zero);

                GridLocation location = a_grid.GetSlotFromLocation(tmpLocation);

                if (location != null && !location.Filled && !location.Impassible)
                {
                    location.SetToFilled(true);
                    FirstEnemy tmpEnemy = new FirstEnemy(Vector2.Zero, new Vector2(1, 1));

                    tmpEnemy.Position = location.Position + tmpEnemy.Dimension/2;

                    GameGlobals.PassNpc(tmpEnemy);
                }
            }

            _wasOnGround = OnGround;

            base.Update(a_offset, a_grid);
            
            if (checkScroll)
            {
                GameGlobals.CheckScroll(Position);
            }
            //a_grid.GetSlotFromPixel(Position, Vector2.Zero).Filled = true;
        }

        public override void Draw(Vector2 a_offset)
        {
            base.Draw(a_offset);
        }
    }
}
