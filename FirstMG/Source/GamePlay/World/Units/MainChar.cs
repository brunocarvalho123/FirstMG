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
        enum State
        {
            STANDING,
            JUMPING,
            RUNNING,
            ATTACKING
        }
        enum Orientation
        {
            RIGHT,
            LEFT
        }

        private TimeSpan _jumpTimer        = TimeSpan.FromMilliseconds(151);
        private TimeSpan _extraGroundTimer = TimeSpan.FromMilliseconds(76);
        private MyTimer  _staminaTimer     = new MyTimer(1000);
        private bool     _wasOnGround      = false;

        private State       _state       = State.STANDING;
        private Orientation _orientation = Orientation.RIGHT;

        public MainChar(string a_path, Vector2 a_position, Vector2 a_dimension, Vector2 a_frames) : base(a_path, a_position, a_dimension, a_frames)
        {
            Health     = 5.0f;
            HealthMax  = Health;
            Stamina    = 10.0f;
            StaminaMax = Stamina;

            MovSpeed  = 1.1f;
            MaxHSpeed = 8.0f;
            JumpSpeed = 28.0f;

            BoundingBoxOffset = new Vector4(.25f, .25f, .6f, 1);

            FrameAnimations = true;
            CurrentAnimation = 0;
            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 0), 3, 384, 0, "IdleR"));
            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 1), 3, 384, 0, "IdleL"));
            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 2), 6, 128, 0, "RunR"));
            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 3), 6, 128, 0, "RunL"));
            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 4), 4, 128, 1, "JumpR"));
            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 5), 4, 128, 1, "JumpL"));
            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 6), 2, 128, 0, "FallR"));
            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 7), 2, 128, 0, "FallL"));

            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 8), 5, 150, 1, 3, NormalAttack, "AttR"));
            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 9), 5, 150, 1, 3, NormalAttack, "AttL"));
        }

        public void NormalAttack(object a_obj)
        {
            if (Stamina > 0)
            {
                Stamina--;
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

        public void HandleInput(Vector2 a_offset, SquareGrid a_grid)
        {
            bool pressedSpace = Globals.MyKeyboard.GetPress("Space");

            if (OnGround && HSpeed == 0 && _state != State.ATTACKING) _state = State.STANDING;
            if (_state != State.ATTACKING)
            {
                // Runing stuff
                if (Globals.MyKeyboard.GetPress("A") || Globals.MyKeyboard.GetPress("Left"))
                {
                    HSpeed -= MovSpeed;
                    _state = State.RUNNING;
                }
                if (Globals.MyKeyboard.GetPress("D") || Globals.MyKeyboard.GetPress("Right"))
                {
                    HSpeed += MovSpeed;
                    _state = State.RUNNING;
                }

                // Jumping stuff
                if (Globals.MyKeyboard.GetNewPress("Space"))
                {
                    _jumpTimer = TimeSpan.FromMilliseconds(0);
                }
                if (!pressedSpace && _wasOnGround && !OnGround)
                {
                    _extraGroundTimer = TimeSpan.FromMilliseconds(0);
                }

                _jumpTimer += Globals.GlobalGameTime.ElapsedGameTime;
                _extraGroundTimer += Globals.GlobalGameTime.ElapsedGameTime;

                if (_jumpTimer.TotalMilliseconds < 150 && (_extraGroundTimer.TotalMilliseconds < 75 || OnGround))
                {
                    VSpeed = -JumpSpeed;
                    _jumpTimer = TimeSpan.FromMilliseconds(151);
                    _state = State.JUMPING;
                }
                if (!pressedSpace && VSpeed < 0 && !OnGround)
                {
                    VSpeed *= 0.8f;
                }

                if (Globals.MyMouse.LeftClick())
                {
                    if (OnGround)
                    {
                        _state = State.ATTACKING;
                    }
                }
            }

            if (Globals.MyMouse.RightClick())
            {
                Vector2 tmpLocation = a_grid.GetLocationFromPixel(Globals.NewVector(Globals.MyMouse.NewMousePos) - a_offset, Vector2.Zero);

                GridLocation location = a_grid.GetSlotFromLocation(tmpLocation);

                if (location != null && !location.Filled && !location.Impassible)
                {
                    location.SetToFilled(true);
                    FirstEnemy tmpEnemy = new FirstEnemy(Vector2.Zero, new Vector2(1, 1));

                    tmpEnemy.Position = location.Position + tmpEnemy.Dimension / 2;

                    GameGlobals.PassNpc(tmpEnemy);
                }
            }
        }

        public void StartAnimation()
        {
            string ori = "R";
            if (_orientation == Orientation.LEFT) ori = "L";

            if (VSpeed > 0 && !OnGround)
            {
                SetAnimationByName("Fall" + ori);
                return;
            }
            else if (VSpeed < 0 && !OnGround)
            {
                SetAnimationByName("Jump" + ori);
                return;
            }

            switch (_state)
            {
                case State.ATTACKING:
                    SetAnimationByName("Att" + ori);
                    if (FrameAnimationList[CurrentAnimation].IsAtEnd()) _state = State.STANDING;
                    break;
                case State.RUNNING:
                    SetAnimationByName("Run" + ori);
                    break;
                case State.STANDING:
                    SetAnimationByName("Idle" + ori);
                    break;
                default:
                    SetAnimationByName("Idle" + ori);
                    break;
            }
        }

        public override void Update(Vector2 a_offset, SquareGrid a_grid)
        {
            //a_grid.GetSlotFromPixel(Position, Vector2.Zero).Filled = false;

            bool checkScroll = false;

            HandleInput(a_offset, a_grid);
            StartAnimation();

            _staminaTimer.UpdateTimer();
            if (_staminaTimer.Test())
            {
                RechargeStamina(1);
                _staminaTimer.Reset();
            }

            _wasOnGround = OnGround;

            if (HSpeed > 0)
            {
                _orientation = Orientation.RIGHT;
                checkScroll = true;
            }
            else if (HSpeed < 0)
            {
                _orientation = Orientation.LEFT;
                checkScroll = true;
            }

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
