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
        private Engine.Output.DisplayBar _healthBar;
        private Engine.Output.DisplayBar _staminaBar;

        public UI()
        {
            _font = Engine.Globals.MyContent.Load<SpriteFont>("Fonts\\Arial16");

            _healthBar  = new Engine.Output.DisplayBar(new Vector2(154, 20), 3, Color.Red);
            _staminaBar = new Engine.Output.DisplayBar(new Vector2(154, 20), 3, Color.Yellow);
        }

        public void Update(World a_world)
        {
            _healthBar.Update(a_world.MainCharacter.Health, a_world.MainCharacter.HealthMax);
            _staminaBar.Update(a_world.MainCharacter.Stamina, a_world.MainCharacter.StaminaMax);
        }

        public void Draw(World a_world)
        {
            Engine.Globals.NormalEffect.Parameters["xSize"].SetValue(1.0f);
            Engine.Globals.NormalEffect.Parameters["ySize"].SetValue(1.0f);
            Engine.Globals.NormalEffect.Parameters["xDraw"].SetValue(1.0f);
            Engine.Globals.NormalEffect.Parameters["yDraw"].SetValue(1.0f);
            Engine.Globals.NormalEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
            Engine.Globals.NormalEffect.CurrentTechnique.Passes[0].Apply();

            string displayLine = $"Shreks killed: {a_world._nKilled}";
            Vector2 stringDimension = _font.MeasureString(displayLine);
            Engine.Globals.MySpriteBatch.DrawString(_font, displayLine, new Vector2(Engine.Globals.ScreenWidth / 2 - (stringDimension.X/2), Engine.Globals.ScreenHeight-100), Color.Black );

            if (a_world.MainCharacter.Dead)
            {
                displayLine = "Press Enter to restart!";
                stringDimension = _font.MeasureString(displayLine);
                Engine.Globals.MySpriteBatch.DrawString(_font, displayLine, new Vector2(Engine.Globals.ScreenWidth / 2 - (stringDimension.X / 2), Engine.Globals.ScreenHeight/2), Color.Black);
            }

            _healthBar.Draw(new Vector2(100, Engine.Globals.ScreenHeight - 100));
            _staminaBar.Draw(new Vector2(Engine.Globals.ScreenWidth - 300, Engine.Globals.ScreenHeight - 100));
        }
    }
}
