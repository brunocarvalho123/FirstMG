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
        private float _floorDist;
        private float _health;
        private float _healthMax;
        private float _hitDist;
        private float _jumpSpeed;
        private float _initialYPos;
        private float _maxJump; //TODO: Remove this implement proper acceleration
        private float _speed;
        private float _stamina;
        private float _staminaMax;

        /* Booleans */
        private bool _dead;
        private bool _jumping;

        public Unit(string a_path, Vector2 a_position, Vector2 a_dimension, Vector2 a_frames) : base(a_path, a_position, a_dimension, a_frames, Color.White)
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
        public float InitialYPos
        {
            get { return _initialYPos; }
            protected set { _initialYPos = value; }
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
        public float JumpSpeed
        {
            get { return _jumpSpeed; }
            protected set { _jumpSpeed = value; }
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

        public virtual Tuple<Terrain,bool> OnTerrain(List<Terrain> a_terrains)
        {
            IEnumerable<Terrain> query = from terrain in a_terrains
                                         where Math.Abs(terrain.Position.X - Position.X) <= terrain.Dimension.X / 2 + Dimension.X / 2
                                         select terrain;

            Terrain closestTerrain = null;
            foreach (Terrain terrain in query)
            {
                if (terrain.Position.Y == Position.Y)
                {
                    return new Tuple<Terrain, bool>(terrain,true);
                }
                else if (closestTerrain == null ||  (terrain.Position.Y > Position.Y) && (terrain.Position.Y - Position.Y < closestTerrain.Position.Y - Position.Y) )
                {
                    closestTerrain = terrain;
                }
            }
            return new Tuple<Terrain, bool>(closestTerrain, false);
        }

        public virtual void Jump()
        {
            if (Position.Y > (InitialYPos - MaxJump))
            {
                Position = new Vector2(Position.X, Position.Y - _jumpSpeed);
            }
            else
            {
                Jumping = false;
            }
        }

        public virtual void GravityEffect(List<Terrain> a_terrains)
        {
            Tuple<Terrain,bool> terrainTuple = OnTerrain(a_terrains);
            if (terrainTuple.Item2 == false && Globals.ScreenHeight > Position.Y)
            {
                if (terrainTuple.Item1 != null && Position.Y <= terrainTuple.Item1.Position.Y && Position.Y + _jumpSpeed >= terrainTuple.Item1.Position.Y)
                {
                    Position = new Vector2(Position.X, terrainTuple.Item1.Position.Y);
                }
                else
                {
                    Position = new Vector2(Position.X, Position.Y + _jumpSpeed);
                }
            }
        }

        public virtual void Update(Vector2 a_offset, List<Terrain> a_terrains)
        {
            if (Jumping == true)
            {
                Jump();
            } 
            else
            {
                GravityEffect(a_terrains);
            }

            if (Position.Y >= Globals.ScreenHeight)
            {
                Dead = true;
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
