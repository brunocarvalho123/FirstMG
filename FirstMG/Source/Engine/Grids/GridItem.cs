using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMG.Source.Engine
{
    class GridItem : Animated2D
    {
        public GridItem(string a_path, Vector2 a_position, Vector2 a_dims, Vector2 a_frames) : base(a_path, a_position, a_dims, a_frames, Color.White)
        {

        }

    }
}
