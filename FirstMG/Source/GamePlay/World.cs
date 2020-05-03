#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
#endregion

namespace FirstMG.Source.GamePlay
{
    class World
    {
        private Vector2 _offset;
        private MainChar _mainChar;
        private UI _ui;
        private List<Projectile> _projectiles = new List<Projectile>();
        private List<Npc> _npcs = new List<Npc>();

        public int _nKilled;

        public World()
        {
            _mainChar = new MainChar("Assets\\pixo", /* position */ new Vector2(300, GameGlobals.FloorLevel), /* dimension */ new Vector2(150,150));
            _ui = new UI();

            GameGlobals.PassProjectile = AddProjectile;
            GameGlobals.PassNpc = AddNpc;
            GameGlobals.CheckScroll = CheckScroll;

            _nKilled = 0;

            _offset = new Vector2(0, 0);
        }

        public virtual void AddProjectile(object a_projectile)
        {
            _projectiles.Add((Projectile)a_projectile);
        }

        public virtual void AddNpc(object a_npc)
        {
            _npcs.Add((Npc)a_npc);
        }

        public virtual void CheckScroll(object a_position)
        {
            Vector2 tmpPos = (Vector2)a_position;
            if (tmpPos.X < (-_offset.X + (Engine.Globals.ScreenWidth * .3f)))
            {
                _offset = new Vector2(_offset.X + _mainChar.Speed, _offset.Y);
            }
            if (tmpPos.X > (-_offset.X + (Engine.Globals.ScreenWidth * .7f)))
            {
                _offset = new Vector2(_offset.X - _mainChar.Speed, _offset.Y);
            }
            if (tmpPos.Y < (-_offset.Y + (Engine.Globals.ScreenHeight * .3f)))
            {
                _offset = new Vector2(_offset.X, _offset.Y + _mainChar.Speed);
            }
            if (tmpPos.Y > (-_offset.Y + (Engine.Globals.ScreenHeight * .7f)))
            {
                _offset = new Vector2(_offset.X, _offset.Y - _mainChar.Speed);
            }
        }

        public virtual void Update()
        {
            _mainChar.Update(_offset);

            for (int idx = 0; idx < _projectiles.Count(); idx++)
            {
                _projectiles[idx].Update(_offset, _npcs.ToList<Unit>());
                if (_projectiles[idx].Done)
                {
                    _projectiles.RemoveAt(idx);
                    idx--;
                }
            }

            for (int idx = 0; idx < _npcs.Count(); idx++)
            {
                _npcs[idx].Update(_offset, _mainChar);
                if (_npcs[idx].Dead)
                {
                    _nKilled++;
                    _npcs.RemoveAt(idx);
                    idx--;
                }
            }

            _ui.Update();
        }

        public virtual void Draw(Vector2 a_offset)
        {
            _mainChar.Draw(_offset);

            foreach (Projectile proj in _projectiles)
            {
                proj.Draw(_offset);
            }

            foreach (Npc npc in _npcs)
            {
                npc.Draw(_offset);
            }

            _ui.Draw(this);
        }
    }
}
