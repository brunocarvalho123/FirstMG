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

            GameGlobals.PassProjectile     = AddProjectile;
            GameGlobals.PassNpc            = AddNpc;
            GameGlobals.CheckScroll        = CheckScroll;
            GameGlobals.ResetScroll        = ResetScroll;
            GameGlobals.ExecuteAttack      = ExecuteAttack;
            GameGlobals.ExecuteEnemyAttack = ExecuteEnemyAttack;
            GameGlobals.GetClosestNpc      = GetClosestNpc;
            GameGlobals.paused             = false;

            _nKilled = 0;
            _offset  = new Vector2(0, 0);

            LoadData(1);
            
            _ui = new UI(ResetWorld, ChangeGameState);
            _tiledBackground = new TiledBackground("Assets\\UI\\swamp_background", new Vector2(0,0), new Vector2(1920,1080), new Vector2(_grid.TotalPhysicalDims.X, _grid.TotalPhysicalDims.Y));
        }

        public virtual void AddProjectile(object a_projectile)
        {
            _enemyProjectiles.Add((Projectile)a_projectile);
        }

        public virtual void AddNpc(object a_npc)
        {
            _npcs.Add((Npc)a_npc);
        }

        public virtual bool LandAttack(Attack a_attack, Unit a_target)
        {
            Vector4 ownerBB  = a_attack.Owner.BoundingBox;
            Vector4 targetBB = a_target.BoundingBox;
            switch (a_attack.Ori)
            {
                case GameGlobals.Orientation.LEFT:
                    float maxAttX = ownerBB.X - a_attack.Range;
                    if (targetBB.Y >= maxAttX &&
                        a_attack.Owner.Position.X >= targetBB.Y &&
                        ownerBB.Z <= targetBB.W &&
                        ownerBB.W >= targetBB.Z)
                    {
                        return true;
                    }
                    break;
                case GameGlobals.Orientation.RIGHT:
                    float maxAttY = ownerBB.Y + a_attack.Range;
                    if (targetBB.X <= maxAttY &&
                        a_attack.Owner.Position.X <= targetBB.X &&
                        ownerBB.Z <= targetBB.W &&
                        ownerBB.W >= targetBB.Z)
                    {
                        return true;
                    }
                    break;
                case GameGlobals.Orientation.BOT:
                    float maxAttW = ownerBB.W + a_attack.Range;
                    if (targetBB.Z <= maxAttW &&
                        a_attack.Owner.Position.Y <= targetBB.Z &&
                        ownerBB.X <= targetBB.Y &&
                        ownerBB.Y >= targetBB.X)
                    {
                        return true;
                    }
                    break;
                case GameGlobals.Orientation.TOP:
                    float maxAttZ = ownerBB.Z - a_attack.Range;
                    if (targetBB.W >= maxAttZ &&
                        a_attack.Owner.Position.Y >= targetBB.W &&
                        ownerBB.X <= targetBB.Y &&
                        ownerBB.Y >= targetBB.X)
                    {
                        return true;
                    }
                    break;
                default:
                    break;
            }

            return false;
        }

        public virtual void ExecuteAttack(object a_attack)
        {
            Attack attack = (Attack)a_attack;
            foreach (Npc npc in _npcs)
            {
                if (LandAttack(attack, npc))
                {
                    npc.GetHit(attack.Power);
                    attack.LandedHit = true;
                    return;
                }
            }
        }

        public virtual void ExecuteEnemyAttack(object a_attack)
        {
            Attack attack = (Attack)a_attack;
            if (LandAttack(attack, MainCharacter))
            {
                MainCharacter.GetHit(attack.Power);
                attack.LandedHit = true;
                return;
            }
        }

        public virtual object GetClosestNpc(object a_unit)
        {
            Unit unit = (Unit)a_unit;

            Npc closestNpc = null;
            float bestDist = Int32.MaxValue;
            foreach (Npc npc in _npcs)
            {
                float distDiff = Globals.GetDistance(unit.Position, npc.Position);
                if (distDiff <= bestDist)
                {
                    closestNpc = npc;
                    bestDist = distDiff;
                }
            }
            return closestNpc;
        }

        public virtual void ResetScroll(object a_position)
        {
            Vector2 tmpPos = (Vector2)a_position;
            float diff = 0;

            if (tmpPos.X + (Engine.Globals.ScreenWidth * .5f) < _grid.TotalPhysicalDims.X && tmpPos.X - (Engine.Globals.ScreenWidth * .5f) > _grid.StartingPhysicalPos.X)
            {
                diff = tmpPos.X - (-_offset.X + Engine.Globals.ScreenWidth * .5f);
                _offset = new Vector2(_offset.X - diff, _offset.Y);
            }
            else if (tmpPos.X + (Engine.Globals.ScreenWidth * .5f) >= _grid.TotalPhysicalDims.X && tmpPos.X - (Engine.Globals.ScreenWidth * .5f) > _grid.StartingPhysicalPos.X)
            {
                _offset = new Vector2(-_grid.TotalPhysicalDims.X + (Engine.Globals.ScreenWidth), _offset.Y);
            }
            else if (tmpPos.X - (Engine.Globals.ScreenWidth * .5f) <= _grid.StartingPhysicalPos.X)
            {
                _offset = new Vector2(_grid.StartingPhysicalPos.X, _offset.Y);
            }
        }

        public virtual void CheckScroll(object a_position)
        {
            Vector2 tmpPos = (Vector2)a_position;
            float diff = 0;
            float maxHSpeed = 15.0f;

            if ((tmpPos.X < (-_offset.X + (Engine.Globals.ScreenWidth * .5f))) && (tmpPos.X - (Engine.Globals.ScreenWidth * .5f) > _grid.StartingPhysicalPos.X) )
            {
                diff = -_offset.X + (Engine.Globals.ScreenWidth * .5f) - tmpPos.X;
                _offset = new Vector2(_offset.X + Math.Min(diff, maxHSpeed), _offset.Y);
            }
            if (tmpPos.X > (-_offset.X + (Engine.Globals.ScreenWidth * .5f)) && (tmpPos.X + (Engine.Globals.ScreenWidth * .5f) < _grid.TotalPhysicalDims.X))
            {
                diff = tmpPos.X - (-_offset.X + Engine.Globals.ScreenWidth * .5f);
                _offset = new Vector2(_offset.X - Math.Min(maxHSpeed, diff), _offset.Y);
            }
            //if (tmpPos.Y < (-_offset.Y + (Engine.Globals.ScreenHeight * .5f)))
            //{
            //    diff = -_offset.Y + (Engine.Globals.ScreenHeight * .5f) - tmpPos.Y;
            //    _offset = new Vector2(_offset.X, _offset.Y + Math.Min(maxVSpeed, diff));
            //}
            //if (tmpPos.Y > (-_offset.Y + (Engine.Globals.ScreenHeight * .5f)))
            //{
            //    diff = tmpPos.Y - (-_offset.Y + Engine.Globals.ScreenHeight * .5f);
            //    _offset = new Vector2(_offset.X, _offset.Y - Math.Min(maxVSpeed, diff));
            //}

            _offset = new Vector2(_offset.X, 0);
        }

        public virtual void LoadData(int a_level)
        {
            int scale = 2;
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
            MainCharacter = new MainChar(mcAsset, /* position */ mcPosition, /* dimension */ new Vector2(76 * scale, 64 * scale), /* frames */ new Vector2(6,14));

            // Load map
            XElement map = XDocument.Load("XML\\Maps\\swamp.xml").Element("map");

            XElement enemies = map.Elements("objectgroup").Where(elem => (string)elem.Attribute("name") == "EnemyLayer").First();
            foreach (XElement enemy in enemies.Elements("object"))
            {
                float posX = (float)Convert.ToDouble(enemy.Attribute("x").Value);
                float posY = (float)Convert.ToDouble(enemy.Attribute("y").Value);
                float enemyWidth = (float)Convert.ToDouble(enemy.Attribute("width").Value);
                float enemyHeight = (float)Convert.ToDouble(enemy.Attribute("height").Value);
                Type enemyType = Type.GetType("FirstMG.Source.GamePlay." + enemy.Attribute("type").Value);

                if (enemyType != null)
                {
                    AddNpc((Npc)(Activator.CreateInstance(enemyType, new Vector2(posX * scale, posY * scale), new Vector2(enemyWidth * scale, enemyHeight * scale))));
                }
            }

            Vector2 mapSize = new Vector2(Convert.ToInt32(map.Attribute("width").Value), Convert.ToInt32(map.Attribute("height").Value));
            Vector2 tileDims = new Vector2(Convert.ToInt32(map.Attribute("tilewidth").Value) * scale, Convert.ToInt32(map.Attribute("tileheight").Value) * scale);
            _grid = new SquareGrid(tileDims, new Vector2(0, 0), mapSize, map, 0.99f);
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
            _tiledBackground.Draw(Vector2.Zero);

            _grid.DrawGrid(_offset, "BackgroundLayer");

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

            _grid.DrawGrid(_offset, "TileLayer");

            _ui.Draw(this);
        }
    }
}
