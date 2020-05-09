using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FirstMG.Source.GamePlay
{
    class Projectile : Engine.Asset2D
    {
        private float          _speed;
        private bool           _done;
        private Vector2        _direction;
        private Unit           _owner;
        private Engine.MyTimer _timer;

        public Projectile(string a_path, Vector2 a_position, Vector2 a_dimension, Unit a_owner, Vector2 a_target) 
            : base(a_path, a_position, a_dimension)
        {
            _speed = 2.0f;
            _done  = false;
            _owner = a_owner;

            _direction = a_target - a_owner.Position;
            _direction.Normalize();

            Rotation = Engine.Globals.RotateTowards(Position, Engine.Globals.NewVector(a_target));

            _timer = new Engine.MyTimer(2000);
        }

        public bool Done
        {
            get { return _done; }
        }
        public float Speed
        {
            get { return _speed; }
            protected set { _speed = value; }
        }

        public virtual void Update(Vector2 a_offset, List<Unit> a_units)
        {
            Position += _direction * _speed;

            _timer.UpdateTimer();
            if (_timer.Test())
            {
                _done = true;
            }
            
            if (HitSomething(a_units))
            {
                _done = true;
            }
        }

        public virtual bool HitSomething(List<Unit> a_units)
        {
            foreach (Unit unit in a_units)
            {
                if (Engine.Globals.GetDistance(Position,unit.Position) < unit.HitDistance)
                {
                    unit.GetHit(1);
                    return true;
                }
            }
            return false;
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
