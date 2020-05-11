using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMG.Source.Engine
{
    class Button : Asset2D
    {
        private bool _pressed;
        private bool _hovered;
        private string _text;

        private Color _hoverColor;
        private SpriteFont _font;

        public object _info;

        PassObject ButtonClicked;
        

        public Button(string a_path, Vector2 a_position, Vector2 a_dimension, string a_fontPath, string a_text, PassObject a_buttonClicked, object a_info)
            : base(a_path, a_position, a_dimension)
        {
            _text = a_text;
            ButtonClicked = a_buttonClicked;
            _info = a_info;

            if (a_fontPath != "")
            {
                _font = Globals.MyContent.Load<SpriteFont>(a_fontPath);
            }

            _pressed = false;
            _hoverColor = new Color(200, 230, 255);
        }

        public bool Pressed
        {
            get { return _pressed; }
            protected set { _pressed = value; }
        }
        public bool Hovered
        {
            get { return _hovered; }
            protected set { _hovered = value; }
        }
        public string Text
        {
            get { return _text; }
            protected set { _text = value; }
        }

        public virtual void RunButtonClick()
        {
            ButtonClicked?.Invoke(_info);

            Reset();
        }

        public virtual void Reset()
        {
            Pressed = false;
            Hovered = false;
        }

        public override void Update(Vector2 a_offset)
        {
            if (Hover(a_offset))
            {
                Hovered = true;
                if (Globals.MyMouse.LeftClick())
                {
                    Hovered = false;
                    Pressed = true;
                }
                else if (Globals.MyMouse.LeftClickRelease())
                {
                    RunButtonClick();
                }
            }
            else
            {
                Hovered = false;
            }

            if (!Globals.MyMouse.LeftClick() && !Globals.MyMouse.LeftClickHold())
            {
                Pressed = false;
            }

            base.Update(a_offset);
        }

  
        public override void Draw(Vector2 a_offset)
        {
            Color tmpColor = Color.White;
            if (Pressed)
            {
                tmpColor = Color.Gray;
            }
            else if (Hovered)
            {
                tmpColor = _hoverColor;
            }

            Globals.NormalEffect.Parameters["xSize"].SetValue((float)Asset.Bounds.Width);
            Globals.NormalEffect.Parameters["ySize"].SetValue((float)Asset.Bounds.Height);
            Globals.NormalEffect.Parameters["xDraw"].SetValue((float)((int)Dimension.X));
            Globals.NormalEffect.Parameters["yDraw"].SetValue((float)((int)Dimension.Y));
            Globals.NormalEffect.Parameters["filterColor"].SetValue(tmpColor.ToVector4());
            Globals.NormalEffect.CurrentTechnique.Passes[0].Apply();

            base.Draw(a_offset);

            Vector2 stringDimension = _font.MeasureString(_text);
            Globals.MySpriteBatch.DrawString(_font, _text, Position + a_offset + new Vector2(-stringDimension.X / 2, -stringDimension.Y/2), Color.Black);
        }
    }
}
