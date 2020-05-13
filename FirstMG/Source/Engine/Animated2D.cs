#region Includes
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace FirstMG.Source.Engine
{
    class Animated2D : Asset2D
    {
        private List<FrameAnimation> _frameAnimationList = new List<FrameAnimation>();

        private Vector2 _frames;
        private Color   _color;
        private bool    _frameAnimations;
        private int     _currentAnimation = 0;

        public Animated2D(string a_path, Vector2 a_pos, Vector2 a_dims, Vector2 a_frames, Color a_color) : base(a_path, a_pos, a_dims)
        {
            Frames = new Vector2(a_frames.X, a_frames.Y);

            _color = a_color;
        }

        #region Properties
        public Vector2 Frames
        {
            protected set
            {
                _frames = value;
                if(Asset != null)
                {
                    FrameSize = new Vector2(Asset.Bounds.Width / _frames.X, Asset.Bounds.Height / _frames.Y);
                }
            }
            get { return _frames; }
        }
        public bool FrameAnimations
        {
            get { return _frameAnimations;  }
            protected set { _frameAnimations = value; }
        }
        public int CurrentAnimation
        {
            get { return _currentAnimation; }
            protected set { _currentAnimation = value; }
        }
        public List<FrameAnimation> FrameAnimationList
        {
            get { return _frameAnimationList; }
            protected set { _frameAnimationList = value; }
        }
        #endregion


        public override void Update(Vector2 a_offset)
        {
            if(_frameAnimations && _frameAnimationList != null && _frameAnimationList.Count > _currentAnimation)
            {
                _frameAnimationList[_currentAnimation].Update();
            }

            base.Update(a_offset);
        }

        public virtual int GetAnimationFromName(string a_animationName)
        {
            for(int i=0; i < _frameAnimationList.Count; i++)
            {
                if(_frameAnimationList[i].Name == a_animationName)
                {
                    return i;
                }
            }

            return -1;
        }

        public virtual void SetAnimationByName(string a_name)
        {
            int tempAnimation = GetAnimationFromName(a_name);

            if(tempAnimation != -1)
            {
                if(tempAnimation != _currentAnimation)
                {
                    _frameAnimationList[tempAnimation].Reset();
                }

                _currentAnimation = tempAnimation;
                
            }
        }

        public override void Draw(Vector2 a_offset)
        {

            if(_frameAnimations && _frameAnimationList[_currentAnimation].Frames > 0)
            {
                //Globals.spriteBatch.Draw(myModel, new Rectangle((int)(pos.X+screenShift.X), (int)(pos.Y+screenShift.Y), (int)dims.X, (int)dims.Y), new Rectangle((int)((currentFrame.X-1)*dims.X), (int)((currentFrame.Y-1)*dims.Y), (int)(currentFrame.X*dims.X), (int)(currentFrame.Y*dims.Y)), color, rot, new Vector2(myModel.Bounds.Width/2, myModel.Bounds.Height/2), new SpriteEffects(), 0);
                _frameAnimationList[_currentAnimation].Draw(Asset, Dimension, FrameSize, a_offset, Position, Rotation, _color, new SpriteEffects());

            }
            else
            {
                base.Draw(a_offset);
            }
        }


    }
}
