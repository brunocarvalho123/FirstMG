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
        public Dirt(string a_path, Vector2 a_position)
            : base(a_path, a_position, new Vector2(16, 48))
        {
            IsBackground = false;
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
