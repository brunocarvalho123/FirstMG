#region Includes
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion

namespace FirstMG.Source.Engine.Input
{
    public class MyMouseControl
    {
        private bool _dragging;
        private bool _rightDrag;

        private Vector2 _newMousePos;
        private Vector2 _oldMousePos;
        private Vector2 _firstMousePos;
        private Vector2 _newMouseAdjustedPos;
        private Vector2 _systemCursorPos;
        private Vector2 _screenLoc;

        private MouseState _newMouse;
        private MouseState _oldMouse;
        private MouseState _firstMouse;

        public MyMouseControl()
        {
            _dragging = false;

            _newMouse = Mouse.GetState();
            _oldMouse = _newMouse;
            _firstMouse = _newMouse;

            _newMousePos = new Vector2(_newMouse.Position.X, _newMouse.Position.Y);
            _oldMousePos = new Vector2(_newMouse.Position.X, _newMouse.Position.Y);
            _firstMousePos = new Vector2(_newMouse.Position.X, _newMouse.Position.Y);

            GetMouseAndAdjust();

            //_screenLoc = new Vector2((int)(systemCursorPos.X/Globals.screenWidth), (int)(systemCursorPos.Y/Globals.screenHeight));

        }

        #region Properties

        public Vector2 NewMousePos
        {
            get { return _newMousePos; }
        }

        public MouseState First
        {
            get { return _firstMouse; }
        }

        public MouseState New
        {
            get { return _newMouse; }
        }

        public MouseState Old
        {
            get { return _oldMouse; }
        }

        #endregion

        public void Update()
        {
            GetMouseAndAdjust();


            if(_newMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && _oldMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
            {
                _firstMouse = _newMouse;
                _firstMousePos = _newMousePos = GetScreenPos(_firstMouse);
            }

            
        }

        public void UpdateOld()
        {
            _oldMouse = _newMouse;
            _oldMousePos = GetScreenPos(_oldMouse);
        }

        public virtual float GetDistanceFromClick()
        {
            return Globals.GetDistance(_newMousePos, _firstMousePos);
        }

        public virtual void GetMouseAndAdjust()
        {
            _newMouse = Mouse.GetState();
            _newMousePos = GetScreenPos(_newMouse);

        }




        public int GetMouseWheelChange()
        {
            return _newMouse.ScrollWheelValue - _oldMouse.ScrollWheelValue;
        }


        public Vector2 GetScreenPos(MouseState MOUSE)
        {
            Vector2 tempVec = new Vector2(MOUSE.Position.X, MOUSE.Position.Y);


            return tempVec;
        }

        //public virtual bool LeftClick()
        //{
        //    if( _newMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && _oldMouse.LeftButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed && _newMouse.Position.X >= 0 && _newMouse.Position.X <= Globals.screenWidth && _newMouse.Position.Y >= 0 && _newMouse.Position.Y <= Globals.screenHeight)
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        //public virtual bool LeftClickHold()
        //{
        //    bool holding = false;

        //    if( _newMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && _oldMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && _newMouse.Position.X >= 0 && _newMouse.Position.X <= Globals.screenWidth && _newMouse.Position.Y >= 0 && _newMouse.Position.Y <= Globals.screenHeight)
        //    {
        //        holding = true;

        //        if(Math.Abs(_newMouse.Position.X - _firstMouse.Position.X) > 8 || Math.Abs(_newMouse.Position.Y - _firstMouse.Position.Y) > 8)
        //        {
        //            _dragging = true;
        //        }
        //    }

        //    return holding;
        //}

        public virtual bool LeftClickRelease()
        {
            if(_newMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && _oldMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                _dragging = false;
                return true;
            }

            return false;
        }

        //public virtual bool RightClick()
        //{
        //    if(_newMouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && _oldMouse.RightButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed && _newMouse.Position.X >= 0 && _newMouse.Position.X <= Globals.screenWidth && _newMouse.Position.Y >= 0 && _newMouse.Position.Y <= Globals.screenHeight)
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        //public virtual bool RightClickHold()
        //{
        //    bool holding = false;

        //    if( _newMouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && _oldMouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && _newMouse.Position.X >= 0 && _newMouse.Position.X <= Globals.screenWidth && _newMouse.Position.Y >= 0 && _newMouse.Position.Y <= Globals.screenHeight)
        //    {
        //        holding = true;

        //        if(Math.Abs(_newMouse.Position.X - _firstMouse.Position.X) > 8 || Math.Abs(_newMouse.Position.Y - _firstMouse.Position.Y) > 8)
        //        {
        //            _rightDrag = true;
        //        }
        //    }

        //    return holding;
        //}

        public virtual bool RightClickRelease()
        {
            if( _newMouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released && _oldMouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                _dragging = false;
                return true;
            }

            return false;
        }

        public void SetFirst()
        {

        }
    }
}
