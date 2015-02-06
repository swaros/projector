using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector
{
    class TimeUtil
    {        
        private long timeMs = 0;

        public void setTimer()
        {
            //currentTime = new DateTime(;
            timeMs = DateTime.Now.Ticks;
        }

        public bool getDiff(long diffNeeded)
        {
            return (diffNeeded <= this.getDiff());
        }

        public long getDiff()
        {
            //currentTime = new DateTime();
            long nowTime = DateTime.Now.Ticks;

            return nowTime - timeMs;
        }

        
        
    }
}
