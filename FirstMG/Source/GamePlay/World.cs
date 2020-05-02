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
        private List<Projectile> _projectiles = new List<Projectile>();

        public World()
        {
            _mainChar = new MainChar("Assets\\pixo", /* position */ new Vector2(300,600), /* dimension */ new Vector2(150,150));

            GameGlobals.PassProjectile = AddProjectile;

            _offset = new Vector2(0, 0);
        }

        public virtual void AddProjectile(object a_projectile)
        {
            _projectiles.Add((Projectile)a_projectile);
        }

        public virtual void Update()
        {
            _mainChar.Update();

            for (int idx = 0; idx < _projectiles.Count(); idx++)
            {
                _projectiles[idx].Update(_offset, null);
                if (_projectiles[idx].Done)
                {
                    _projectiles.RemoveAt(idx);
                    idx--;
                }
            }
        }

        public virtual void Draw(Vector2 a_offset)
        {
            _mainChar.Draw(a_offset);

            foreach (Projectile proj in _projectiles)
            {
                proj.Draw(_offset); // Or a_offset?? confusing...
            }
        }
    }
}
