using FirstMG.Source.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMG.Source.GamePlay
{
    class CrumblingBlock : Npc
    {
        private bool _locationImpassible = false;
       
        public CrumblingBlock(Vector2 a_position, Vector2 a_dims) : base("Assets\\Tiles\\block_crumble", a_position, a_dims, new Vector2(10,1))
        {
            FrameAnimations = true;
            CurrentAnimation = 0;
            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 0), 1, 33, 0, "Idle"));
            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 0), 10, 128, 1, 10, Crumble, "Crumble"));
        }

        public void Crumble(object a_obj)
        {
            Dead = true;
        }

        public override void Update(Vector2 a_offset, MainChar a_mainChar, SquareGrid a_grid)
        {
            if (!_locationImpassible)
            {
                GridLocation location = a_grid.GetSlotFromPixel(Position - new Vector2(0, a_grid.SlotDimensions.Y), Vector2.Zero);
                location.SetToFilled(true);
                Position = location.Position + Dimension / 2;
                _locationImpassible = true;
            }
            if (FrameAnimationList[CurrentAnimation].CurrentFrame == 4)
            {
                a_grid.GetSlotFromPixel(Position, Vector2.Zero).Impassible = false;
                a_grid.GetSlotFromPixel(Position, Vector2.Zero).Filled = false;
            }
            if (Globals.GetDistance(a_mainChar.Position, Position) < 100)
            {
                SetAnimationByName("Crumble");
            }

            base.Update(a_offset);
        }

        public override void Draw(Vector2 a_offset)
        {
            base.Draw(a_offset);
        }

    }
}
