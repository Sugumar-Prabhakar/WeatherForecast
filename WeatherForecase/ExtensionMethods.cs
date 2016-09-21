using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WeatherForecast
{
    public static class ExtensionMethods
    {
        public static void WaitWithPumping(this Task task)
        {
            if (task == null) throw new ArgumentNullException("task is null");
            var nestedFrame = new DispatcherFrame();
            task.ContinueWith(_ => nestedFrame.Continue = false);
            Dispatcher.PushFrame(nestedFrame);
            task.Wait();
        }
    }
}
