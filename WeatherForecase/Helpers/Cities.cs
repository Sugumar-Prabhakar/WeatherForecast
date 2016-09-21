using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WeatherForecast.Helpers
{
    [XmlRoot(ElementName = "Coord")]
    public class Coord
    {
        [XmlElement(ElementName = "lat")]
        public string Lat { get; set; }
        [XmlElement(ElementName = "lon")]
        public string Lon { get; set; }
    }

    [XmlRoot(ElementName = "City")]
    public class City : IDataErrorInfo
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Country")]
        public string Country { get; set; }
        [XmlElement(ElementName = "Coord")]
        public Coord Coord { get; set; }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "Name")
                {
                    if (string.IsNullOrEmpty(Name))
                        result = "Please enter a Name of the city";
                }
                else if (columnName == "Country")
                {
                    if (string.IsNullOrEmpty(Country))
                        result = "Please enter a Name of the country";
                }
                else if (columnName == "Lat")
                {
                    if (string.IsNullOrEmpty(Coord.Lat))
                        result = "Please enter a valid lattitude of the city";
                }
                else if (columnName == "Lon")
                {
                    if (string.IsNullOrEmpty(Coord.Lon))
                        result = "Please enter a valid longitude of the city";
                }
                return result;
            }
        }
    }

    [XmlRoot(ElementName = "Cities")]
    public class Cities
    {
        [XmlElement(ElementName = "City")]
        public List<City> City { get; set; }
    }
}
