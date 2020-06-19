using Microsoft.Xna.Framework;

namespace FirstMG.Source.GamePlay
{
    class Attack
    {
        private Vector2 _position;
        private float _range;
        private float _power;

        private bool _landedHit;

        public Attack(Vector2 a_position, float a_range, float a_power) 
        {
            _position = a_position;
            _range    = a_range;
            _power    = a_power;

            _landedHit = false;
        }

        public Vector2 Position
        {
            get { return _position; }
        }
        public float Range
        {
            get { return _range; }
        }
        public float Power
        {
            get { return _power; }
        }
        public bool LandedHit
        {
            get { return _landedHit; }
            set { _landedHit = value; }
        }
    }
}
