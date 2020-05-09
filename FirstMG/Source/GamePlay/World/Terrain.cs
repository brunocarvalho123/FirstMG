using FirstMG.Source.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMG.Source.GamePlay
{
    class Terrain : Asset2D
    {
        private bool _isBackground = true;
        public Terrain(string a_path, Vector2 a_position, Vector2 a_dimension) : base(a_path, a_position, a_dimension)
        {
            /* empty */
        }

        public bool IsBackground
        {
            get { return _isBackground; }
            protected set { _isBackground = value;  }
        }

        public override void Update(Vector2 a_offset)
        {
            /* empty */
        }

        public override void Draw(Vector2 a_offset)
        {
            Engine.Globals.NormalEffect.Parameters["xSize"].SetValue((float)Asset.Bounds.Width);
            Engine.Globals.NormalEffect.Parameters["ySize"].SetValue((float)Asset.Bounds.Height);
            Engine.Globals.NormalEffect.Parameters["xDraw"].SetValue((float)((int)Dimension.X));
            Engine.Globals.NormalEffect.Parameters["yDraw"].SetValue((float)((int)Dimension.Y));
            Engine.Globals.NormalEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
            Engine.Globals.NormalEffect.CurrentTechnique.Passes[0].Apply();

            base.Draw(a_offset);
        }
    }
}
