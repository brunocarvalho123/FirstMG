using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMG.Source.Engine;
using Microsoft.Xna.Framework;

namespace FirstMG.Source.GamePlay
{
    class WindEffect : Effect
    {
        Unit _unit = null;
        public WindEffect(Unit a_unit, Vector2 a_dims) 
            : base("Assets\\Effects\\effect_tornado", a_unit.Position, a_dims, new Vector2(60,1), 750)
        {
            _unit = a_unit;

            FrameAnimations = true;
            CurrentAnimation = 0;
            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 0), 60, 10, 0, "Base"));
        }

        public override void Update(Vector2 a_offset)
        {
            if (_unit.Dead)
            {
                Done = true;
            }
            else 
            { 
                Position = _unit.Position;
            }
            base.Update(a_offset);
        }
    }
}
