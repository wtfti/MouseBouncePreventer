namespace RazerDeathadderFix
{
    using System;
    using System.Diagnostics;
    using Interfaces;
    public class MousePreventer : IMousePreventer
    {
        private const long DifferenceTicksBetweenEachClick = 1000000;
        private const long MaxAllowedMilisecondsForStopwatch = 30000;
        private const int ResetStopwatchTimerInterval = 28000;
        private readonly Stopwatch stopwatch;
        private readonly System.Windows.Forms.Timer resetStopwatchTimer;
        private event EventHandler Tick;
        private long lastEllapsedTime;

        public MousePreventer()
        {
            this.stopwatch = new Stopwatch();
            this.stopwatch.Start();
            this.resetStopwatchTimer = new System.Windows.Forms.Timer
            {
                Interval = ResetStopwatchTimerInterval,
                Enabled = true
            };
            this.Tick = this.OnTick;
            this.resetStopwatchTimer.Tick += (sender, args) => this.Tick(sender, args);
        }

        public event EventHandler OnMouseBounce;

        private void OnTick(object sender, EventArgs eventArgs)
        {
            if (this.stopwatch.ElapsedMilliseconds > MaxAllowedMilisecondsForStopwatch)
            {
                this.lastEllapsedTime = 0;
                this.stopwatch.Restart();
            }
        }

        public bool IsMouseBounce(IntPtr wParam)
        {
            MouseMessages mouseEvent = (MouseMessages)wParam;
            if (mouseEvent == MouseMessages.WM_LBUTTONDOWN)
            {
                long currentTime = this.stopwatch.Elapsed.Ticks;
                if (this.lastEllapsedTime > 0)
                {
                    long timeDifference = currentTime - this.lastEllapsedTime;
                    if (timeDifference < DifferenceTicksBetweenEachClick)
                    {
                        this.lastEllapsedTime = currentTime;
                        return true;
                    }
                }
                
                this.lastEllapsedTime = currentTime;
            }

            return false;
        }

        public void Deactivate()
        {
            this.stopwatch.Stop();
            this.resetStopwatchTimer.Dispose();
        }
    }
}
