#region Includes
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
#endregion

namespace FirstMG.Source.Engine.Input
{
    class MyKeyboard
    {
        private KeyboardState _newKeyboard;
        private KeyboardState _oldKeyboard;

        private List<MyKey> _pressedKeys = new List<MyKey>();
        private List<MyKey> _oldPressedKeys = new List<MyKey>();

        public MyKeyboard()
        {
            /* Empty */
        }

        public virtual void Update()
        {
            _newKeyboard = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            GetPressedKeys();
        }

        public void UpdateOld()
        {
            _oldKeyboard = _newKeyboard;

            _oldPressedKeys = new List<MyKey>();
            for (int i = 0; i < _pressedKeys.Count; i++)
            {
                _oldPressedKeys.Add(_pressedKeys[i]);
            }
        }


        public bool GetPress(string a_key)
        {
            for (int i = 0; i < _pressedKeys.Count; i++)
            {
                if (_pressedKeys[i].Key == a_key)
                {
                    return true;
                }

            }
            return false;
        }

        public bool GetNewPress(string a_key)
        {
            bool isPressed = false;
            bool wasPressed = false;
            for (int i = 0; i < _pressedKeys.Count; i++)
            {
                if (_pressedKeys[i].Key == a_key)
                {
                    isPressed = true;
                }
            }
            for (int i = 0; i < _oldPressedKeys.Count; i++)
            {
                if (_oldPressedKeys[i].Key == a_key)
                {
                    wasPressed = true;
                }
            }
            if (!wasPressed && isPressed) return true;
            return false;
        }


        public virtual void GetPressedKeys()
        {
            //bool found = false;
            _pressedKeys.Clear();
            for (int i = 0; i < _newKeyboard.GetPressedKeys().Length; i++)
            {
                _pressedKeys.Add(new MyKey(_newKeyboard.GetPressedKeys()[i].ToString(), 1));
            }
        }
    }
}
