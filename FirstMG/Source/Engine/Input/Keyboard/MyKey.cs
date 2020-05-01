using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMG.Source.Engine.Input
{
    class MyKey
    {
        private int _state;
        private string _key;
        private string _print;
        private string _display;

        public string Key
        {
            get { return _key; }
        }

        public MyKey(string a_key, int a_state)
        {
            _key = a_key;
            _state = a_state;
            MakePrint(_key);
        }

        public virtual void Update()
        {
            _state = 2;
        }

        public void MakePrint(string a_key)
        {
            _display = a_key;

            string tempStr = "";

            if (a_key == "A" || a_key == "B" || a_key == "C" || a_key == "D" || a_key == "E" || a_key == "F" || a_key == "G" || a_key == "H" || a_key == "I" || a_key == "J" || a_key == "K" || a_key == "L" || a_key == "M" || a_key == "N" || a_key == "O" || a_key == "P" || a_key == "Q" || a_key == "R" || a_key == "S" || a_key == "T" || a_key == "U" || a_key == "V" || a_key == "W" || a_key == "X" || a_key == "Y" || a_key == "Z")
            {
                tempStr = a_key;
            }
            if (a_key == "Space")
            {
                tempStr = " ";
            }
            if (a_key == "OemCloseBrackets")
            {
                tempStr = "]";
                _display = tempStr;
            }
            if (a_key == "OemOpenBrackets")
            {
                tempStr = "[";
                _display = tempStr;
            }
            if (a_key == "OemMinus")
            {
                tempStr = "-";
                _display = tempStr;
            }
            if (a_key == "OemPeriod" || a_key == "Decimal")
            {
                tempStr = ".";
            }
            if (a_key == "D1" || a_key == "D2" || a_key == "D3" || a_key == "D4" || a_key == "D5" || a_key == "D6" || a_key == "D7" || a_key == "D8" || a_key == "D9" || a_key == "D0")
            {
                tempStr = a_key.Substring(1);
            }
            else if (a_key == "NumPad1" || a_key == "NumPad2" || a_key == "NumPad3" || a_key == "NumPad4" || a_key == "NumPad5" || a_key == "NumPad6" || a_key == "NumPad7" || a_key == "NumPad8" || a_key == "NumPad9" || a_key == "NumPad0")
            {
                tempStr = a_key.Substring(6);
            }


            _print = tempStr;
        }
    }
}
