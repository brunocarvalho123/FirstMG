#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMG.Source.Engine;
using Microsoft.Xna.Framework;
#endregion

namespace FirstMG.Source.GamePlay
{
    class Demon : Npc
    {

        float _originalXPos;

        public Demon(Vector2 a_position, Vector2 a_dimension) 
            : base("Assets\\Enemies\\animacao_demon", a_position, a_dimension, new Vector2(6, 16))
        {
            Health = 4.0f;
            HealthMax = Health;

            MovSpeed = 2f;
            MaxHSpeed = 3f;

            _originalXPos = a_position.X;

            Ori = GameGlobals.Orientation.LEFT;

            BoundingBoxOffset = new Vector4(.10f, .10f, .10f, .8f);

            FrameAnimations = true;
            CurrentAnimation = 0;
            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 0), 6, 256, 0, "IdleR"));
            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 1), 6, 256, 0, "IdleL"));
            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 2), 6, 128, 0, "MoveR"));
            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 3), 6, 128, 0, "MoveL"));

            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 4), 6, 128, 1, 4, NormalAttack, "AttR"));
            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 5), 6, 128, 1, 4, NormalAttack, "AttL"));

            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 6), 3, 100, 1, "HurtR"));
            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 7), 3, 100, 1, "HurtL"));

            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 12), 4, 100, 1, 4, Die, "DieR"));
            FrameAnimationList.Add(new FrameAnimation(new Vector2(FrameSize.X, FrameSize.Y), Frames, new Vector2(0, 13), 4, 100, 1, 4, Die, "DieL"));
        }

        public void NormalAttack(object a_obj)
        {
            GameGlobals.ExecuteEnemyAttack(new Attack(this, 60, 1, Ori));
        }

        public void Die(object a_obj)
        {
            Dead = true;
        }

        public override void GetHit(float a_damage)
        {
            Health -= a_damage;
            if (Health <= 0)
            {
                _state = State.DYING;
            }
            else
            {
                CurrentAnimation = 0;
                _state = State.HURTING;
            }
        }

        public void MoveTowards(float a_xPos)
        {
            if (Position.X > a_xPos)
            {
                HSpeed -= MovSpeed;

                if (Position.X + HSpeed < a_xPos)
                {

                    HSpeed = -(Position.X - a_xPos);
                }
            }
            else if (Position.X < a_xPos)
            {
                HSpeed += MovSpeed;

                if (Position.X + HSpeed > a_xPos)
                {
                    HSpeed = a_xPos - Position.X;
                }
            }
            else
            {
                HSpeed = 0;
                return;
            }
            _state = State.RUNNING;
        }

        protected override void AI(MainChar a_mainChar)
        {
            if (_state == State.ATTACKING || _state == State.DYING || _state == State.HURTING) return;

            float mcDist = Globals.GetDistance(Position, a_mainChar.Position);

            if (mcDist < 70)
            {
                CurrentAnimation = 0;
                _state = State.ATTACKING;
                return;
            }
            else if (mcDist < 400)
            {
                MoveTowards(a_mainChar.Position.X);
            }
            else
            {
                if (Math.Abs(Position.X -_originalXPos) < 1)
                {
                    _state = State.STANDING;
                }
                else
                {
                    MoveTowards(_originalXPos);
                }
            }
        }

        public void StartAnimation()
        {
            string ori = "R";
            if (Ori == GameGlobals.Orientation.LEFT) ori = "L";

            switch (_state)
            {
                case State.ATTACKING:
                    SetAnimationByName("Att" + ori);
                    if (FrameAnimationList[CurrentAnimation].IsAtEnd()) _state = State.STANDING;
                    break;
                case State.HURTING:
                    SetAnimationByName("Hurt" + ori);
                    if (FrameAnimationList[CurrentAnimation].IsAtEnd()) _state = State.STANDING;
                    break;
                case State.RUNNING:
                    SetAnimationByName("Move" + ori);
                    break;
                case State.DYING:
                    SetAnimationByName("Die" + ori);
                    break;
                case State.STANDING:
                    SetAnimationByName("Idle" + ori);
                    break;
                default:
                    _state = State.STANDING;
                    SetAnimationByName("Idle" + ori);
                    break;
            }
        }

        public override void Update(Vector2 a_offset, MainChar a_mainChar, SquareGrid a_grid)
        {
            a_grid.GetSlotFromPixel(Position, Vector2.Zero).Filled = false;

            AI(a_mainChar);

            StartAnimation();

            base.Update(a_offset, a_mainChar, a_grid);

            a_grid.GetSlotFromPixel(Position, Vector2.Zero).Filled = true;
        }

        public override void Draw(Vector2 a_offset)
        {
            base.Draw(a_offset);
        }
    }
}
