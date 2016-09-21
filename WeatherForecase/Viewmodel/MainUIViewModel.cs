using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using WeatherForecast.Helpers.OpenWeather;
using WeatherForecast;
using WeatherForecast.Helpers;
using WeatherForecast.Viewmodel;
using System.Configuration;
using WeatherForecast.Helpers.Yahoo;

namespace WeatherForecast.Viewmodel
{
    public class MainUIViewModel : ObservableClass
    {
        #region Private Fields

        private string appId;
        private dynamic _weatherDetails;
        private Helpers.City _selectedCity;
        private string _cityName;
        private string _countryName;
        private string _lattitude;
        private string _longitude;
        private string _selectedWeatherSource;
        private bool _showWeatherDetailsWizard;
        private ObservableClass _currentpageViewModel;
        private bool _isOffline;
        private string _status;

        #endregion

        #region Properties

        public Helpers.City SelectedCity
        {
            get
            {
                return _selectedCity;
            }

            set
            {
                _selectedCity = value;
                GetWeatherDetails();
                RaisePropertyChanged("SelectedCity");
            }
        }
        public string SelectedWeatherSource
        {
            get
            {
                return _selectedWeatherSource;
            }

            set
            {
                _selectedWeatherSource = value;
                if (SelectedCity != null) GetWeatherDetails();
                RaisePropertyChanged("SelectedWeatherSource");
            }
        }
        public string CityName
        {
            get
            {
                return _cityName;
            }

            set
            {
                _cityName = value;
                if (string.IsNullOrEmpty(value))
                    throw new Exception("City Name is Mandatory");
                RaisePropertyChanged("CityName");
            }
        }
        public string CountryName
        {
            get
            {
                return _countryName;
            }

            set
            {
                _countryName = value;
                if (string.IsNullOrEmpty(value))
                    throw new Exception("Country Name is Mandatory");
                RaisePropertyChanged("CountryName");
            }
        }
        public string Lattitude
        {
            get
            {
                return _lattitude;
            }

            set
            {
                _lattitude = value;
                if (string.IsNullOrEmpty(value))
                    throw new Exception("Lattitude is Mandatory");
                RaisePropertyChanged("Lattitude");
            }
        }
        public string Longitude
        {
            get
            {
                return _longitude;
            }

            set
            {
                _longitude = value;
                if (string.IsNullOrEmpty(value))
                    throw new Exception("Longitude is Mandatory");
                RaisePropertyChanged("Lattitude");
            }
        }
        public bool ShowWeatherDetailsWizard
        {
            get
            {
                return _showWeatherDetailsWizard;
            }

            set
            {
                _showWeatherDetailsWizard = value;
                RaisePropertyChanged("ShowWeatherDetailsWizard");
            }
        }
        public ObservableClass CurrentpageViewModel
        {
            get { return _currentpageViewModel; }
            set { _currentpageViewModel = value; RaisePropertyChanged("CurrentpageViewModel"); }
        }
        public bool IsOffline
        {
            get { return _isOffline; }
            set { _isOffline = value; RaisePropertyChanged("IsOffline"); }
        }
        public string Status
        {
            get { return _status; }
            set { _status = value; RaisePropertyChanged("Status"); }
        }

        public ICommand AddCommand
        {
            get { return new RelayCommand(OnAddCommand); }
        }
        public ICommand ShowWeatherCommand
        {
            get { return new RelayCommand(OnShowWeatherCommand); }
        }
        public ICommand DownloadCityWeatherDetailsCommand
        {
            get { return new RelayCommand(OnDownloadCityWeatherDetailsCommand); }
        }

        public ObservableCollection<string> WeatherSource { get; set; }
        public ObservableCollection<Helpers.City> Cities { get; set; }
        public ObservableCollection<string> Dates { get; set; }

        #endregion

        #region Private Methods

