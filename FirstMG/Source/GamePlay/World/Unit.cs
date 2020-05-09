using FirstMG.Source.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMG.Source.GamePlay
{
    class Unit : Asset2D
    {
        /* Floats */
        private float _floorDist;
        private float _health;
        private float _healthMax;
        private float _hitDist;
        private float _jumpSpeed;
        private float _maxJump; //TODO: Remove this implement proper acceleration
        private float _speed;
        private float _stamina;
        private float _staminaMax;

        /* Booleans */
        private bool _dead;
        private bool _jumping;

        public Unit(string a_path, Vector2 a_position, Vector2 a_dimension) : base(a_path, a_position, a_dimension)
        {
            _floorDist  = 0.0f;
            _health     = 1.0f;
            _healthMax  = _health;
            _stamina    = 0;
            _staminaMax = _stamina;
            _hitDist    = 50.0f;
            _jumpSpeed  = 5.0f;
            _maxJump    = 50.0f;
            _speed      = 2.0f;

            _dead      = false;
            _jumping   = false;
        }

        public float FloorDistance
        {
            get { return _floorDist; }
            protected set { _floorDist = value; }
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
        public float MaxJump
        {
            get { return _maxJump; }
            protected set { _maxJump = value; }
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
        public float Speed
        {
            get { return _speed; }
            protected set { _speed = value; }
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
            if (FloorDistance < MaxJump)
            {
                FloorDistance += _jumpSpeed;
                Position = new Vector2(Position.X, GameGlobals.FloorLevel - FloorDistance);
            }
            else
            {
                Jumping = false;
            }
        }

        public virtual void GravityEffect()
        {
            if (FloorDistance > 0)
            {
                FloorDistance -= _jumpSpeed;
                if (FloorDistance < 0) FloorDistance = 0;
                Position = new Vector2(Position.X, GameGlobals.FloorLevel - FloorDistance);
            } 
        }

        public override void Update(Vector2 a_offset)
        {
            if (Jumping == true)
            {
                Jump();
            } 
            else
            {
                GravityEffect();
            }
            base.Update(a_offset);
        }

        public override void Draw(Vector2 a_offset)
        {
            Engine.Globals.NormalEffect.Parameters["xSize"].SetValue((float)Asset.Bounds.Width);
            Engine.Globals.NormalEffect.Parameters["ySize"].SetValue((float)Asset.Bounds.Height);
            Engine.Globals.NormalEffect.Parameters["xDraw"].SetValue((float)((int)Dimension.X));
            Engine.Globals.NormalEffect.Parameters["yDraw"].SetValue((float)((int)Dimension.Y));
            Engine.Globals.NormalEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
            Engine.Globals.NormalEffect.CurrentTechnique.Passes[0].Apply();

            base.Draw(a_offset);
        }
    }
}
