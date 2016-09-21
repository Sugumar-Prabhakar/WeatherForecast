using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast
{
    public delegate void BackWindowHandler();
    public class EventPublisher
    {
        #region SingleTon
        private static EventPublisher _instance;

        public static EventPublisher Instance
        {
            get { return _instance ?? (_instance = new EventPublisher()); }
        }

        private EventPublisher() { }
        #endregion

        #region Back Button Event

        public event BackWindowHandler BackButtonPressed;

        public void FireBackButtonPressedEvent()
        {
            if(BackButtonPressed != null)
            {
                foreach(Delegate d in BackButtonPressed.GetInvocationList())
                {
                    try
                    {
                        d.DynamicInvoke(null);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        #endregion
    }
}
