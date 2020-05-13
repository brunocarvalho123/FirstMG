#region Includes
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

        public Npc(string a_path, Vector2 a_position, Vector2 a_dimension, Vector2 a_frames) : base(a_path, a_position, a_dimension, a_frames)
        {
            Speed = 2.0f;
        }

        protected virtual void AI(MainChar a_mainChar)
        {
            Position += Globals.RadialMovement(Position, a_mainChar.Position, Speed);
            Rotation = Globals.RotateTowards(Position, a_mainChar.Position);

            if (Globals.GetDistance(Position, a_mainChar.Position) < 50)
            {
                a_mainChar.GetHit(1);
                Dead = true;
            }
        }

        public virtual void Update(Vector2 a_offset, MainChar a_mainChar)
        {
            AI(a_mainChar);

            base.Update(a_offset);
        }

        public override void Draw(Vector2 a_offset)
        {
            base.Draw(a_offset);
        }
    }
}
