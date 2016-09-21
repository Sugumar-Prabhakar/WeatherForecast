using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Helpers
{
    public abstract class WeatherDetails
    {
        public abstract string BuildUrl();
        public abstract string DownloadWeatherDetails(string url);
    }
}
