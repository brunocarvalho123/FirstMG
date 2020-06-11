#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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


        private List<GridItem> _gridItems = new List<GridItem>();
        private List<List<GridLocation>> _slots = new List<List<GridLocation>>();


        public SquareGrid(Vector2 a_slotDims, Vector2 a_startPos, Vector2 a_gridDims, XElement a_map, float a_gravity = .8f, float a_friction = .5f)
        {
            _gravity  = a_gravity;
            _friction = a_friction;

            _showGrid = false;

            _slotDims = a_slotDims;
            _gridDims = a_gridDims;

            _physicalStartPos = new Vector2((int)a_startPos.X, (int)a_startPos.Y);
            _totalPhysicalDims = new Vector2((int)_gridDims.X * a_slotDims.X, (int)_gridDims.Y * a_slotDims.Y);

            _currentHoverSlot = new Vector2(-1, -1);

            SetBaseGrid();
            
            _gridImg = new Asset2D("Assets\\UI\\shade", _slotDims/2, new Vector2(_slotDims.X-2, _slotDims.Y-2));

            LoadData(a_map);
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
        public Vector2 TotalPhysicalDims
        {
            get { return _totalPhysicalDims; }
        }
        public float Gravity
        {
            get { return _gravity; }
        }
        public float Friction
        {
            get { return _friction; }
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

        public virtual List<GridLocation> GetTopSlots(Vector4 a_boundingBox)
        {
            List<GridLocation> topSlots = new List<GridLocation>();

            // Slots above bounding box
            for (float i = a_boundingBox.X; i <= a_boundingBox.Y; i += SlotDimensions.X)
            {
                topSlots.Add(GetSlotFromPixel(new Vector2(i, a_boundingBox.Z - SlotDimensions.Y), Vector2.Zero));
            }
            GridLocation lastSlot = GetSlotFromPixel(new Vector2(a_boundingBox.Y - 1, a_boundingBox.Z - SlotDimensions.Y), Vector2.Zero);
            if (topSlots.Last() != lastSlot) topSlots.Add(lastSlot);

            // Slots intersecting bounding box
            for (float i = a_boundingBox.X; i <= a_boundingBox.Y; i += SlotDimensions.X)
            {
                topSlots.Add(GetSlotFromPixel(new Vector2(i, a_boundingBox.Z), Vector2.Zero));
            }
            lastSlot = GetSlotFromPixel(new Vector2(a_boundingBox.Y - 1, a_boundingBox.Z), Vector2.Zero);
            if (topSlots.Last() != lastSlot) topSlots.Add(lastSlot);

            return topSlots;
        }
        public virtual List<GridLocation> GetBotSlots(Vector4 a_boundingBox) 
        {
            List<GridLocation> botSlots = new List<GridLocation>();

            // Slots below bounding box
            for (float i = a_boundingBox.X; i <= a_boundingBox.Y; i += SlotDimensions.X)
            {
                botSlots.Add(GetSlotFromPixel(new Vector2(i, a_boundingBox.W + SlotDimensions.Y), Vector2.Zero));
            }
            GridLocation lastSlot = GetSlotFromPixel(new Vector2(a_boundingBox.Y - 1, a_boundingBox.W + SlotDimensions.Y), Vector2.Zero);

            if (botSlots.Last() != lastSlot) botSlots.Add(lastSlot);

            return botSlots;
        }
        public virtual List<GridLocation> GetLeftSlots(Vector4 a_boundingBox) 
        {
            List<GridLocation> leftSlots = new List<GridLocation>();

            // Slots to the left of bounding box
            for (float i = a_boundingBox.Z + SlotDimensions.Y; i <= a_boundingBox.W; i += SlotDimensions.Y)
            {
                leftSlots.Add(GetSlotFromPixel(new Vector2(a_boundingBox.X - SlotDimensions.X, i), Vector2.Zero));
            }
            GridLocation lastSlot = GetSlotFromPixel(new Vector2(a_boundingBox.X - SlotDimensions.X, a_boundingBox.W), Vector2.Zero);
            if (leftSlots.Last() != lastSlot) leftSlots.Add(lastSlot);

            // Slots intersecting the bounding box
            for (float i = a_boundingBox.Z + SlotDimensions.Y; i <= a_boundingBox.W; i += SlotDimensions.Y)
            {
                leftSlots.Add(GetSlotFromPixel(new Vector2(a_boundingBox.X, i), Vector2.Zero));
            }
            lastSlot = GetSlotFromPixel(new Vector2(a_boundingBox.X, a_boundingBox.W), Vector2.Zero);
            if (leftSlots.Last() != lastSlot) leftSlots.Add(lastSlot);

            return leftSlots;
        }
        public virtual List<GridLocation>GetRightSlots(Vector4 a_boundingBox) 
        {
            List<GridLocation> rightSlots = new List<GridLocation>();

            // Slots to the right of bounding box
            for (float i = a_boundingBox.Z + SlotDimensions.Y; i <= a_boundingBox.W; i += SlotDimensions.Y)
            {
                rightSlots.Add(GetSlotFromPixel(new Vector2(a_boundingBox.Y + SlotDimensions.X, i), Vector2.Zero));
            }
            GridLocation lastSlot = GetSlotFromPixel(new Vector2(a_boundingBox.Y + SlotDimensions.X, a_boundingBox.W), Vector2.Zero);
            if (rightSlots.Last() != lastSlot) rightSlots.Add(lastSlot);

            // Slots intersecting the bounding box
            for (float i = a_boundingBox.Z + SlotDimensions.Y; i <= a_boundingBox.W; i += SlotDimensions.Y)
            {
                rightSlots.Add(GetSlotFromPixel(new Vector2(a_boundingBox.Y, i), Vector2.Zero));
            }
            lastSlot = GetSlotFromPixel(new Vector2(a_boundingBox.Y, a_boundingBox.W), Vector2.Zero);
            if (rightSlots.Last() != lastSlot) rightSlots.Add(lastSlot);

            return rightSlots;
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


        public virtual void AddGridItem(XElement a_tile, Vector2 a_location)
        {
            _gridItems.Add(new GridItem(/* Path       */ a_tile.Element("image").Attribute("source").Value,
                                        /* Position   */ GetPositionFromLocation(a_location) + SlotDimensions/2,
                                        /* Dimensions */ Globals.NewVector(SlotDimensions),
                                        /* Frames      */ new Vector2(1,1)));

            GridLocation slot = GetSlotFromLocation(a_location);

            bool isImpassible = Convert.ToBoolean(a_tile.Element("properties").Descendants().Where(elem => (string)elem.Attribute("name") == "impassible").FirstOrDefault().Attribute("value").Value);
            bool isDeadly = Convert.ToBoolean(a_tile.Element("properties").Descendants().Where(elem => (string)elem.Attribute("name") == "deadly").FirstOrDefault().Attribute("value").Value);
            
            slot.SetToFilled(isImpassible);
            slot.Deadly = isDeadly;
        }

        public virtual void LoadData(XElement a_map)
        {
            if (a_map != null)
            {
                string tilesetSource = a_map.Element("tileset").Attribute("source").Value;
                XElement tileset = XDocument.Load("XML\\Tilesets\\" + tilesetSource).Element("tileset");

                string layerData = a_map.Element("layer").Value;

                string[] tileArray = layerData.Split(',','\n');

                int x = 0;
                int y = 0;
                foreach (string tile in tileArray)
                {
                    if (tile != "")
                    {
                        if (x == _gridDims.X)
                        {
                            x = 0;
                            y += 1;
                        }
                        if (tile != "0")
                        {
                            int tileId = Convert.ToInt32(tile) - 1;
                            XElement tileElement = tileset.Descendants().Where(elem => (string) elem.Attribute("id") == tileId.ToString()).FirstOrDefault();
                            AddGridItem(tileElement, new Vector2(x, y));
                        }
                        x += 1;
                    }
                }
            }
        }


        public virtual void SetBaseGrid()
        {
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

            foreach (GridItem item in _gridItems)
            {
                item.Draw(a_offset);
            }
        }
    }
}
