using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMG.Source.GamePlay
{
    class GameGlobals
    {
        public enum Orientation
        {
            RIGHT,
            LEFT,
            BOT,
            TOP
        }

        public static Engine.PassObject PassProjectile;
        public static Engine.PassObject PassNpc;
        public static Engine.PassObject CheckScroll;
        public static Engine.PassObject ResetScroll;
        public static Engine.PassObject ExecuteAttack;
        public static Engine.PassObject ExecuteEnemyAttack;

        public static Engine.PassObjectAndReturn GetClosestNpc;

        public static bool paused     = false;
    }
}
