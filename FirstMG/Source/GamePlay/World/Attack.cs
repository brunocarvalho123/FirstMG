using Microsoft.Xna.Framework;

namespace FirstMG.Source.GamePlay
{
    class Attack
    {
        private Unit _owner;
        private float _range;
        private float _power;

        private bool _landedHit;

        GameGlobals.Orientation _orientation;

        public Attack(Unit a_owner, float a_range, float a_power, GameGlobals.Orientation a_orientation) 
        {
            _owner = a_owner;
            _range = a_range;
            _power = a_power;

            _orientation = a_orientation;

            _landedHit = false;
        }

        public Unit Owner
        {
            get { return _owner; }
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
        public GameGlobals.Orientation Ori
        {
            get { return _orientation; }
        }
    }
}
