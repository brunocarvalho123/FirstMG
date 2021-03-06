﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FirstMG.Source.GamePlay
{
    class CurlyLine : Projectile
    {

        public CurlyLine(Vector2 a_position, Unit a_owner, Vector2 a_target) 
            : base("Assets\\curly_line", a_position, new Vector2 (35,60), a_owner, a_target)
        {
            Speed = 5.0f;
            ProjectileTimer = new Engine.MyTimer(1200);
        }

        public override void Update(Vector2 a_offset, List<Unit> a_units)
        {
            base.Update(a_offset, a_units);
        }

        public override void Draw(Vector2 a_offset)
        {
            base.Draw(a_offset);
        }
    }
}
