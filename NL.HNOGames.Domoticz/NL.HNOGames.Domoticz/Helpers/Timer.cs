using System;
using System.Threading;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Helpers
{
    /// <summary>
    /// Defines the <see cref="Timer" />
    /// </summary>
    public sealed class Timer : CancellationTokenSource
    {
        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class.
        /// </summary>
        /// <param name="callback">The callback<see cref="Action{object}"/></param>
        /// <param name="state">The state<see cref="object"/></param>
        /// <param name="dueTime">The dueTime<see cref="int"/></param>
        /// <param name="period">The period<see cref="int"/></param>
        public Timer(Action<object> callback, object state, int dueTime, int period)
        {
            Task.Delay(dueTime, Token).ContinueWith(async (t, s) =>
            {
                var tuple = (Tuple<Action<object>, object>)s;
                while (!IsCancellationRequested)
                {
                    await Task.Run(() =>
                     {
                         tuple.Item1?.Invoke(tuple.Item2);
                     });
                    await Task.Delay(period);
                }

            }, Tuple.Create(callback, state), CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
                TaskScheduler.Default);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether IsDisposed
        /// </summary>
        private bool IsDisposed { get; set; }

        #endregion

        /// <summary>
        /// The Dispose
        /// </summary>
        /// <param name="disposing">The disposing<see cref="bool"/></param>
        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            if (disposing)
            {
                Cancel();
            }

            base.Dispose(disposing);
        }
    }
}
