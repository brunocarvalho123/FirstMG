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
        private string _layer;
        public GridItem(string a_path, Vector2 a_position, Vector2 a_dims, Vector2 a_frames, string a_layer = "") : base(a_path, a_position, a_dims, a_frames, Color.White)
        {
            _layer = a_layer;
        }

        public string Layer
        {
            get { return _layer; }
        }
    }
}
