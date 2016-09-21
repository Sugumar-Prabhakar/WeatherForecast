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
    public class OpenWeatherViewModelUnitTest
    {
        [TestMethod()]
        public void OpenWeatherViewModelTest()
        {
            OpenWeatherViewModel vm = new OpenWeatherViewModel(null, DateTime.Today.ToShortDateString());
            vm.InitializeViewModel();
            Assert.IsNull(vm.HourlyDate);

        }

        [TestMethod()]
        public void InitializeViewModelTest()
        {
            OpenWeatherViewModel vm = new OpenWeatherViewModel(null, DateTime.Today.ToShortDateString());
            vm.InitializeViewModel();
            Assert.IsNull(vm.HourlyDate);
        }

        [TestMethod()]
        public void ShowWeatherInfoTest()
        {
            OpenWeatherViewModel vm = new OpenWeatherViewModel(null, DateTime.Today.ToShortDateString());
            vm.InitializeViewModel();
            Assert.IsNull(vm.HourlyDate);
            vm.ShowWeatherInfo(DateTime.Today.ToShortDateString());
            Assert.IsNull(vm.Clouds_All);
        }
    }
}