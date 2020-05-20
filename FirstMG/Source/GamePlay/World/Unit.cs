using FirstMG.Source.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMG.Source.GamePlay
{
    class Unit : Animated2D
    {
        /* Floats */
        private float _health;
        private float _healthMax;
        private float _stamina;
        private float _staminaMax;

        private float _hitDist;

        private float _hSpeed;
        private float _vSpeed;
        private float _jumpSpeed;

        /* Booleans */
        private bool _dead;
        private bool _jumping;
        private bool _onGround;

        public Unit(string a_path, Vector2 a_position, Vector2 a_dimension, Vector2 a_frames) : base(a_path, a_position, a_dimension, a_frames, Color.White)
        {
            _health     = 1.0f;
            _healthMax  = _health;
            _stamina    = 0;
            _staminaMax = _stamina;
            _hitDist    = 50.0f;
            _vSpeed     = 5.0f;
            _hSpeed     = 2.0f;

            _dead      = false;
            _jumping   = false;
            _onGround  = true;
        }

        public float Health
        {
            get { return _health; }
            set { _health = value; }
        }
        public float HealthMax
        {
            get { return _healthMax; }
            protected set { _healthMax = value; }
        }
        public float HitDistance
        {
            get { return _hitDist; }
            protected set { _hitDist = value; }
        }
        public bool Dead
        {
            get { return _dead; }
            protected set { _dead = value; }
        }
        public bool Jumping
        {
            get { return _jumping; }
            protected set { _jumping = value; }
        }
        public bool OnGround
        {
            get { return _onGround; }
            protected set { _onGround = value; }
        }
        public float VSpeed
        {
            get { return _vSpeed; }
            protected set { _vSpeed = value; }
        }
        public float JumpSpeed
        {
            get { return _jumpSpeed; }
            protected set { _jumpSpeed = value; }
        }
        public float Speed
        {
            get { return _hSpeed; }
            protected set { _hSpeed = value; }
        }
        public float Stamina
        {
            get { return _stamina; }
            set { _stamina = value; }
        }
        public float StaminaMax
        {
            get { return _staminaMax; }
            protected set { _staminaMax = value; }
        }
        public float PositionYOffseted
        {
            get { return Position.Y + Dimension.Y/2; }
        }


        public virtual void GetHit(float a_damage)
        {
            Health -= a_damage;
            if (Health <= 0)
            {
                Dead = true;
            }
        }

        public virtual void Jump()
        {
            VSpeed = -JumpSpeed;
        }

        public virtual bool IsOnGround(SquareGrid a_grid, GridLocation a_slotBelowLeft, GridLocation a_slotBelowRight)
        {
            bool impassibleLeft = false;
            if(a_slotBelowLeft == null)
            {
                impassibleLeft = false;
            } 
            else if (Math.Abs(a_slotBelowLeft.Position.Y - PositionYOffseted) > 0.11f)
            {
                impassibleLeft = false;
            }
            else
            {
                impassibleLeft = a_slotBelowLeft.Impassible;
            }

            bool impassibleRight = false;
            if (a_slotBelowRight == null)
            {
                impassibleRight = false;
            }
            else if (Math.Abs(a_slotBelowRight.Position.Y - PositionYOffseted) > 0.11f)
            {
                impassibleRight = false;
            }
            else
            {
                impassibleRight = a_slotBelowRight.Impassible;
            }

            return impassibleLeft || impassibleRight;
        }

        public virtual void GravityEffect(SquareGrid a_grid, GridLocation a_slotBelowLeft, GridLocation a_slotBelowRight)
        {
            if (OnGround)
            {
                VSpeed = 0;
            }
            else
            {
                VSpeed += a_grid.Gravity;
                if (a_slotBelowLeft != null && (a_slotBelowLeft.Impassible || a_slotBelowRight.Impassible) && a_slotBelowLeft.Position.Y - (PositionYOffseted + VSpeed) < 0)
                {
                    VSpeed = Math.Abs(a_slotBelowLeft.Position.Y - PositionYOffseted) - 0.1f;
                }
            }

            if (VSpeed > GameGlobals.maxVSpeed)
            {
                VSpeed = GameGlobals.maxVSpeed;
            }
        }

        public virtual void Update(Vector2 a_offset, SquareGrid a_grid)
        {
            GridLocation slotBelowLeft = a_grid.GetSlotBelow(a_grid.GetLocationFromPixel(Position + new Vector2(-Dimension.Y/2, Dimension.Y/2), Vector2.Zero));
            GridLocation slotBelowRight = a_grid.GetSlotBelow(a_grid.GetLocationFromPixel(Position + new Vector2(Dimension.Y/2, Dimension.Y/2), Vector2.Zero));
            OnGround = IsOnGround(a_grid, slotBelowLeft, slotBelowRight);

            if (Jumping == true)
            {
                Jump();
                Jumping = false;
            }
            else
            {
                GravityEffect(a_grid, slotBelowLeft, slotBelowRight);
            }

            if (VSpeed != 0)
            {
                Position = new Vector2(Position.X, Position.Y + VSpeed);
                if (Position.Y >= Globals.ScreenHeight)
                {
                    Dead = true;
                }
            }

            base.Update(a_offset);
        }

        public override void Draw(Vector2 a_offset)
        {
            Globals.NormalEffect.Parameters["xSize"].SetValue((float)Asset.Bounds.Width);
            Globals.NormalEffect.Parameters["ySize"].SetValue((float)Asset.Bounds.Height);
            Globals.NormalEffect.Parameters["xDraw"].SetValue((float)((int)Dimension.X));
            Globals.NormalEffect.Parameters["yDraw"].SetValue((float)((int)Dimension.Y));
            Globals.NormalEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
            Globals.NormalEffect.CurrentTechnique.Passes[0].Apply();

            if (Dead)
            {
                base.Draw(a_offset, Color.Red);
            } else
            {
                base.Draw(a_offset);
            }
        }
    }
}
