using FirstMG.Source.Engine;
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
        private Asset2D _pauseOverlay;
        private Button _resetButton;
        private SpriteFont _font;
        private Engine.Output.DisplayBar _healthBar;
        private Engine.Output.DisplayBar _staminaBar;

        public UI(PassObject a_resetWorld)
        {
            _font = Globals.MyContent.Load<SpriteFont>("Fonts\\Arial16");
            _pauseOverlay = new Asset2D("Assets\\pause_overlay", new Vector2(Globals.ScreenWidth / 2, Globals.ScreenHeight / 2), new Vector2(300,300));
            _resetButton = new Button("Assets\\simple_button", new Vector2(0,0), new Vector2(96,32), "Fonts\\Arial16", "Restart", a_resetWorld, null);

            _healthBar  = new Engine.Output.DisplayBar(new Vector2(154, 20), 3, Color.Red);
            _staminaBar = new Engine.Output.DisplayBar(new Vector2(154, 20), 3, Color.Yellow);
        }

        public void Update(World a_world)
        {
            _healthBar.Update(a_world.MainCharacter.Health, a_world.MainCharacter.HealthMax);
            _staminaBar.Update(a_world.MainCharacter.Stamina, a_world.MainCharacter.StaminaMax);

            if (a_world.MainCharacter.Dead)
            {
                _resetButton.Update(new Vector2(Globals.ScreenWidth / 2, Globals.ScreenHeight / 2 + 100));
            }
        }

        public void Draw(World a_world)
        {
            Globals.NormalEffect.Parameters["xSize"].SetValue(1.0f);
            Globals.NormalEffect.Parameters["ySize"].SetValue(1.0f);
            Globals.NormalEffect.Parameters["xDraw"].SetValue(1.0f);
            Globals.NormalEffect.Parameters["yDraw"].SetValue(1.0f);
            Globals.NormalEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
            Globals.NormalEffect.CurrentTechnique.Passes[0].Apply();

            string displayLine = $"Shreks killed: {a_world._nKilled}";
            Vector2 stringDimension = _font.MeasureString(displayLine);
            Globals.MySpriteBatch.DrawString(_font, displayLine, new Vector2(Globals.ScreenWidth / 2 - (stringDimension.X/2), Globals.ScreenHeight-100), Color.Black );

            if (a_world.MainCharacter.Dead)
            {
                displayLine = "You ded brah... Press Enter to restart!";
                stringDimension = _font.MeasureString(displayLine);
                Globals.MySpriteBatch.DrawString(_font, displayLine, new Vector2(Globals.ScreenWidth / 2 - (stringDimension.X / 2), Globals.ScreenHeight/2), Color.Black);

                _resetButton.Draw(new Vector2(Globals.ScreenWidth / 2, Globals.ScreenHeight / 2 + 100));
            }

            _healthBar.Draw(new Vector2(100, Globals.ScreenHeight - 100));
            _staminaBar.Draw(new Vector2(Globals.ScreenWidth - 300, Globals.ScreenHeight - 100));

            if (GameGlobals.paused)
            {
                _pauseOverlay.Draw(Vector2.Zero);
            }
        }
    }
}
