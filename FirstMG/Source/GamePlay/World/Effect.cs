using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FirstMG.Source.Engine;
using Microsoft.Xna.Framework;

namespace FirstMG.Source.GamePlay
{
    class Effect : Animated2D
    {
        private bool           _done;
        private Engine.MyTimer _timer;

        public Effect(string a_path, Vector2 a_position, Vector2 a_dimension, Vector2 a_frames, int a_msec) 
            : base(a_path, a_position, a_dimension, a_frames, Color.White)
        {
            _done  = false;

            _timer = new Engine.MyTimer(a_msec);
        }

        protected Engine.MyTimer EffectTimer
        {
            get { return _timer; }
            set { _timer = value; }
        }
        public bool Done
        {
            get { return _done; }
            protected set { _done = value; }
        }

        public override void Update(Vector2 a_offset)
        {
            _timer.UpdateTimer();
            if (_timer.Test())
            {
                _done = true;
            }

            base.Update(a_offset);
        }

        public override void Draw(Vector2 a_offset)
        {
            base.Draw(a_offset);
        }
    }
}
