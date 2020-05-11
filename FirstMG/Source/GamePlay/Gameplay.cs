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

        Engine.PassObject ChangeGameState;

        public GamePlay(Engine.PassObject a_changeGameState)
        {
            _playState = 0;
            ChangeGameState = a_changeGameState;
            ResetWorld(null);
        }

        public virtual void ResetWorld(object a_object)
        {
            _world = new World(ResetWorld, ChangeGameState);
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
