﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMG.Source.GamePlay
{
    class GameGlobals
    {
        public static Engine.PassObject PassProjectile;
        public static Engine.PassObject PassNpc;
        public static Engine.PassObject CheckScroll;
        public static Engine.PassObject ExecuteAttack;

        public static bool paused     = false;
    }
}
