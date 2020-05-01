#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace FirstMG.Source.Engine
{
    class Asset2D
    {

        private float _rotation;
        private Vector2 _position;
        private Vector2 _dimension;
        private Texture2D _asset;

        public Asset2D(string a_path, Vector2 a_position, Vector2 a_dimension)
        {
            _position = a_position;
            _dimension = a_dimension;

            _asset = Globals.MyContent.Load<Texture2D>(a_path);
        }

        protected Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        protected float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public virtual void Update()
        {

        }

        public virtual void Draw(Vector2 a_offset)
        {
            if (_asset != null) 
            {
                Globals.MySpriteBatch.Draw(_asset,                                                                                                             // Texture 
                                          new Rectangle((int)(_position.X+a_offset.X),(int)(_position.Y+a_offset.Y),(int)(_dimension.X),(int)(_dimension.Y)),  // Destination rectangle
                                          null,                                                                                                                // Source rectangle
                                          Color.White,                                                                                                         // Color
                                          _rotation,                                                                                                           // Rotation
                                          new Vector2(_asset.Bounds.Width/2, _asset.Bounds.Height/2),                                                          // Origin
                                          new SpriteEffects(),                                                                                                 // Effects
                                          0.0f);                                                                                                               // Layer depth
            }
        }

        public virtual void Draw(Vector2 a_offset, Vector2 a_origin)
        {
            if (_asset != null)
            {
                Globals.MySpriteBatch.Draw(_asset,                                                                                                                    // Texture 
                                          new Rectangle((int)(_position.X + a_offset.X), (int)(_position.Y + a_offset.Y), (int)(_dimension.X), (int)(_dimension.Y)),  // Destination rectangle
                                          null,                                                                                                                       // Source rectangle
                                          Color.White,                                                                                                                // Color
                                          _rotation,                                                                                                                  // Rotation
                                          new Vector2(a_origin.X, a_origin.Y),                                                                                        // Origin
                                          new SpriteEffects(),                                                                                                        // Effects
                                          0.0f);                                                                                                                      // Layer depth
            }
        }
    }
}
