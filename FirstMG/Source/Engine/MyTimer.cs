using System;
using System.Xml.Linq;

namespace FirstMG.Source.Engine
{
    public class MyTimer
    {
        private bool _goodToGo;
        private int _mSec;
        private TimeSpan _timer = new TimeSpan();
        

        public MyTimer(int a_ms)
        {
            _goodToGo = false;
            _mSec = a_ms;
        }
        public MyTimer(int m, bool a_startLoaded)
        {
            _goodToGo = a_startLoaded;
            _mSec = m;
        }

        public int MSec
        {
            get { return _mSec; }
            set { _mSec = value; }
        }
        public int Timer
        {
            get { return (int)_timer.TotalMilliseconds; }
        }

        

        public void UpdateTimer()
        {
            _timer += Globals.GlobalGameTime.ElapsedGameTime;
        }

        public void UpdateTimer(float a_speed)
        {
            _timer += TimeSpan.FromTicks((long)(Globals.GlobalGameTime.ElapsedGameTime.Ticks * a_speed));
        }

        public virtual void AddToTimer(int a_ms)
        {
            _timer += TimeSpan.FromMilliseconds((long)(a_ms));
        }

        public bool Test()
        {
            if(_timer.TotalMilliseconds >= _mSec || _goodToGo)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            _timer = _timer.Subtract(new TimeSpan(0, 0, _mSec/60000, _mSec/1000, _mSec%1000));
            if(_timer.TotalMilliseconds < 0)
            {
                _timer = TimeSpan.Zero;
            }
            _goodToGo = false;
        }

        public void Reset(int a_newTimer)
        {
            _timer = TimeSpan.Zero;
            MSec = a_newTimer;
            _goodToGo = false;
        }

        public void ResetToZero()
        {
            _timer = TimeSpan.Zero;
            _goodToGo = false;
        }

        public virtual XElement ReturnXML()
        {
            XElement xml= new XElement("Timer",
                                    new XElement("_mSec", _mSec),
                                    new XElement("_timer", Timer));



            return xml;
        }

        public void SetTimer(TimeSpan a_time)
        {
            _timer = a_time;
        }

        public virtual void SetTimer(int a_ms)
        {
            _timer = TimeSpan.FromMilliseconds((long)(a_ms));
        }
    }
}
