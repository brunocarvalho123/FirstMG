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

        public FirstEnemy(Vector2 a_position) 
            : base("Assets\\shrek", a_position, new Vector2(100,100))
        {
            Speed = 3.2f;
        }

        public override void Update(Vector2 a_offset, MainChar a_mainChar)
        {
            base.Update(a_offset, a_mainChar);
        }

        public override void Draw(Vector2 a_offset)
        {
            base.Draw(a_offset);
        }
    }
}
