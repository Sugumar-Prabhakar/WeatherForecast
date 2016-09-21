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
    public class MainViewModelUnitTest
    {
        [TestMethod()]
        public void InitializeViewTest()
        {
            MainUIViewModel vm = new MainUIViewModel();
            vm.InitializeView();
            Assert.AreEqual(2, vm.WeatherSource.Count);
        }

        [TestMethod()]
        public void UnInitializeViewTest()
        {
           
        }

        [TestMethod()]
        public void DownloadCommandTest_OpenWeather()
        {
            MainUIViewModel vm = new MainUIViewModel();
            vm.InitializeView();
            Assert.AreEqual(2, vm.WeatherSource.Count);
            vm.DownloadCityWeatherDetailsCommand.Execute("Open Weather");
            Assert.AreSame(typeof(OpenWeatherViewModel), vm.CurrentpageViewModel.GetType());
        }

        [TestMethod()]
        public void DownloadCommandTest_Yahoo()
        {
            MainUIViewModel vm = new MainUIViewModel();
            vm.InitializeView();
            Assert.AreEqual(2, vm.WeatherSource.Count);
            vm.DownloadCityWeatherDetailsCommand.Execute("Yahoo");
            Assert.AreSame(typeof(YahooWeatherViewModel), vm.CurrentpageViewModel.GetType());
        }
    }
}