﻿#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMG.Source.Engine;
using Microsoft.Xna.Framework;
#endregion

namespace FirstMG.Source.GamePlay
{
    class Npc : Unit
    {
        protected State _state = State.STANDING;

        public Npc(string a_path, Vector2 a_position, Vector2 a_dimension, Vector2 a_frames) : base(a_path, a_position, a_dimension, a_frames)
        {
            MovSpeed = 2.0f;
        }

        protected virtual void AI(MainChar a_mainChar)
        {
            Position += Globals.RadialMovement(Position, a_mainChar.Position, MovSpeed);
            Rotation = Globals.RotateTowards(Position, a_mainChar.Position);

            if (Globals.GetDistance(Position, a_mainChar.Position) < 50)
            {
                a_mainChar.GetHit(1);
                Dead = true;
            }
        }

        public virtual void Update(Vector2 a_offset, MainChar a_mainChar, SquareGrid a_grid)
        {
            //AI(a_mainChar);

            base.Update(a_offset, a_grid);
        }

        public override void Draw(Vector2 a_offset)
        {
            //Globals.NormalEffect.Parameters["xSize"].SetValue((float)Asset.Bounds.Width);
            //Globals.NormalEffect.Parameters["ySize"].SetValue((float)Asset.Bounds.Height);
            //Globals.NormalEffect.Parameters["xDraw"].SetValue((float)((int)Dimension.X));
            //Globals.NormalEffect.Parameters["yDraw"].SetValue((float)((int)Dimension.Y));
            Globals.NormalEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());
            Globals.NormalEffect.CurrentTechnique.Passes[0].Apply();

            base.Draw(a_offset);
        }
    }
}
