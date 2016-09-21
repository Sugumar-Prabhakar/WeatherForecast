using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherForecast.Helpers.Yahoo;
using WeatherForecast;

namespace WeatherForecast.Viewmodel
{
    public class YahooWeatherViewModel :  ObservableClass
    {
        #region Constructor

        public YahooWeatherViewModel(YahooWeatherDetails weatherInfo)
        {
            _weatherInfo = weatherInfo;
        }

        #endregion

        #region Private Fields

        private YahooWeatherDetails _weatherInfo;
        private string _loc_City;
        private string _loc_Country;
        private string _loc_Region;
        private string _wind_Chill;
        private string _wind_Direction;
        private string _wind_Speed;
        private string _atom_Humidity;
        private string _atom_Pressure;
        private string _atom_Rising;
        private string _atom_Visiblity;
        private string _ast_Sunrise;
        private string _ast_Sunset;
        private string _units_Distance;
        private string _units_Pressure;
        private string _units_Speed;
        private string _units_Temp;
        private string _fct_Code;
        private string _fct_Date;
        private string _fct_Day;
        private string _fct_High;
        private string _fct_Low;
        private string _con_Code;
        private string _con_Date;
        private string _con_Temp;

        #endregion

        #region Properties

        public string Loc_City
        {
            get { return _loc_City; }
            set { _loc_City = value; RaisePropertyChanged("Loc_City"); }
        }
        public string Loc_Country
        {
            get { return _loc_Country; }
            set { _loc_Country = value; RaisePropertyChanged("Loc_Country"); }
        }
        public string Loc_Region
        {
            get { return _loc_Region; }
            set { _loc_Region = value; RaisePropertyChanged("Loc_Region"); }
        }
        public string Wind_Chill
        {
            get { return _wind_Chill; }
            set { _wind_Chill = value; RaisePropertyChanged("Wind_Chill"); }
        }
        public string Wind_Direction
        {
            get { return _wind_Direction; }
            set { _wind_Direction = value; RaisePropertyChanged("Wind_Direction"); }
        }
        public string Wind_Speed
        {
            get { return _wind_Speed; }
            set { _wind_Speed = value; RaisePropertyChanged("Wind_Speed"); }
        }
        public string Atom_Humidity
        {
            get { return _atom_Humidity; }
            set { _atom_Humidity = value; RaisePropertyChanged("Atom_Humidity"); }
        }
        public string Atom_Pressure
        {
            get { return _atom_Pressure; }
            set { _atom_Pressure = value; RaisePropertyChanged("Atom_Pressure"); }
        }
        public string Atom_Rising
        {
            get { return _atom_Rising; }
            set { _atom_Rising = value; RaisePropertyChanged("Atom_Rising"); }
        }
        public string Atom_Visiblity
        {
            get { return _atom_Visiblity; }
            set { _atom_Visiblity = value; RaisePropertyChanged("Atom_Visiblity"); }
        }
        public string Ast_Sunrise
        {
            get { return _ast_Sunrise; }
            set { _ast_Sunrise = value; RaisePropertyChanged("Ast_Sunrise"); }
        }
        public string Ast_Sunset
        {
            get { return _ast_Sunset; }
            set { _ast_Sunset = value; RaisePropertyChanged("Ast_Sunset"); }
        }
        public string Units_Distance
        {
            get { return _units_Distance; }
            set { _units_Distance = value; RaisePropertyChanged("Units_Distance"); }
        }
        public string Units_Pressure
        {
            get { return _units_Pressure; }
            set { _units_Pressure = value; RaisePropertyChanged("Units_Pressure"); }
        }
        public string Units_Speed
        {
            get { return _units_Speed; }
            set { _units_Speed = value; RaisePropertyChanged("Units_Speed"); }
        }
        public string Units_Temp
        {
            get { return _units_Temp; }
            set { _units_Temp = value; RaisePropertyChanged("Units_Temp"); }
        }
        public string Fct_Code
        {
            get { return _fct_Code; }
            set { _fct_Code = value; RaisePropertyChanged("Fct_Code"); }
        }
        public string Fct_Date
        {
            get { return _fct_Date; }
            set { _fct_Date = value; RaisePropertyChanged("Fct_Date"); }
        }
        public string Fct_Day
        {
            get { return _fct_Day; }
            set { _fct_Day = value; RaisePropertyChanged("Fct_Day"); }
        }
        public string Fct_High
        {
            get { return _fct_High; }
            set { _fct_High = value; RaisePropertyChanged("Fct_High"); }
        }
        public string Fct_Low
        {
            get { return _fct_Low; }
            set { _fct_Low = value; RaisePropertyChanged("Fct_Low"); }
        }
        public string Con_Code
        {
            get { return _con_Code; }
            set { _con_Code = value; RaisePropertyChanged("Con_Code"); }
        }
        public string Con_Date
        {
            get { return _con_Date; }
            set { _con_Date = value; RaisePropertyChanged("Con_Date"); }
        }
        public string Con_Temp
        {
            get { return _con_Temp; }
            set { _con_Temp = value; RaisePropertyChanged("Con_Temp"); }
        }

        public ICommand GoBackCommand
        {
            get { return new RelayCommand(OnGoBackCommand); }
        }

        #endregion

        #region UI Command Impl

        private void OnGoBackCommand(object o)
        {
            EventPublisher.Instance.FireBackButtonPressedEvent();
        }

        #endregion

        #region Public Methods

        public override void InitializeViewModel()
        {
            if(_weatherInfo != null)
            {
                var channel = _weatherInfo.query.results.channel;

                Loc_City = _weatherInfo.query.results.channel.location.city;
                Loc_Country = _weatherInfo.query.results.channel.location.country;
                Loc_Region = _weatherInfo.query.results.channel.location.region;

                Wind_Chill = channel.wind.chill;
                Wind_Direction = channel.wind.direction;
                Wind_Speed = channel.wind.speed;

                Atom_Humidity = channel.atmosphere.humidity;
                Atom_Pressure = channel.atmosphere.pressure;
                Atom_Rising = channel.atmosphere.rising;
                Atom_Visiblity = channel.atmosphere.visibility;

                Ast_Sunrise = channel.astronomy.sunrise;
                Ast_Sunset = channel.astronomy.sunset;

                Units_Distance = channel.units.distance;
                Units_Pressure = channel.units.pressure;
                Units_Speed = channel.units.speed;
                Units_Temp = channel.units.temperature;

                var forecast = channel.item.forecast.FirstOrDefault(x => DateTime.Parse(x.date) == DateTime.Today);
                if(forecast != null)
                {
                    Fct_Code = forecast.code;
                    Fct_Date = forecast.date;
                    Fct_Day = forecast.day;
                    Fct_High = forecast.high;
                    Fct_Low = forecast.low;
                }

                Con_Code = channel.item.condition.code;
                Con_Date = channel.item.condition.date;
                Con_Temp = channel.item.condition.temp;
            }
        }

        #endregion
    }
}
