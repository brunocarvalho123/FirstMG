using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMG.Source.Engine
{
    class TiledBackground : Asset2D
    {
        private Vector2 _backgroundDims;

        public TiledBackground(string a_path, Vector2 a_position, Vector2 a_dims, Vector2 a_backgroundDims) : base(a_path, a_position, new Vector2((float)Math.Floor(a_dims.X), (float)Math.Floor(a_dims.Y)))
        {
            _backgroundDims = new Vector2((float)Math.Floor(a_backgroundDims.X), (float)Math.Floor(a_backgroundDims.Y));
        }

        public override void Draw(Vector2 a_offset)
        {
            float nXTiles = _backgroundDims.X / Dimension.X;
            float nYTiles = _backgroundDims.Y / Dimension.Y;

            for (int i = 0; i < nXTiles; i++)
            {
                for (int j = 0; j < nYTiles; j++)
                {
                    if (i < nXTiles - 1 && j < nYTiles - 1)
                    {
                        base.Draw(a_offset + new Vector2(Dimension.X / 2 + Dimension.X * i, Dimension.Y / 2 + Dimension.Y * j));
                    } 
                    else
                    {
                        float xLeft = Math.Min(Dimension.X, (_backgroundDims.X - (i * Dimension.X)));
                        float yLeft = Math.Min(Dimension.Y, (_backgroundDims.Y - (j * Dimension.Y)));

                        float xPercentLeft = Math.Min(1, xLeft / Dimension.X);
                        float yPercentLeft = Math.Min(1, yLeft / Dimension.Y);

                        Engine.Globals.MySpriteBatch.Draw(Asset, 
                                                          new Rectangle((int)(Position.X + a_offset.X + Dimension.X * i), (int)(Position.Y + a_offset.Y + Dimension.Y * j), (int)(Math.Ceiling(Dimension.X * xPercentLeft)), (int)(Math.Ceiling(Dimension.Y * yPercentLeft))), 
                                                          new Rectangle(0, 0, (int)(xPercentLeft * Asset.Bounds.Width), (int)(yPercentLeft * Asset.Bounds.Height)), 
                                                          Color.White, 
                                                          Rotation, 
                                                          new Vector2(0,0), 
                                                          new SpriteEffects(), 
                                                          0);

                    }
                }
            }
        }

    }
}
