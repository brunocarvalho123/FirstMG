#region Includes
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace FirstMG.Source.Engine
{
    public class FrameAnimation
    {
        private bool    _hasFired;
        private int     _frames;
        private int     _currentFrame;
        private int     _maxPasses;
        private int     _currentPass;
        private int     _fireFrame;
        private string  _name;
        private Vector2 _sheet;
        private Vector2 _startFrame;
        private Vector2 _sheetFrame;
        private Vector2 _spriteDims;
        private MyTimer _frameTimer;

        public PassObject FireAction;

        public FrameAnimation(Vector2 a_spriteDims, Vector2 a_sheetDims, Vector2 a_start, int a_totalFrames, int a_timePerFrame, int a_maxPasses, string a_name = "")
        {
            _spriteDims   = a_spriteDims;
            _sheet        = a_sheetDims;
            _startFrame   = a_start;
            _sheetFrame   = new Vector2(a_start.X, a_start.Y);
            _frames       = a_totalFrames;
            _currentFrame = 0;
            _frameTimer   = new MyTimer(a_timePerFrame);
            _maxPasses    = a_maxPasses;
            _currentPass  = 0;
            _name         = a_name;
            _hasFired     = false;

            _fireFrame = 0;
            FireAction = null;
        }

        public FrameAnimation(Vector2 a_spriteDims, Vector2 a_sheetDims, Vector2 a_start, int a_totalFrames, int a_timePerFrame, int a_maxPasses, int a_fireFrame, PassObject a_fireAction, string a_name = "")
        {
            _spriteDims   = a_spriteDims;
            _sheet        = a_sheetDims;
            _startFrame   = a_start;
            _sheetFrame   = new Vector2(a_start.X, a_start.Y);
            _frames       = a_totalFrames;
            _currentFrame = 0;
            _frameTimer   = new MyTimer(a_timePerFrame);
            _maxPasses    = a_maxPasses;
            _currentPass  = 0;
            _name         = a_name;
            _hasFired     = false;
            
            _fireFrame = a_fireFrame;
            FireAction = a_fireAction;
        }

        #region Properties
        public int Frames
        {
            get { return _frames; }
        }
        public int CurrentFrame
        {
            get { return _currentFrame; }
            set { _currentFrame = value; }
        }
        public int CurrentPass
        {
            get { return _currentPass; }
            set { _currentPass = value; }
        }
        public int MaxPasses
        {
            get { return _maxPasses; }
        }
        public string Name
        {
            get { return Name; }
        }
        #endregion

        public void Update()
        {

            if(Frames > 1)
            {
                _frameTimer.UpdateTimer();
                if (_frameTimer.Test() && (MaxPasses == 0 || MaxPasses > CurrentPass))
                {
                    CurrentFrame++;
                    if(CurrentFrame >= Frames)
                    {
                        CurrentPass++;
                    }
                    if(MaxPasses == 0 || MaxPasses > CurrentPass)
                    {
                        _sheetFrame.X += 1;
                    
                        if(_sheetFrame.X >= _sheet.X)
                        {
                            _sheetFrame.X = 0;
                            _sheetFrame.Y += 1;
                        }
                        if(CurrentFrame >= _frames)
                        {
                            CurrentFrame = 0;
                            _hasFired = false;
                            _sheetFrame = new Vector2(_startFrame.X, _startFrame.Y);
                        }
                    }
                    _frameTimer.Reset();
                }
            }

            if(FireAction != null && _fireFrame == CurrentFrame && !_hasFired)
            {
                FireAction(null);
                _hasFired = true;
            }
        }

        public void Reset()
        {
            CurrentFrame = 0;
            CurrentPass = 0;
            _sheetFrame = new Vector2(_startFrame.X, _startFrame.Y);
            _hasFired = false;
        }

        public bool IsAtEnd()
        {
            if(CurrentFrame + 1 >= Frames)
            {
                return true;
            }
            return false;
        }


        public void Draw(Texture2D a_asset, Vector2 a_dims, Vector2 a_imageDims, Vector2 a_offset, Vector2 a_pos, float a_rotation, Color a_color, SpriteEffects a_spriteEffect)
        {
            Globals.MySpriteBatch.Draw(a_asset, new Rectangle((int)((a_pos.X + a_offset.X)), (int)((a_pos.Y + a_offset.Y)), (int)Math.Ceiling(a_dims.X), (int)Math.Ceiling(a_dims.Y)), new Rectangle((int)(_sheetFrame.X * a_imageDims.X), (int)(_sheetFrame.Y * a_imageDims.Y), (int)a_imageDims.X, (int)a_imageDims.Y), a_color, a_rotation, a_imageDims / 2, a_spriteEffect, 0);
        }

    }
}
