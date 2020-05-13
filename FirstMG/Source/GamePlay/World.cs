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
        private UI _ui;
        private List<Projectile> _projectiles = new List<Projectile>();
        private List<Npc> _npcs = new List<Npc>();
        private List<Terrain> _terrains = new List<Terrain>();

        public int _nKilled;
        private Engine.MyTimer _enemyTimer = new Engine.MyTimer(1000);
        public MainChar MainCharacter { get; private set; }
        Engine.PassObject ResetWorld;
        Engine.PassObject ChangeGameState;

        public World(Engine.PassObject a_resetWorld, Engine.PassObject a_changeGameState)
        {
            ResetWorld = a_resetWorld;
            ChangeGameState = a_changeGameState;

            GameGlobals.PassProjectile = AddProjectile;
            GameGlobals.PassNpc        = AddNpc;
            GameGlobals.CheckScroll    = CheckScroll;
            GameGlobals.paused         = false;

            _nKilled = 0;
            _offset  = new Vector2(0, 0);
            _ui      = new UI(ResetWorld, ChangeGameState);

            LoadData(1);
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
                _offset = new Vector2(_offset.X + MainCharacter.Speed, _offset.Y);
            }
            if (tmpPos.X > (-_offset.X + (Engine.Globals.ScreenWidth * .7f)))
            {
                _offset = new Vector2(_offset.X - MainCharacter.Speed, _offset.Y);
            }
            if (tmpPos.Y < (-_offset.Y + (Engine.Globals.ScreenHeight * .3f)))
            {
                _offset = new Vector2(_offset.X, _offset.Y + MainCharacter.Speed);
            }
            if (tmpPos.Y > (-_offset.Y + (Engine.Globals.ScreenHeight * .7f)))
            {
                _offset = new Vector2(_offset.X, _offset.Y - MainCharacter.Speed);
            }
        }

        public virtual void LoadData(int a_level)
        {
            XDocument xml = XDocument.Load("XML\\Levels\\Level" + a_level + ".xml");

            // Load MainChar
            String mcAsset = "Assets\\nice_sprite";
            Vector2 mcPosition = new Vector2(Engine.Globals.ScreenWidth / 2, Engine.Globals.ScreenHeight / 2);
            XElement mainCharXML = xml.Element("Root").Element("Unit").Element("MainChar");
            if (mainCharXML != null)
            {
                if (mainCharXML.Element("asset") != null)
                {
                    mcAsset = mainCharXML.Element("asset").Value;
                }
                if (mainCharXML.Element("position") != null)
                {
                    mcPosition = new Vector2(Convert.ToInt32(mainCharXML.Element("position").Value), Engine.Globals.ScreenHeight / 2);
                }
            }
            MainCharacter = new MainChar(mcAsset, /* position */ mcPosition, /* dimension */ new Vector2(57, 50), /* frames */ new Vector2(1,1));


            // Load Terrain
            XElement terrainXML = xml.Element("Root").Element("Terrain");
            if (terrainXML != null)
            {
                List<XElement> dirtRows = (from t in terrainXML.Descendants("DirtRow") select t).ToList<XElement>();

                foreach (XElement dirtRow in dirtRows)
                {
                    int yVal = Convert.ToInt32(dirtRow.Element("Position").Element("y").Value);
                    int startingXVal = Convert.ToInt32(dirtRow.Element("Position").Element("starting_x").Value);
                    int finalXVal = Convert.ToInt32(dirtRow.Element("Position").Element("final_x").Value);

                    _terrains.Add(new Dirt("Assets\\dirt_left", /* position */ new Vector2(startingXVal, yVal)));
                    for (int i = startingXVal+16; i <= finalXVal; i += 16)
                    {
                        _terrains.Add(new Dirt("Assets\\dirt_mid", /* position */ new Vector2(i, yVal)));
                    }
                    _terrains.Add(new Dirt("Assets\\dirt_right", /* position */ new Vector2(finalXVal - (finalXVal%16) + 16 , yVal)));
                }
            }
        }

        public virtual void Update()
        {
            if (!MainCharacter.Dead && GameGlobals.paused == false)
            {
                //_enemyTimer.UpdateTimer();
                //if (_enemyTimer.Test())
                //{
                //    int randomX = Engine.Globals.RandomNumber(0, 1000);
                //    int randomY = Engine.Globals.RandomNumber(0, 1000);
                //    Console.WriteLine(randomX + " , " + randomY);
                //    GameGlobals.PassNpc(new FirstEnemy(new Vector2(randomX,randomY)));;
                //    _enemyTimer.Reset();
                //}

                MainCharacter.Update(_offset, _terrains);

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
                    _npcs[idx].Update(_offset, MainCharacter);
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
                if (Engine.Globals.MyKeyboard.GetNewPress("Enter"))
                {
                    ResetWorld(null);
                }
            }

            if (Engine.Globals.MyKeyboard.GetNewPress("Escape") && !MainCharacter.Dead)
            {
                GameGlobals.paused = !GameGlobals.paused;
            }

            _ui.Update(this);
        }

        public virtual void Draw(Vector2 a_offset)
        {
            foreach (Terrain terrain in _terrains)
            {
                terrain.Draw(new Vector2(_offset.X, _offset.Y + (terrain.Dimension.Y/2 + MainCharacter.Dimension.Y/2)));
            }

            MainCharacter.Draw(_offset);

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
