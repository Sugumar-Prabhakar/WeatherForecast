using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeatherForecast.Viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Viewmodel.Tests
{
    [TestClass()]
    public class YahooWeatherViewModelUnitTest
    {
        [TestMethod()]
        public void YahooWeatherViewModelTest()
        {
            YahooWeatherViewModel vm = new YahooWeatherViewModel(null);
            vm.InitializeViewModel();
            Assert.IsNull(vm.Loc_City);

        }

        [TestMethod()]
        public void InitializeViewModelTest()
        {
            YahooWeatherViewModel vm = new YahooWeatherViewModel(null);
            vm.InitializeViewModel();
            Assert.IsNull(vm.Loc_City);
        }
    }
}