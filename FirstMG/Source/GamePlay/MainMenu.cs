using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FirstMG.Source.GamePlay
{
    public class MainMenu
    {

        private Engine.Asset2D _background;
        private List<Engine.Button> _buttons = new List<Engine.Button>();

        public Engine.PassObject PlayClickDel;
        public Engine.PassObject ExitClickDel;


        public MainMenu(Engine.PassObject a_playClick, Engine.PassObject a_exitClick)
        {
            PlayClickDel = a_playClick;
            ExitClickDel = a_exitClick;

            _background = new Engine.Asset2D("Assets\\UI\\main_menu_bkg",
                                             new Vector2(Engine.Globals.ScreenWidth/2, Engine.Globals.ScreenHeight/2),
                                             new Vector2(Engine.Globals.ScreenWidth, Engine.Globals.ScreenHeight));

            _buttons.Add(new Engine.Button("Assets\\UI\\simple_button", new Vector2(0, 0), new Vector2(96, 32), "Fonts\\Arial16", "Play", PlayClickDel, 1));
            _buttons.Add(new Engine.Button("Assets\\UI\\simple_button", new Vector2(0, 0), new Vector2(96, 32), "Fonts\\Arial16", "Exit", ExitClickDel, null));
        }


        public virtual void Update()
        {
            for (int i = 0; i<_buttons.Count; i++)
            {
                _buttons[i].Update(new Vector2(340, 600 + 45 * i));
            }
        }

        public virtual void Draw()
        {
            _background.Draw(Vector2.Zero);

            for (int i = 0; i < _buttons.Count; i++)
            {
                _buttons[i].Draw(new Vector2(340, 600 + 45 * i));
            }
        }
    }
}
