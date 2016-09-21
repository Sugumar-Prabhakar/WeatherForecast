using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherForecast;
using WeatherForecast.Helpers.OpenWeather;

namespace WeatherForecast.Viewmodel
{
    public class OpenWeatherViewModel :  ObservableClass
    {
        #region Private Fields
        
        private string _selectedDate;
        private OpenWeatherDetails _weatherInfo;
        private string _clouds_All;
        private string _winds_Degree;
        private string _winds_Speed;
        private string _temp_Temperature;
        private string _temp_MinTemperature;
        private string _temp_MaxTemperature;
        private string _temp_Pressure;
        private string _temp_SeaLevel;
        private string _temp_GndLevel;
        private string _temp_Humidity;
        private string _temp_TempKF;

        #endregion

        #region Constructor
        public OpenWeatherViewModel(OpenWeatherDetails weatherinfo, string date)
        {
            _selectedDate = date;
            _weatherInfo = weatherinfo;
        }
        #endregion

        #region Properties

        public string Clouds_All
        {
            get
            {
                return _clouds_All;
            }

            set
            {
                _clouds_All = value;
                RaisePropertyChanged("Clouds_All");
            }
        }
        public string Winds_Degree
        {
            get
            {
                return _winds_Degree;
            }

            set
            {
                _winds_Degree = value;
                RaisePropertyChanged("Winds_Degree");
            }
        }
        public string Winds_Speed
        {
            get
            {
                return _winds_Speed;
            }

            set
            {
                _winds_Speed = value;
                RaisePropertyChanged("Winds_Speed");
            }
        }
        public string Temp_Temperature
        {
            get
            {
                return _temp_Temperature;
            }

            set
            {
                _temp_Temperature = value;
                RaisePropertyChanged("Temp_Temperature");
            }
        }
        public string Temp_MaxTemperature
        {
            get
            {
                return _temp_MaxTemperature;
            }

            set
            {
                _temp_MaxTemperature = value;
                RaisePropertyChanged("Temp_MaxTemperature");
            }
        }
        public string Temp_Pressure
        {
            get
            {
                return _temp_Pressure;
            }

            set
            {
                _temp_Pressure = value;
                RaisePropertyChanged("Temp_Pressure");
            }
        }
        public string Temp_SeaLevel
        {
            get
            {
                return _temp_SeaLevel;
            }

            set
            {
                _temp_SeaLevel = value;
                RaisePropertyChanged("Temp_SeaLevel");
            }
        }
        public string Temp_GndLevel
        {
            get
            {
                return _temp_GndLevel;
            }

            set
            {
                _temp_GndLevel = value;
                RaisePropertyChanged("Temp_GndLevel");
            }
        }
        public string Temp_Humidity
        {
            get
            {
                return _temp_Humidity;
            }

            set
            {
                _temp_Humidity = value;
                RaisePropertyChanged("Temp_Humidity");
            }
        }
        public string Temp_TempKF
        {
            get
            {
                return _temp_TempKF;
            }

            set
            {
                _temp_TempKF = value;
                RaisePropertyChanged("Temp_TempKF");
            }
        }
        public string Temp_MinTemperature
        {
            get
            {
                return _temp_MinTemperature;
            }

            set
            {
                _temp_MinTemperature = value;
                RaisePropertyChanged("Temp_MinTemperature");
            }
        }

        public ICommand CloseCommand
        {
            get { return new RelayCommand(OnCloseCommand); }
        }
        public ICommand ShowWeatherDetailsCommand
        {
            get { return new RelayCommand(OnShowWeatherDetailsCommand); }
        }

        public ObservableCollection<string> HourlyDate { get; set; }

        #endregion

        #region UI Commands Impl

        private void OnCloseCommand(object o)
        {
            EventPublisher.Instance.FireBackButtonPressedEvent();
        }
        private void OnShowWeatherDetailsCommand(object o)
        {
            if (o != null)
            {
                string input = (string)o;
                ShowWeatherInfo(input);
            }
        }

        #endregion

        #region Public Methods

        public override void InitializeViewModel()
        {
            if(_weatherInfo != null)
            {
                HourlyDate = new ObservableCollection<string>();
                _weatherInfo.list.ForEach(x =>
                {
                    string date = DateTime.Parse(x.dt_txt).ToShortDateString();
                    if (!HourlyDate.Contains(x.dt_txt) && date.Equals(_selectedDate))
                        HourlyDate.Add(x.dt_txt);
                });
                RaisePropertyChanged("HourlyDate");
                ShowWeatherInfo(HourlyDate[0]);
            }
        }

        public void ShowWeatherInfo(string date)
        {
            if (_weatherInfo != null)
            {
                var weatherInfo = _weatherInfo.list.First(x => x.dt_txt.Equals(date));
                if (weatherInfo != null)
                {
                    Clouds_All = weatherInfo.clouds.all.ToString();
                    Winds_Degree = weatherInfo.wind.deg.ToString();
                    Winds_Speed = weatherInfo.wind.speed.ToString();
                    Temp_Temperature = weatherInfo.main.temp.ToString();
                    Temp_MaxTemperature = weatherInfo.main.temp_max.ToString();
                    Temp_Pressure = weatherInfo.main.pressure.ToString();
                    Temp_SeaLevel = weatherInfo.main.sea_level.ToString();
                    Temp_GndLevel = weatherInfo.main.grnd_level.ToString();
                    Temp_Humidity = weatherInfo.main.humidity.ToString();
                    Temp_TempKF = weatherInfo.main.temp_kf.ToString();
                    Temp_MinTemperature = weatherInfo.main.temp_min.ToString();
                }
            }
        }

        #endregion
    }
}
