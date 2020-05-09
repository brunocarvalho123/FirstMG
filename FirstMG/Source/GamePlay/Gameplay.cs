using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMG.Source.GamePlay
{
    class GamePlay
    {

        private int _playState;
        private World _world;

        public GamePlay()
        {
            _playState = 0;

            ResetWorld(null);
        }

        public virtual void ResetWorld(object a_object)
        {
            _world = new World(ResetWorld);
        }

        public virtual void Update()
        {
            if (_playState == 0)
            {
                _world.Update();
            }

        }
        public virtual void Draw()
        {
            if (_playState == 0)
            {
                _world.Draw(Vector2.Zero);
            }

        }
    }
}
