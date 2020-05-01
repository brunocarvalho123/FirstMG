#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
#endregion

namespace FirstMG.Source.GamePlay
{
    class World
    {

        private MainChar _mainChar;

        public World()
        {
            _mainChar = new MainChar("Assets\\pixo", /* position */ new Vector2(300,300), /* dimension */ new Vector2(150,150));
        }

        public virtual void Update()
        {
            _mainChar.Update();
        }

        public virtual void Draw(Vector2 a_offset)
        {
            _mainChar.Draw(a_offset);
        }
    }
}
