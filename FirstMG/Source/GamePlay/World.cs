#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
        private List<Terrain> _terrains = new List<Terrain>();

        public int _nKilled;

        Engine.PassObject ResetWorld;

        public World(Engine.PassObject a_resetWorld)
        {
            ResetWorld = a_resetWorld;

            GameGlobals.PassProjectile = AddProjectile;
            GameGlobals.PassNpc = AddNpc;
            GameGlobals.CheckScroll = CheckScroll;

            _nKilled = 0;
            _offset  = new Vector2(0, 0);
            _ui      = new UI();

            LoadData(1);
        }

        public MainChar MainCharacter
        {
            get { return _mainChar; }
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

        public virtual void LoadData(int a_level)
        {
            XDocument xml = XDocument.Load("XML\\Levels\\Level" + a_level + ".xml");

            String mcAsset = "Assets\\pixo";
            Vector2 mcPosition = new Vector2(Engine.Globals.ScreenWidth / 2, GameGlobals.FloorLevel);

            if (xml.Element("Root").Element("Unit").Element("MainChar") != null)
            {
                if (xml.Element("Root").Element("Unit").Element("MainChar").Element("asset") != null)
                {
                    mcAsset = xml.Element("Root").Element("Unit").Element("MainChar").Element("asset").Value;
                }
                if (xml.Element("Root").Element("Unit").Element("MainChar").Element("position") != null)
                {
                    mcPosition = new Vector2(Convert.ToInt32(xml.Element("Root").Element("Unit").Element("MainChar").Element("position").Value), GameGlobals.FloorLevel);
                }
            }

            _mainChar = new MainChar(mcAsset, /* position */ mcPosition, /* dimension */ new Vector2(150, 150));

            for (int i = 0; i < 50; i++)
            {
                _terrains.Add(new Dirt(/* position */ new Vector2((i*50), GameGlobals.FloorLevel + 25)));
            }
        }

        public virtual void Update()
        {
            if (!_mainChar.Dead)
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
            }
            else
            {
                if (Engine.Globals.MyKeyboard.GetPress("Enter"))
                {
                    ResetWorld(null);
                }
            }

            _ui.Update(this);
        }

        public virtual void Draw(Vector2 a_offset)
        {
            foreach (Terrain terrain in _terrains)
            {
                terrain.Draw(_offset);
            }

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
