using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMG.Source.GamePlay
{
    class UI
    {

        private SpriteFont _font;

        public UI()
        {
            _font = Engine.Globals.MyContent.Load<SpriteFont>("Fonts\\Arial16");
        }

        public void Update()
        {

        }

        public void Draw(World a_world)
        {
            string displayLine = $"Shreks killed: {a_world._nKilled}";
            Vector2 stringDimension = _font.MeasureString(displayLine);
            Engine.Globals.MySpriteBatch.DrawString(_font, displayLine, new Vector2(Engine.Globals.ScreenWidth / 2 - (stringDimension.X/2), Engine.Globals.ScreenHeight-100), Color.Black );
        }
    }
}
