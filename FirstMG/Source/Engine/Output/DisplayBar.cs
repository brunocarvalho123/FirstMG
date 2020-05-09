using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMG.Source.Engine.Output
{
    class DisplayBar
    {
        private Asset2D _bar;
        private Asset2D _barBackground;
        private int     _border;
        private Color   _color;

        public DisplayBar (Vector2 a_dimensions, int a_border, Color a_color)
        {
            _border = a_border;
            _color  = a_color;

            _bar           = new Asset2D("Assets\\solid", new Vector2(0, 0), new Vector2(a_dimensions.X - _border * 2, a_dimensions.Y - _border * 2));
            _barBackground = new Asset2D("Assets\\shade", new Vector2(0, 0), new Vector2(a_dimensions.X, a_dimensions.Y));
        }

        public virtual void Update(float a_current, float a_max)
        {
            _bar.Dimension = new Vector2((a_current/a_max) * (_barBackground.Dimension.X-_border*2), _bar.Dimension.Y);
        }

        public virtual void Draw(Vector2 a_offset)
        {
            Engine.Globals.NormalEffect.Parameters["xSize"].SetValue(1.0f);
            Engine.Globals.NormalEffect.Parameters["ySize"].SetValue(1.0f);
            Engine.Globals.NormalEffect.Parameters["xDraw"].SetValue(1.0f);
            Engine.Globals.NormalEffect.Parameters["yDraw"].SetValue(1.0f);
            Engine.Globals.NormalEffect.Parameters["filterColor"].SetValue(Color.Black.ToVector4());
            Engine.Globals.NormalEffect.CurrentTechnique.Passes[0].Apply();

            _barBackground.Draw(a_offset, new Vector2(0,0), Color.Black);

            Engine.Globals.NormalEffect.Parameters["filterColor"].SetValue(_color.ToVector4());
            Engine.Globals.NormalEffect.CurrentTechnique.Passes[0].Apply();

            _bar.Draw(a_offset + new Vector2(_border, _border), new Vector2(0, 0), _color);
        }
    }
}
