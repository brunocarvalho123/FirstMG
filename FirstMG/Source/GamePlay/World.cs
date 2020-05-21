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
        private List<Projectile> _projectiles = new List<Projectile>();
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
            _projectiles.Add((Projectile)a_projectile);
        }

        public virtual void AddNpc(object a_npc)
        {
            _npcs.Add((Npc)a_npc);
        }

        public virtual void CheckScroll(object a_position)
        {
            Vector2 tmpPos = (Vector2)a_position;
            if ((tmpPos.X < (-_offset.X + (Engine.Globals.ScreenWidth * .3f))) && tmpPos.X > (_grid.TotalPhysicalDims.X * .2f))
            {
                _offset = new Vector2(_offset.X + MainCharacter.MovSpeed, _offset.Y);
            }
            if (tmpPos.X > (-_offset.X + (Engine.Globals.ScreenWidth * .7f)) && tmpPos.X < (_grid.TotalPhysicalDims.X * .8f))
            {
                _offset = new Vector2(_offset.X - MainCharacter.MovSpeed, _offset.Y);
            }
            if (tmpPos.Y < (-_offset.Y + (Engine.Globals.ScreenHeight * .3f)))
            {
                _offset = new Vector2(_offset.X, _offset.Y - MainCharacter.VSpeed);
            }
            if (tmpPos.Y > (-_offset.Y + (Engine.Globals.ScreenHeight * .7f)))
            {
                _offset = new Vector2(_offset.X, _offset.Y - MainCharacter.VSpeed);
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
            MainCharacter = new MainChar(mcAsset, /* position */ mcPosition, /* dimension */ new Vector2(49, 49), /* frames */ new Vector2(1,1));

            _grid = new SquareGrid(new Vector2(50, 50), new Vector2(0, 0), new Vector2(Globals.ScreenWidth + 200, Globals.ScreenHeight + 200), xml.Element("Root").Element("GridItem"));
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

                for (int idx = 0; idx < _npcs.Count(); idx++)
                {
                    _npcs[idx].Update(_offset, MainCharacter, _grid);
                    if (_npcs[idx].Dead)
                    {
                        GridLocation location = _grid.GetSlotFromPixel(_npcs[idx].Position, Vector2.Zero);
                        location.Filled = false;

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

            foreach (Npc npc in _npcs)
            {
                npc.Draw(_offset);
            }

            _ui.Draw(this);
        }
    }
}
