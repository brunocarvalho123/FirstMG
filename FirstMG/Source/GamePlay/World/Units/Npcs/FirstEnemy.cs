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
    class FirstEnemy : Npc
    {

        public FirstEnemy(Vector2 a_position, Vector2 a_frames) 
            : base("Assets\\UI\\standard_dirt", a_position, new Vector2(50,50), a_frames)
        {
            Speed = 4.2f;
        }

        public override void Update(Vector2 a_offset, MainChar a_mainChar, SquareGrid a_grid)
        {
            base.Update(a_offset, a_mainChar, a_grid);
        }

        public override void Draw(Vector2 a_offset)
        {
            base.Draw(a_offset);
        }
    }
}
