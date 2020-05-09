using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMG.Source.GamePlay
{
    class Dirt : Terrain
    {
        public Dirt(Vector2 a_position)
            : base("Assets\\dirt", a_position, new Vector2(50, 50))
        {

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
