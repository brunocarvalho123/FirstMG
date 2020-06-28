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
        public enum State
        {
            STANDING,
            JUMPING,
            RUNNING,
            ATTACKING,
            DYING,
            HURTING,
            JUMP_ATTACKING,
            DASHING
        }

        /* Floats */
        private float _health;
        private float _healthMax;
        private float _stamina;
        private float _staminaMax;

        private float _hitDist;

        private float _hSpeed;
        private float _vSpeed;
        private float _movSpeed;
        private float _jumpSpeed;

        /* Booleans */
        private bool _dead;
        private bool _onGround;
        private bool _ignoringPhysics;

        private float _maxVSpeed; 
        private float _maxHSpeed;

        private Vector4 _boundingBoxOffset;

        private GameGlobals.Orientation _orientation = GameGlobals.Orientation.RIGHT;

        public Unit(string a_path, Vector2 a_position, Vector2 a_dimension, Vector2 a_frames) : base(a_path, a_position, a_dimension, a_frames, Color.White)
        {
            _health     = 1.0f;
            _healthMax  = _health;
            _stamina    = 0;
            _staminaMax = _stamina;
            _hitDist    = 50.0f;

            _maxVSpeed  = 49.9f;
            _maxHSpeed  = 6.0f;

            _boundingBoxOffset = new Vector4(1, 1, 1, 1);

            _dead            = false;
            _onGround        = false;
            _ignoringPhysics = false;
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
        public bool OnGround
        {
            get { return _onGround; }
            protected set { _onGround = value; }
        }
        public bool IgnoringPhysics
        {
            get { return _ignoringPhysics; }
            protected set { _ignoringPhysics = value; }
        }
        public float HSpeed
        {
            get { return _hSpeed; }
            protected set { _hSpeed = value; }
        }
        public float VSpeed
        {
            get { return _vSpeed; }
            protected set { _vSpeed = value; }
        }
        public float MaxHSpeed
        {
            get { return _maxHSpeed; }
            protected set { _maxHSpeed = value; }
        }
        public float MaxVSpeed
        {
            get { return _maxVSpeed; }
            protected set { _maxVSpeed = value; }
        }
        public float MovSpeed
        {
            get { return _movSpeed; }
            protected set { _movSpeed = value; }
        }
        public float JumpSpeed
        {
            get { return _jumpSpeed; }
            protected set { _jumpSpeed = value; }
        }
        public float Stamina
        {
            get { return _stamina; }
            set { _stamina = Math.Max(value,0); }
        }
        public float StaminaMax
        {
            get { return _staminaMax; }
            protected set { _staminaMax = value; }
        }
        public float BottomPosition
        {
            get { return Position.Y + Dimension.Y/2; }
        }
        public Vector4 BoundingBox
        {
            get {
                    return new Vector4(/* Left  X */ Position.X - (Dimension.X / 2) * BoundingBoxOffset.X,
                                       /* Right Y */ Position.X + (Dimension.X / 2) * BoundingBoxOffset.Y,
                                       /* Top   Z */ Position.Y - (Dimension.Y / 2) * BoundingBoxOffset.Z,
                                       /* Bot   W */ Position.Y + (Dimension.Y / 2) * BoundingBoxOffset.W);
            }
        }
        public Vector4 BoundingBoxOffset
        {
            get { return _boundingBoxOffset; }
            protected set { _boundingBoxOffset = value; }
        }
        public GameGlobals.Orientation Ori
        {
            get { return _orientation; }
            protected set { _orientation = value; }
        }

        public void Die(object a_obj)
        {
            Dead = true;
        }

        public virtual void GetHit(float a_damage)
        {
            Health -= a_damage;
            if (Health <= 0)
            {
                Dead = true;
            }
        }

        public virtual void MoveLeft(SquareGrid a_grid, Vector4 a_boundingBox, List<GridLocation> a_leftSlots)
        {
            if (!IgnoringPhysics)
            {
                if (OnGround) HSpeed = Math.Min(0, HSpeed + a_grid.Friction);
                else HSpeed = Math.Min(0, HSpeed + a_grid.Friction / 2);
            }

            foreach (GridLocation leftSlot in a_leftSlots)
            {
                if (leftSlot != null)
                {
                    float slotLeftPos = leftSlot.Position.X + a_grid.SlotDimensions.X;
                    if (a_boundingBox.X + HSpeed < slotLeftPos)
                    {
                        if (leftSlot.Impassible)
                        {
                            HSpeed = -(a_boundingBox.X - slotLeftPos);
                            HSpeed = Math.Min(HSpeed, 0);
                            break;
                        }
                        else if (leftSlot.Deadly)
                        {
                            Dead = true;
                            break;
                        }
                    }
                }
            }

            if (!IgnoringPhysics) HSpeed = Math.Max(HSpeed, -MaxHSpeed);
        }

        public virtual void MoveRight(SquareGrid a_grid, Vector4 a_boundingBox, List<GridLocation> a_rightSlots)
        {
            if (!IgnoringPhysics)
            {
                if (OnGround) HSpeed = Math.Max(0, HSpeed - a_grid.Friction);
                else HSpeed = Math.Max(0, HSpeed - a_grid.Friction * 0.8f);
            }

            foreach (GridLocation rightSlot in a_rightSlots)
            {
                if (rightSlot != null)
                {
                    float slotRightPos = rightSlot.Position.X;
                    if (a_boundingBox.Y + HSpeed - slotRightPos > 0)
                    {
                        if (rightSlot.Impassible)
                        {
                            HSpeed = slotRightPos - a_boundingBox.Y;
                            HSpeed = Math.Max(HSpeed, 0);
                            break;
                        }
                        else if (rightSlot.Deadly)
                        {
                            Dead = true;
                            break;
                        }
                    }
                }
            }

            if (!IgnoringPhysics) HSpeed = Math.Min(HSpeed, MaxHSpeed);
        }

        public virtual void Jump(SquareGrid a_grid, Vector4 a_boundingBox, List<GridLocation> a_topSlots)
        {
            foreach (GridLocation topSlot in a_topSlots)
            {
                if (topSlot != null)
                {
                    float topSlotPos = topSlot.Position.Y + a_grid.SlotDimensions.Y;
                    if (a_boundingBox.Z + VSpeed <= topSlotPos)
                    {
                        if (topSlot.Impassible)
                        {
                            VSpeed = Math.Min(0,(topSlotPos - a_boundingBox.Z));
                            return;
                        }
                        else if (topSlot.Deadly)
                        {
                            Dead = true;
                            return;
                        }
                    }
                }
            }
        }

        public virtual bool IsOnGround(SquareGrid a_grid, Vector4 a_boundingBox, List<GridLocation> a_botSlots)
        {
            foreach (GridLocation botSlot in a_botSlots)
            {
                if (botSlot != null &&
                    botSlot.Impassible &&
                    Math.Abs(botSlot.Position.Y - a_boundingBox.W) < 0.11f)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual void GravityEffect(SquareGrid a_grid, Vector4 a_boundingBox, List<GridLocation> a_botSlots)
        {
            if (OnGround && VSpeed > 0)
            {
                VSpeed = 0;
                return;
            }
            VSpeed += a_grid.Gravity;


            foreach (GridLocation botSlot in a_botSlots)
            {
                if (botSlot != null &&
                    botSlot.Position.Y - (a_boundingBox.W + VSpeed) < 0)
                {
                    if (botSlot.Impassible)
                    {
                        VSpeed = Math.Abs(botSlot.Position.Y - a_boundingBox.W) - 0.1f;
                        break;
                    }
                    else if (botSlot.Deadly)
                    {
                        Dead = true;
                        break;
                    }
                    
                }
            }

            VSpeed = Math.Min(VSpeed, MaxVSpeed);
        }

        public virtual void Update(Vector2 a_offset, SquareGrid a_grid)
        {
            if (HSpeed > 0)
            {
                Ori = GameGlobals.Orientation.RIGHT;
            }
            else if (HSpeed < 0)
            {
                Ori = GameGlobals.Orientation.LEFT;
            }

            Vector4 boundingBox = BoundingBox;

            List<GridLocation> botSlots   = a_grid.GetBotSlots(boundingBox);

            OnGround = IsOnGround(a_grid, boundingBox, botSlots);

            if (VSpeed < 0)
            {
                List<GridLocation> topSlots = a_grid.GetTopSlots(boundingBox);
                Jump(a_grid, boundingBox, topSlots);
            }
            if (!IgnoringPhysics) GravityEffect(a_grid, boundingBox, botSlots);

            Position = new Vector2(Position.X, Position.Y + VSpeed);

            boundingBox.Z = Position.Y - (Dimension.Y / 2) * BoundingBoxOffset.Z;
            boundingBox.W = Position.Y + (Dimension.Y / 2) * BoundingBoxOffset.W;
            if (HSpeed < 0)
            {
                List<GridLocation> leftSlots = a_grid.GetLeftSlots(boundingBox);
                MoveLeft(a_grid, boundingBox, leftSlots);
            }
            else if (HSpeed > 0)
            {
                List<GridLocation> rightSlots = a_grid.GetRightSlots(boundingBox);
                MoveRight(a_grid, boundingBox, rightSlots);
            }

            if (Position.X + HSpeed < a_grid.TotalPhysicalDims.X && Position.X + HSpeed > a_grid.StartingPhysicalPos.X)
            {
                Position = new Vector2(Position.X + HSpeed, Position.Y);
            }

            if (Position.Y >= a_grid.TotalPhysicalDims.Y)
            {
                Dead = true;
            }

            base.Update(a_offset);
        }

        public override void Draw(Vector2 a_offset)
        {
            //Globals.NormalEffect.Parameters["xSize"].SetValue((float)Asset.Bounds.Width);
            //Globals.NormalEffect.Parameters["ySize"].SetValue((float)Asset.Bounds.Height);
            //Globals.NormalEffect.Parameters["xDraw"].SetValue((float)((int)Dimension.X));
            //Globals.NormalEffect.Parameters["yDraw"].SetValue((float)((int)Dimension.Y));
            //Globals.NormalEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
            //Globals.NormalEffect.CurrentTechnique.Passes[0].Apply();

            base.Draw(a_offset);
        }
    }
}
