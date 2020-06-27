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
            ATTACKING,
            JUMP_ATTACKING,
            DASHING
        }

        private TimeSpan _jumpTimer         = TimeSpan.FromMilliseconds(151);
        private TimeSpan _extraGroundTimer  = TimeSpan.FromMilliseconds(76);
        private MyTimer  _staminaTimer      = new MyTimer(250);
        private MyTimer  _invulnerableTimer = new MyTimer(1500);
        private MyTimer  _swapTimer         = new MyTimer(3000);

        private bool     _wasOnGround       = false;
        private bool     _invulnerable      = false;

        private State       _state       = State.STANDING;

        Npc _swappedEnemy = null;

        public MainChar(string a_path, Vector2 a_position, Vector2 a_dimension, Vector2 a_frames) : base(a_path, a_position, a_dimension, a_frames)
        {
            Health     = 5.0f;
            HealthMax  = Health;
            Stamina    = 5.0f;
            StaminaMax = Stamina;

            MovSpeed  = 1.1f;
            MaxHSpeed = 7.5f;
            JumpSpeed = 26.1f;

            Ori = GameGlobals.Orientation.RIGHT;

            BoundingBoxOffset = new Vector4(.25f, .25f, .6f, .9f);

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

            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 8), 6, 64, 1, 4, NormalAttack, "AttR"));
            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 9), 6, 64, 1, 4, NormalAttack, "AttL"));

            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 10), 2, 100, 0, "JattR"));
            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 11), 2, 100, 0, "JattL"));

            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 12), 4, 45, 1, 4, StopDashing, "DashR"));
            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 13), 4, 45, 1, 4, StopDashing, "DashL"));
        }

        public MyTimer SwapTimer
        {
            get { return _swapTimer; }
        }
        public bool Swapping
        {
            get { return (_swappedEnemy != null && !_swappedEnemy.Dead); }
        }

        public void NormalAttack(object a_obj)
        {
            if (Stamina >= 1)
            {
                GameGlobals.ExecuteAttack(new Attack(this, 60, 1, Ori));
                Stamina--;
            }
        }

        public void StopDashing(object a_obj)
        {
            _state = State.STANDING;
            IgnoringPhysics = false;
        }

        public override void GetHit(float a_damage)
        {
            if (!_invulnerable && _state != State.DASHING)
            {
                base.GetHit(a_damage);
                //VSpeed = -50;
                //HSpeed = -50;
                // TODO: Make a decente knockback system
                _invulnerable = true;
                _invulnerableTimer.Reset();
            }
        }

        public void SwapPositionWithClosestEnemy()
        {
            Npc closestEnemy = (Npc)GameGlobals.GetClosestNpc(this);

            if (closestEnemy != null && Math.Abs(closestEnemy.Position.X - Position.X) < 1000)
            {
                Vector2 tmpPos = Position;

                Position = closestEnemy.Position + new Vector2(0, closestEnemy.Dimension.Y/2 * closestEnemy.BoundingBoxOffset.W - Dimension.Y/2);
                closestEnemy.Position = tmpPos + new Vector2(0, Dimension.Y/2 * BoundingBoxOffset.W - closestEnemy.Dimension.Y/2);

                GameGlobals.PassEffect(new WindEffect(this, new Vector2(Dimension.Y * 1.5f, Dimension.Y * 1.5f)));
                GameGlobals.PassEffect(new WindEffect(closestEnemy, new Vector2(Dimension.Y * 1.5f, Dimension.Y * 1.5f)));

                Stamina -= 3;
                SwapTimer.ResetToZero();
                _swappedEnemy = closestEnemy;
            }
        }

        public void SwapBack()
        {
            if (Swapping)
            {
                Vector2 tmpPos = Position;
                Position = _swappedEnemy.Position + new Vector2(0, _swappedEnemy.Dimension.Y / 2 * _swappedEnemy.BoundingBoxOffset.W - Dimension.Y / 2);
                _swappedEnemy.Position = tmpPos + new Vector2(0, Dimension.Y / 2 * BoundingBoxOffset.W - _swappedEnemy.Dimension.Y / 2);

                GameGlobals.PassEffect(new WindEffect(this, new Vector2(Dimension.Y * 1.5f, Dimension.Y * 1.5f)));
                GameGlobals.PassEffect(new WindEffect(_swappedEnemy, new Vector2(Dimension.Y * 1.5f, Dimension.Y * 1.5f)));

                SwapTimer.ResetToZero();
                _swappedEnemy = null;
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

            if (OnGround && HSpeed == 0 && _state != State.ATTACKING && _state != State.JUMP_ATTACKING) _state = State.STANDING;

            if (_state != State.ATTACKING && _state != State.JUMP_ATTACKING)
            {
                // Runing stuff
                if (Globals.MyKeyboard.GetPress("A") || Globals.MyKeyboard.GetPress("Left"))
                {
                    HSpeed -= MovSpeed;
                }
                if (Globals.MyKeyboard.GetPress("D") || Globals.MyKeyboard.GetPress("Right"))
                {
                    HSpeed += MovSpeed;
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
                    if (OnGround && Stamina >= 1)
                    {
                        _state = State.ATTACKING;
                    }
                }

                if (!OnGround && Stamina >= 1 && (Globals.MyKeyboard.GetPress("S") || Globals.MyKeyboard.GetPress("Down")))
                {
                    if (VSpeed < 0) VSpeed = -0.1f;
                    Stamina--;
                    _state = State.JUMP_ATTACKING;
                }

                if (Stamina >= 3 && !Swapping && (Globals.MyKeyboard.GetPress("R")))
                {
                    SwapPositionWithClosestEnemy();
                }
            }
            else if (_state == State.JUMP_ATTACKING) 
            {
                if (Globals.MyKeyboard.GetPress("A") || Globals.MyKeyboard.GetPress("Left"))
                {
                    HSpeed -= MovSpeed;
                }
                if (Globals.MyKeyboard.GetPress("D") || Globals.MyKeyboard.GetPress("Right"))
                {
                    HSpeed += MovSpeed;
                }
                if (Globals.MyMouse.LeftClick())
                {
                    _state = State.JUMPING;
                }
            }

            if (Globals.MyMouse.RightClick())
            {
                Vector2 tmpLocation = a_grid.GetLocationFromPixel(Globals.NewVector(Globals.MyMouse.NewMousePos) - a_offset, Vector2.Zero);

                GridLocation location = a_grid.GetSlotFromLocation(tmpLocation);

                if (location != null && !location.Filled && !location.Impassible)
                {
                    //location.SetToFilled(true);
                    FirstEnemy tmpEnemy = new FirstEnemy(Vector2.Zero, new Vector2(1, 1));

                    tmpEnemy.Position = location.Position + tmpEnemy.Dimension / 2;

                    GameGlobals.PassNpc(tmpEnemy);
                }
            }

            if (Globals.MyKeyboard.GetPress("LeftShift") && _state != State.DASHING && Stamina >= 3)
            {
                _state = State.DASHING;
                IgnoringPhysics = true;
                VSpeed = 0;
                Stamina -= 3;
            }

            if(_state == State.STANDING && HSpeed != 0) _state = State.RUNNING;
        }

        public void StartAnimation()
        {
            string ori = "R";
            if (Ori == GameGlobals.Orientation.LEFT) ori = "L";

            if (_state == State.DASHING)
            {
                SetAnimationByName("Dash" + ori);
                return;
            }

            if (_state == State.JUMP_ATTACKING && !OnGround)
            {
                SetAnimationByName("Jatt" + ori);
                return;
            }

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
                    _state = State.STANDING;
                    SetAnimationByName("Idle" + ori);
                    break;
            }
        }

        public void UpdateTimers()
        {
            if (Swapping)
            {
                SwapTimer.UpdateTimer();
                if (SwapTimer.Test())
                {
                    SwapBack();
                }
            }

            _staminaTimer.UpdateTimer();
            if (_staminaTimer.Test())
            {
                RechargeStamina(0.2f);
                _staminaTimer.Reset();
            }

            if (_invulnerable)
            {
                _invulnerableTimer.UpdateTimer();
                if (_invulnerableTimer.Test())
                {
                    _invulnerable = false;
                }
            }
        }

        public override void Update(Vector2 a_offset, SquareGrid a_grid)
        {
            //a_grid.GetSlotFromPixel(Position, Vector2.Zero).Filled = false;

            Vector2 originalPosition = Position;

            if (_state == State.DASHING)
            {
                if (Ori == GameGlobals.Orientation.LEFT)
                {
                    HSpeed = -20.0f;
                }
                else
                {
                    HSpeed = 20.0f;
                }
            }
            else
            {
                HandleInput(a_offset, a_grid);
            }


            if (_state == State.JUMP_ATTACKING)
            {
                Attack jumpAttack = new Attack(this, 0, 1, GameGlobals.Orientation.BOT);
                GameGlobals.ExecuteAttack(jumpAttack);
                if (jumpAttack.LandedHit) VSpeed = -JumpSpeed*0.8f;
            }

            StartAnimation();

            UpdateTimers();

            _wasOnGround = OnGround;

            base.Update(a_offset, a_grid);
            
            GameGlobals.CheckScroll(Position);

            //a_grid.GetSlotFromPixel(Position, Vector2.Zero).Filled = true;
        }

        public override void Draw(Vector2 a_offset)
        {
            if (_invulnerable)
            {
                //Globals.NormalEffect.Parameters["xSize"].SetValue((float)Asset.Bounds.Width);
                //Globals.NormalEffect.Parameters["ySize"].SetValue((float)Asset.Bounds.Height);
                //Globals.NormalEffect.Parameters["xDraw"].SetValue((float)((int)Dimension.X));
                //Globals.NormalEffect.Parameters["yDraw"].SetValue((float)((int)Dimension.Y));
                Globals.NormalEffect.Parameters["filterColor"].SetValue(Color.Yellow.ToVector4());
                Globals.NormalEffect.CurrentTechnique.Passes[0].Apply();
            }
            base.Draw(a_offset);
        }
    }
}
