using System;
using System.Collections.Generic;
using System.Threading;

namespace Aura
{
    public class TimerManager : ITimedEntity
    {
        #region Singleton
        public static readonly TimerManager Instance = new TimerManager();
        private TimerManager() { }
        #endregion

        private List<Timer> timers = new List<Timer>();

        private bool pauseAll = false;

        public void RegisterTimer(Timer t)
        {
            timers.Add(t);
            t.Start();
        }
        public void RemoveTimer(Timer t)
        {
            timers.Remove(t);
        }

        public void Update()
        {
            foreach (Timer t in timers)
            {
                t.Update();
            }
        }

        public void Initialize() 
        {
            

        }

        public bool isRunning
        {
            get { return pauseAll; }
        }
    }
}
