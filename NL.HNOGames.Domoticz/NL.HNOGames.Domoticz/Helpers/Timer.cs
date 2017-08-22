using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Helpers
{
    public sealed class Timer : CancellationTokenSource
    {
        private bool IsDisposed { get; set; }

        public Timer(Action<object> callback, object state, int dueTime, int period)
        {
            Task.Delay(dueTime, Token).ContinueWith(async (t, s) => {
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
