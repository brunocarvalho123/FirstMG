#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using FirstMG.Source.Engine;
using Microsoft.Xna.Framework;
#endregion

namespace FirstMG.Source.GamePlay
{
    class World
    {
        private Vector2 _offset;
        private UI _ui;
        private List<Projectile> _projectiles      = new List<Projectile>();
        private List<Projectile> _enemyProjectiles = new List<Projectile>();
        private List<Npc> _npcs = new List<Npc>();

        private SquareGrid _grid;

        private TiledBackground _tiledBackground;

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

            LoadData(1);
            
            _ui = new UI(ResetWorld, ChangeGameState);
            _tiledBackground = new TiledBackground("Assets\\UI\\standard_snow", new Vector2(-100,-100), new Vector2(120,100), new Vector2(_grid.TotalPhysicalDims.X + 100, _grid.TotalPhysicalDims.Y + 100));
        }

        public virtual void AddProjectile(object a_projectile)
        {
            _enemyProjectiles.Add((Projectile)a_projectile);
        }

        public virtual void AddNpc(object a_npc)
        {
            _npcs.Add((Npc)a_npc);
        }

        public virtual void CheckScroll(object a_position)
        {
            Vector2 tmpPos = (Vector2)a_position;
            float diff = 0;
            float maxHSpeed = 8.0f;
            float maxVSpeed = 20.0f;

            if ((tmpPos.X < (-_offset.X + (Engine.Globals.ScreenWidth * .25f))) )
            {
                diff = -_offset.X + (Engine.Globals.ScreenWidth * .25f) - tmpPos.X;
                _offset = new Vector2(_offset.X + Math.Min(diff, maxHSpeed), _offset.Y);
            }
            if (tmpPos.X > (-_offset.X + (Engine.Globals.ScreenWidth * .75f)) )
            {
                diff = tmpPos.X - (-_offset.X + Engine.Globals.ScreenWidth * .75f);
                _offset = new Vector2(_offset.X - Math.Min(maxHSpeed, diff), _offset.Y);
            }
            if (tmpPos.Y < (-_offset.Y + (Engine.Globals.ScreenHeight * .40f)))
            {
                diff = -_offset.Y + (Engine.Globals.ScreenHeight * .40f) - tmpPos.Y;
                _offset = new Vector2(_offset.X, _offset.Y + Math.Min(maxVSpeed, diff));
            }
            if (tmpPos.Y > (-_offset.Y + (Engine.Globals.ScreenHeight * .60f)))
            {
                diff = tmpPos.Y - (-_offset.Y + Engine.Globals.ScreenHeight * .60f);
                _offset = new Vector2(_offset.X, _offset.Y - Math.Min(maxVSpeed, diff));
            }

            //_offset = new Vector2((float)Math.Floor(_offset.X), (float)Math.Floor(_offset.Y));
        }

        public virtual void LoadData(int a_level)
        {
            XDocument xml = XDocument.Load("XML\\Levels\\Level" + a_level + ".xml");

            // Load MainChar
            String mcAsset = "Assets\\MainChar\\full_animation";
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
                    mcPosition = new Vector2(Convert.ToInt32(mainCharXML.Element("position").Value), 400);
                }
            }
            MainCharacter = new MainChar(mcAsset, /* position */ mcPosition, /* dimension */ new Vector2(50, 80), /* frames */ new Vector2(1,1));

            AddNpc(new EvilOnion(new Vector2(1300, 200), new Vector2(1, 1)));


            // Load map
            XElement map = XDocument.Load("XML\\Maps\\fst_map.xml").Element("map");
            Vector2 mapSize = new Vector2(Convert.ToInt32(map.Attribute("width").Value), Convert.ToInt32(map.Attribute("height").Value));
            Vector2 tileDims = new Vector2(Convert.ToInt32(map.Attribute("tilewidth").Value), Convert.ToInt32(map.Attribute("tileheight").Value));

            _grid = new SquareGrid(tileDims, new Vector2(0, 0), mapSize, map);
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

                MainCharacter.Update(_offset, _grid);

                for (int idx = 0; idx < _projectiles.Count(); idx++)
                {
                    _projectiles[idx].Update(_offset, _npcs.ToList<Unit>());
                    if (_projectiles[idx].Done)
                    {
                        _projectiles.RemoveAt(idx);
                        idx--;
                    }
                }

                List<Unit> mainCharList = new List<Unit>();
                mainCharList.Add(MainCharacter);
                for (int idx = 0; idx < _enemyProjectiles.Count(); idx++)
                {
                    _enemyProjectiles[idx].Update(_offset, mainCharList);
                    if (_enemyProjectiles[idx].Done)
                    {
                        _enemyProjectiles.RemoveAt(idx);
                        idx--;
                    }
                }

                for (int idx = 0; idx < _npcs.Count(); idx++)
                {
                    _npcs[idx].Update(_offset, MainCharacter, _grid);
                    if (_npcs[idx].Dead)
                    {
                        GridLocation location = _grid.GetSlotFromPixel(_npcs[idx].Position, Vector2.Zero);
                        location.Filled = false;
                        location.Impassible = false;

                        _nKilled++;
                        _npcs.RemoveAt(idx);
                        idx--;
                    }
                }
            }
            else
            {
                if (Globals.MyKeyboard.GetNewPress("Enter"))
                {
                    ResetWorld(null);
                }
            }

            if (_grid != null)
            {
                _grid.Update(_offset);
            }

            if (Globals.MyKeyboard.GetNewPress("Escape") && !MainCharacter.Dead)
            {
                GameGlobals.paused = !GameGlobals.paused;
            }

            if (Globals.MyKeyboard.GetNewPress("G"))
            {
                _grid.ShowGrid = !_grid.ShowGrid;
            }

            _ui.Update(this);
        }

        public virtual void Draw(Vector2 a_offset)
        {
            _tiledBackground.Draw(_offset);
            _grid.DrawGrid(_offset);


            MainCharacter.Draw(_offset);

            foreach (Projectile proj in _projectiles)
            {
                proj.Draw(_offset);
            }

            foreach (Projectile proj in _enemyProjectiles)
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
