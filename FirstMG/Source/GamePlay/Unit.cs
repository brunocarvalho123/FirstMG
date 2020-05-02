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
        private float _hitDist;
        private bool _dead;

        public Unit(string a_path, Vector2 a_position, Vector2 a_dimension) : base(a_path, a_position, a_dimension)
        {
            _speed = 2.0f;
            _dead = false;
            _hitDist = 50.0f;
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
        public bool Dead
        {
            get { return _dead; }
            protected set { _dead = value; }
        }

        public virtual void GetHit()
        {
            _dead = true;
        }

        public override void Update(Vector2 a_offset)
        {
            base.Update(a_offset);
        }

        public override void Draw(Vector2 a_offset)
        {
            base.Draw(a_offset);
        }
    }
}