        private void LoadWeatherSources()
        {
            WeatherSource = new ObservableCollection<string>() { "Open Weather", "Yahoo" };
            SelectedWeatherSource = "Open Weather";
            RaisePropertyChanged("WeatherSource");
        }
        private void LoadCities()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Cities));
                StreamReader writer = new StreamReader(AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug\\", "Resources\\Cities.xml"));
                var val = (Cities)serializer.Deserialize(writer);
                Cities = new ObservableCollection<Helpers.City>(val.City);
                RaisePropertyChanged("Cities");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GetWeatherDetails()
        {
            Status = "Please wait. Fetching details from server.";
            var task = Task.Factory.StartNew(() => { 
            try
            {
                switch (SelectedWeatherSource)
                {
                    case "Open Weather":
                        if (IsOffline)
                            GetOpenWeaterDetailsOffline(SelectedCity.Name);
                        else
                            GetOpenWeaterDetails();
                        break;
                    case "Yahoo":
                        if (IsOffline)
                            GetYahooWeatherDetailsOffline(SelectedCity.Name);
                        else
                            GetYahooWeatherDetails();
                        break;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            });
            task.ContinueWith(t =>
            {
                if(t.Exception != null)
                {
                    MessageBox.Show(t.Exception.Message);
                }
                Status = "";
            });
            task.WaitWithPumping();
        }
        private void GetOpenWeaterDetails()
        {
            appId = ConfigurationManager.AppSettings["OpenWeather"];
            OpenWeather _weather = new OpenWeather(appId, SelectedCity.Name);
            string url = _weather.BuildUrl();
            OpenWeatherDetails weatherDetails = _weather.GetOpenWeatherDetails(url);
            _weatherDetails = weatherDetails;
            LoadOpenWeatherDates(weatherDetails);
        }
        private void GetOpenWeaterDetailsOffline(string city)
        {
            appId = ConfigurationManager.AppSettings["OpenWeather"];
            OpenWeather _weather = new OpenWeather(appId, SelectedCity.Name);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug\\", "Offline Datas\\"), string.Format("{0}\\{1}.txt", SelectedWeatherSource, city));
            if (!File.Exists(path))
                throw new Exception(string.Format("Please download the data for {0} to work in offline mode.", city));

            OpenWeatherDetails weatherDetails = _weather.GetOpenWeatherDetailsOffline(File.ReadAllText(path));
            _weatherDetails = weatherDetails;
            LoadOpenWeatherDates(weatherDetails);
        }

        private void GetYahooWeatherDetails()
        {
            YahooWeather _weather = new YahooWeather(SelectedCity.Name);
            string url = _weather.BuildUrl();
            YahooWeatherDetails weatherDetails = _weather.GetYahooWeatherDetails(url);
            _weatherDetails = weatherDetails;
            LoadYahooWeatherDates(weatherDetails);
        }
        private void GetYahooWeatherDetailsOffline(string city)
        {
            YahooWeather _weather = new YahooWeather(SelectedCity.Name);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug\\", "Offline Datas\\"), string.Format("{0}\\{1}.txt", SelectedWeatherSource, city));
            if (!File.Exists(path))
                throw new Exception(string.Format("Please download the data for {0} to work in offline mode.", city));

            YahooWeatherDetails weatherDetails = _weather.GetYahooWeatherDetailsOffline(File.ReadAllText(path));
            _weatherDetails = weatherDetails;
            LoadYahooWeatherDates(weatherDetails);
        }

        private void DownloadWeatherDetailsOffline(string city)
        {
            WeatherDetails _weather = null;
            string url = GetUrl(ref _weather);
            string weatherDetails = _weather.DownloadWeatherDetails(url);

            string dirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug\\", "Offline Datas\\"), SelectedWeatherSource);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug\\", "Offline Datas\\"), string.Format("{0}\\{1}.txt", SelectedWeatherSource, city));
            if (File.Exists(path))
                File.Delete(path);
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(weatherDetails);
                writer.Close();
            }
            MessageBox.Show("Downloaded Successfully !!!");
        }
        private string GetUrl(ref WeatherDetails _weather)
        {
            string Url = null;
            switch (SelectedWeatherSource)
            {
                case "Open Weather":
                    appId = ConfigurationManager.AppSettings["OpenWeather"];
                    _weather = new OpenWeather(appId, SelectedCity.Name);
                    Url = _weather.BuildUrl();
                    break;
                case "Yahoo":
                    _weather = new YahooWeather(SelectedCity.Name);
                    Url = _weather.BuildUrl();
                    break;
            }
            return Url;
        }

        private void LoadOpenWeatherDates(OpenWeatherDetails wd)
        {
            Dates = new ObservableCollection<string>();
            wd.list.ForEach(x =>
            {
                string date = DateTime.Parse(x.dt_txt).ToShortDateString();
                if (!Dates.Contains(date))
                    Dates.Add(date);
            });
            RaisePropertyChanged("Dates");
        }
        private void LoadYahooWeatherDates(YahooWeatherDetails wd)
        {
            Dates = new ObservableCollection<string>();
            wd.query.results.channel.item.forecast.ForEach(x =>
            {
                if (DateTime.Parse(x.date) <= DateTime.Today.AddDays(4))
                    Dates.Add(x.date);
            });
            RaisePropertyChanged("Dates");
        }
        
        #endregion

        #region UI Comamnd Implementation

        private void OnAddCommand(object o)
        {
            if (string.IsNullOrEmpty(CityName) || string.IsNullOrEmpty(CountryName) || string.IsNullOrEmpty(Lattitude) || string.IsNullOrEmpty(Longitude))
                throw new Exception("Unable to add as some of the mandatory fields are missed");
            Helpers.City _city = new Helpers.City
            {
                Name = CityName,
                Country = CountryName,
                Coord = new Helpers.Coord
                {
                    Lat = Lattitude,
                    Lon = Longitude
                }
            };

            XDocument xdoc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug\\", "Resources\\Cities.xml"));
            XElement cityElement = new XElement("City");
            XElement nameElement = new XElement("Name", _city.Name);
            XElement countryElement = new XElement("Country", _city.Country);
            XElement coordElemt = new XElement("Coord");
            XElement latElement = new XElement("lat", _city.Coord.Lat);
            XElement lonElement = new XElement("lon", _city.Coord.Lon);

            coordElemt.Add(lonElement);
            coordElemt.Add(latElement);

            cityElement.Add(nameElement);
            cityElement.Add(countryElement);
            cityElement.Add(coordElemt);

            xdoc.Root.Add(cityElement);
            
            xdoc.Save(AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug\\", "Resources\\Cities.xml"));
            LoadCities();
            MessageBox.Show("City added successfully");
        }
        private void OnShowWeatherCommand(object o)
        {
            if(o != null)
            {
                string input = (string)o;
                switch (SelectedWeatherSource)
                {
                    case "Open Weather":
                        CurrentpageViewModel = new OpenWeatherViewModel(_weatherDetails as OpenWeatherDetails, input);
                        CurrentpageViewModel.InitializeViewModel();
                        ShowWeatherDetailsWizard = true;
                        break;
                    case "Yahoo":
                        CurrentpageViewModel = new YahooWeatherViewModel(_weatherDetails as YahooWeatherDetails);
                        CurrentpageViewModel.InitializeViewModel();
                        ShowWeatherDetailsWizard = true;
                        break;

                }
            }
        }
        private void OnDownloadCityWeatherDetailsCommand(object o)
        {
            if (SelectedCity != null)
                DownloadWeatherDetailsOffline(SelectedCity.Name);
        }
        #endregion

        #region Public Methods

        public void InitializeView()
        {
            LoadWeatherSources();
            LoadCities();

            EventPublisher.Instance.BackButtonPressed += Instance_BackButtonPressed;
        }

        public void UnInitializeView()
        {
            EventPublisher.Instance.BackButtonPressed -= Instance_BackButtonPressed;
        }

        #endregion

        #region Event Callbacks

        void Instance_BackButtonPressed()
        {
            ShowWeatherDetailsWizard = false;
        }

        #endregion
    }
}
