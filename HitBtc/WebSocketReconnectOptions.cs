using System;

namespace Hitbtc
{
    /// <summary>Controls reconnection of long-running WebSocket notification listeners.</summary>
    public sealed class WebSocketReconnectOptions
    {
        public int MaxAttempts { get; set; } = 5;
        public TimeSpan InitialDelay { get; set; } = TimeSpan.FromSeconds(1);
        public TimeSpan MaximumDelay { get; set; } = TimeSpan.FromSeconds(30);

        internal TimeSpan GetDelay(int attempt)
        {
            if (MaxAttempts < 0) throw new InvalidOperationException("MaxAttempts cannot be negative.");
            if (InitialDelay < TimeSpan.Zero || MaximumDelay < TimeSpan.Zero)
                throw new InvalidOperationException("Reconnect delays cannot be negative.");
            var milliseconds = InitialDelay.TotalMilliseconds * Math.Pow(2, Math.Max(0, attempt - 1));
            return TimeSpan.FromMilliseconds(Math.Min(milliseconds, MaximumDelay.TotalMilliseconds));
        }
    }

    public sealed class HitBtcReconnectingEventArgs : EventArgs
    {
        internal HitBtcReconnectingEventArgs(int attempt, TimeSpan delay, bool authenticated,
            Exception exception)
        {
            Attempt = attempt;
            Delay = delay;
            Authenticated = authenticated;
            Exception = exception;
        }

        public int Attempt { get; }
        public TimeSpan Delay { get; }
        public bool Authenticated { get; }
        public Exception Exception { get; }
    }
}
