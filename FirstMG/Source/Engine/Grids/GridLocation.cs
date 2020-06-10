using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMG.Source.Engine
{ 
    public class GridLocation
    {

        private bool _filled;
        private bool _impassible;
        private bool _unpathable;
        private bool _deadly;

        private float _fScore;
        private float _cost;
        private float _currentDist;

        private Vector2 _parent;
        private Vector2 _position;

        public GridLocation(float a_cost, bool a_filled, Vector2 a_position)
        {
            _cost = a_cost;
            _filled = a_filled;

            _unpathable = false;
            _impassible = false;
            _deadly = false;

            _position = a_position;
        }

        public bool Impassible
        {
            get { return _impassible; }
            set { _impassible = value; }
        }
        public bool Filled
        {
            get { return _filled; }
            set { _filled = value; }
        }
        public bool Deadly
        {
            get { return _deadly; }
            set { _deadly = value; }
        }
        public Vector2 Position
        {
            get { return _position; }
        }

        public virtual void SetToFilled(bool a_impassible)
        {
            _filled = true;
            _impassible = a_impassible;
        }

    }
}
