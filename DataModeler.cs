using System.Collections.Generic;
using System.Xml;
using System;
namespace Project1_Group_17
{
    public class DataModeler
    {
        public delegate void ParseHandler(string fileName);
        private Dictionary<string, CityInfo> cities;

        /// <summary>
        /// Parse a XML file and populate a dictionary
        /// </summary>
        /// <param name="fileName">XML file to be opened</param>
        public void ParseXML(string fileName)
        {
            //TODO: Read file and populate the dictionary
            XmlDocument document = new XmlDocument();
            document.Load(fileName);

            foreach (XmlElement canadaCity in document.DocumentElement)
            {
                //Read in all of the elements of a city
                string city = canadaCity.GetElementsByTagName("city").Item(0).InnerText,
                    cityAscii = canadaCity.GetElementsByTagName("city_ascii").Item(0).InnerText,
                    country = canadaCity.GetElementsByTagName("country").Item(0).InnerText,
                    adminName = canadaCity.GetElementsByTagName("admin_name").Item(0).InnerText,
                    captial = canadaCity.GetElementsByTagName("capital").Item(0).InnerText;

                double lat = Convert.ToDouble(canadaCity.GetElementsByTagName("lat").Item(0).InnerText),
                    lng = Convert.ToDouble(canadaCity.GetElementsByTagName("lng").Item(0).InnerText);

                ulong pop = Convert.ToUInt64(canadaCity.GetElementsByTagName("population").Item(0).InnerText),
                    id = Convert.ToUInt64(canadaCity.GetElementsByTagName("id").Item(0).InnerText);

                cities.Add($"{city}|{adminName}", new CityInfo(id, city, cityAscii, pop, adminName, lat, lng));
                break;
            }
        }

        public void ParseJSON(string fileName)
        {

        }

        public void ParseCSV(string fileName)
        {

        }

        public Dictionary<string, CityInfo> ParseFile(string fileName, string fileType)
        {
            cities = new Dictionary<string, CityInfo>();

            //TODO

            return cities;
        }
    }
}
