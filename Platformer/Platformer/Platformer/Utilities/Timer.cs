using System;
using System.Collections.Generic;
using System.Text;

namespace Platformer.Utilities
{
    /* how to use*/
    /*
     * create an instance in constuctor
     * TalkTimer = new Timer(2f);
     * 
     * Put in update method:
     * TalkTimer.Update();
     * 
     * when you start the timer, when the "wait" begins
     * TalkTimer.Wait = true;
     */
   public  class Timer //timer 
    {
        public bool Wait { get; set; }
        public int ActionDuration { get; private set; }
        public int ActionTime { get; private set; }
        public Timer(float waitSeconds)
        {
            Wait = false;
            ActionDuration = (int)(60 * waitSeconds);//Action for 1.5 s
            ActionTime = 0;
        }
        public void Update()// counts down the time
        {
            if (Wait)
            {
                ActionTime++;
                if (ActionTime >= ActionDuration)
                {
                    ActionTime = 0;
                    Wait = false;
                }
            }
        }
    }
}
