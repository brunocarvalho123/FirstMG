#region Includes
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace FirstMG.Source.Engine
{
    public class SquareGrid
    {

        private bool _showGrid;

        private Vector2 _slotDims; 
        private Vector2 _gridDims;
        private Vector2 _physicalStartPos;
        private Vector2 _totalPhysicalDims;
        private Vector2 _currentHoverSlot;

        private float   _gravity;
        private float   _friction;

        private Asset2D _gridImg;

        private List<List<GridLocation>> _slots = new List<List<GridLocation>>();


        public SquareGrid(Vector2 a_slotDims, Vector2 a_startPos, Vector2 a_totalDims, float a_gravity = .8f, float a_friction = 2)
        {
            _gravity  = a_gravity;
            _friction = a_friction;

            _showGrid = false;

            _slotDims = a_slotDims;

            _physicalStartPos = new Vector2((int)a_startPos.X, (int)a_startPos.Y);
            _totalPhysicalDims = new Vector2((int)a_totalDims.X, (int)a_totalDims.Y);

            _currentHoverSlot = new Vector2(-1, -1);

            SetBaseGrid();
            
            _gridImg = new Asset2D("Assets\\UI\\shade", _slotDims/2, new Vector2(_slotDims.X-2, _slotDims.Y-2));
        }

        public bool ShowGrid
        {
            get { return _showGrid; }
            set { _showGrid = value; }
        }
        public Vector2 SlotDimensions
        {
            get { return _slotDims; }
        }
        public float Gravity
        {
            get { return _gravity; }
        }


        public virtual Vector2 GetLocationFromPixel(Vector2 a_pixel, Vector2 a_offset)
        {
            Vector2 adjustedPos = a_pixel - _physicalStartPos + a_offset;

            Vector2 tempVec = new Vector2(Math.Min(Math.Max(0, (int)(adjustedPos.X / _slotDims.X)), _slots.Count - 1), Math.Min(Math.Max(0, (int)(adjustedPos.Y / _slotDims.Y)), _slots[0].Count - 1));

            return tempVec;
        }

        public virtual Vector2 GetPositionFromLocation(Vector2 a_location)
        {
            return _physicalStartPos + new Vector2((int)a_location.X * _slotDims.X, (int)a_location.Y * _slotDims.Y);
        }

        public virtual GridLocation GetSlotFromLocation(Vector2 a_loc)
        {
            if(a_loc.X >= 0 && a_loc.Y >= 0 && a_loc.X < _slots.Count && a_loc.Y < _slots[(int)a_loc.X].Count)
            {
                return _slots[(int)a_loc.X][(int)a_loc.Y];
            }

            return null;
        }

        public virtual GridLocation GetSlotBelow(Vector2 a_loc)
        {
            if (a_loc.X >= 0 && a_loc.Y + 1 >= 0 && a_loc.X < _slots.Count && a_loc.Y + 1 < _slots[(int)a_loc.X].Count)
            {
                return _slots[(int)a_loc.X][(int)a_loc.Y+1];
            }

            return null;
        }

        public virtual GridLocation GetSlotFromPixel(Vector2 a_pixel, Vector2 a_offset)
        {
            Vector2 tmpLocation = GetLocationFromPixel(a_pixel, a_offset);
            if (tmpLocation.X >= 0 && tmpLocation.Y >= 0 && tmpLocation.X < _slots.Count && tmpLocation.Y < _slots[(int)tmpLocation.X].Count)
            {
                return _slots[(int)tmpLocation.X][(int)tmpLocation.Y];
            }

            return null;
        }

        
        public virtual void SetBaseGrid()
        {
            _gridDims = new Vector2((int)(_totalPhysicalDims.X/_slotDims.X), (int)(_totalPhysicalDims.Y/_slotDims.Y));

            _slots.Clear();
            for(int i=0; i<_gridDims.X; i++)
            {
                _slots.Add(new List<GridLocation>());

                for(int j=0; j<_gridDims.Y; j++)
                {
                    _slots[i].Add(new GridLocation(1, false, _physicalStartPos + new Vector2(i * _slotDims.X, j * _slotDims.Y)));
                }
            }
        }

        public virtual void Update(Vector2 a_offset)
        {
            _currentHoverSlot = GetLocationFromPixel(new Vector2(Globals.MyMouse.NewMousePos.X, Globals.MyMouse.NewMousePos.Y), -a_offset);
        }

        public virtual void DrawGrid(Vector2 a_offset)
        {
            if(_showGrid)
            {
                //Vector2 topLeft = GetLocationFromPixel((new Vector2(0, 0)) / Globals.zoom  - a_offset, Vector2.Zero);
                //Vector2 botRight = GetLocationFromPixel((new Vector2(Globals.screenWidth, Globals.screenHeight)) / Globals.zoom  - a_offset, Vector2.Zero);
                Vector2 topLeft = GetLocationFromPixel(new Vector2(0, 0), Vector2.Zero);
                Vector2 botRight = GetLocationFromPixel(new Vector2(Globals.ScreenWidth, Globals.ScreenHeight), Vector2.Zero);

                Globals.NormalEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
                Globals.NormalEffect.CurrentTechnique.Passes[0].Apply();

                for(int j=(int)topLeft.X;j<=botRight.X && j<_slots.Count;j++)
                {
                    for(int k=(int)topLeft.Y;k<=botRight.Y && k<_slots[0].Count;k++)
                    {
                        if(_currentHoverSlot.X == j && _currentHoverSlot.Y == k)
                        {
                            Globals.NormalEffect.Parameters["filterColor"].SetValue(Color.Red.ToVector4());
                            Globals.NormalEffect.CurrentTechnique.Passes[0].Apply();
                        }
                        else if(_slots[j][k].Filled)
                        {
                            Globals.NormalEffect.Parameters["filterColor"].SetValue(Color.DarkGreen.ToVector4());
                            Globals.NormalEffect.CurrentTechnique.Passes[0].Apply();
                        }
                        else
                        {
                            Globals.NormalEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
                            Globals.NormalEffect.CurrentTechnique.Passes[0].Apply();
                        }

                        _gridImg.Draw(a_offset + _physicalStartPos + new Vector2(j * _slotDims.X, k * _slotDims.Y));
                    }
                }
            }
        }
    }
}
