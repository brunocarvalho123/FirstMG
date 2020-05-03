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
        private float _speed;
        private float _jumpSpeed;
        private float _hitDist;
        private float _maxJump;
        private float _floorDistance;
        private bool _jumping;
        private bool _dead;

        public Unit(string a_path, Vector2 a_position, Vector2 a_dimension) : base(a_path, a_position, a_dimension)
        {
            _speed = 2.0f;
            _jumpSpeed = 5.0f;
            _dead = false;
            _jumping = false;
            _hitDist = 50.0f;
            _maxJump = 50.0f;
            _floorDistance = 0.0f;
        }

        public float HitDistance
        {
            get { return _hitDist; }
            protected set { _hitDist = value; }
        }
        public float Speed
        {
            get { return _speed; }
            protected set { _speed = value; }
        }
        public float MaxJump
        {
            get { return _maxJump; }
            protected set { _maxJump = value; }
        }
        public float FloorDistance
        {
            get { return _floorDistance; }
            protected set { _floorDistance = value; }
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

        public virtual void GetHit()
        {
            _dead = true;
        }

        public virtual void Jump()
        {
            if (FloorDistance < MaxJump)
            {
                FloorDistance += (_jumpSpeed);
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
                FloorDistance -= (_jumpSpeed);
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
            base.Draw(a_offset);
        }
    }
}
