using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Aura.Core
{
    /// <summary>
    /// Class provides basic timing functionality
    /// </summary>
    public class Timer : IDisposable, ITimingDevice, IHasID
    {
        #region Fields
        protected Stopwatch stopWatch;
        protected bool hasActivated;
        protected int ID;

        /// <summary>
        /// Period, in seconds
        /// </summary>
        public double Period { get; set; }
        public bool Repeat { get; set; }
        public event EventHandler TimerEvent;
        #endregion

        #region Constructors
        public Timer()
        {
            Period = float.NaN;
            stopWatch = new Stopwatch();
            ID = IDManager.NewID;
            TimerManager.Instance.RegisterTimer(this);
        }

        public Timer(double period, bool repeat = false)
        {
            Period = period;
            Repeat = repeat;
            stopWatch = new Stopwatch();
            ID = IDManager.NewID;
            TimerManager.Instance.RegisterTimer(this);
        }

        public virtual void Dispose()
        {
            TimerEvent = null;
            stopWatch = null;
            TimerManager.Instance.RemoveTimer(this);
        }
        #endregion

        #region Methods
        public void Start()
        {
            stopWatch.Start();
        }

        public void Stop()
        {
            stopWatch.Stop();
        }

        public void Reset()
        {
            stopWatch.Reset();
            hasActivated = false;
        }

        public void Update()
        {
            //Make sure the timer hasn't already been activated
            if (stopWatch.ElapsedMilliseconds * 1000 > Period && (!hasActivated || Repeat))
            {
                TimerEvent(this, new EventArgs());
                stopWatch.Restart();
                hasActivated = true;
            }
        }

        public bool isRunning
        {
            get { return stopWatch.IsRunning; }
        }

        public int GetID
        {
            get { return ID; }
        }

        public override bool Equals(object obj)
        {
            var t = obj as Timer;
            if (t == null) return false;

            return t.ID == this.ID;
        }
        #endregion
    }

    /// <summary>
    /// Interface defining a timing event object. Provides a universal
    /// interface for various timer implementations.
    /// </summary>
    public interface ITimingDevice : ITimedEntity
    {
        double Period { get; set; }
        bool Repeat { get; set; }
        event EventHandler TimerEvent;

        void Start();
        void Stop();
        void Reset();
        void Update();
    }

    public interface ITimedEntity
    {
        bool isRunning { get; }
    }
}
