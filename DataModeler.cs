/// DataModeler.cs
/// Authors: Connor Black, James Dunton, Hunter Bennett
/// Desc: Loads a dictionary with cities parsed from varying file types

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

using System;
using System.Xml;
namespace Project1_Group_17
{
    public class DataModeler
    {
        private Dictionary<string, CityInfo> ParsedCities;
        public delegate void ParseHandler(string fileName);
        public enum SupportedFileTypes { JSON, XML, CSV };
      
        public DataModeler()
        {
            ParsedCities = new Dictionary<string, CityInfo>();
        }
        /// <summary>
        /// Parse a XML file and populate a dictionary
        /// </summary>
        /// <param name="fileName">XML file to be opened</param>
        public void ParseXML(string fileName)
        {

            XmlDocument document = new XmlDocument();
            document.Load($"../../../Data/{fileName}");

            foreach (XmlElement canadaCity in document.DocumentElement)
            {
                //Read in all of the elements of a city
                string city = canadaCity.GetElementsByTagName("city").Item(0).InnerText,
                    cityAscii = canadaCity.GetElementsByTagName("city_ascii").Item(0).InnerText,
                    adminName = canadaCity.GetElementsByTagName("admin_name").Item(0).InnerText,
                    capital = canadaCity.GetElementsByTagName("capital").Item(0).InnerText;

                double lat = Convert.ToDouble(canadaCity.GetElementsByTagName("lat").Item(0).InnerText),
                    lng = Convert.ToDouble(canadaCity.GetElementsByTagName("lng").Item(0).InnerText);

                ulong pop = Convert.ToUInt64(canadaCity.GetElementsByTagName("population").Item(0).InnerText),
                    id = Convert.ToUInt64(canadaCity.GetElementsByTagName("id").Item(0).InnerText);


                //add the city
                ParsedCities.Add($"{city}|{adminName}", new CityInfo(id, city, cityAscii, pop, adminName, lat, lng, capital));

            }
        }

        public void ParseJSON(string fileName)
        {
            string rawJson = File.ReadAllText($"../../../Data/{fileName}");
            JObject json = JObject.Parse($"{{ data:{rawJson}}}"); // Wrap JSON in braces for valid Parse syntax
            IList<JToken> results = json["data"].Children().ToList();

            foreach (JToken result in results)
            {
                string cityName = GetJTokenPropertyValue(result, "city", "");
                if (cityName == "")
                    continue;

                ulong id = ulong.Parse(GetJTokenPropertyValue(result, "id", "0"));
                string cityAscii = GetJTokenPropertyValue(result, "city_ascii", "");
                ulong population = ulong.Parse(GetJTokenPropertyValue(result, "population", "0"));
                string province = GetJTokenPropertyValue(result, "admin_name", "");
                double latitude = double.Parse(GetJTokenPropertyValue(result, "lat", "0"));
                double longitude = double.Parse(GetJTokenPropertyValue(result, "lng", "0"));
                string capitalStatus = GetJTokenPropertyValue(result, "capital", "");
                CityInfo city = new CityInfo(
                    id,
                    cityName,
                    cityAscii,
                    population,
                    province,
                    latitude,
                    longitude,
                    capitalStatus
                );

                ParsedCities.Add($"{cityName}|{province}", city);
            }
        }

        /// <summary>
        /// Retrieves a property from a JToken, or returns the given default value if not available
        /// </summary>
        /// <param name="token"></param>
        /// <param name="property"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private string GetJTokenPropertyValue(JToken token, string property, string defaultValue)
        {
            return token[property] == null ? defaultValue : token[property].ToString();
        }

        public void ParseCSV(string fileName)
        {

            ParsedCities = new Dictionary<string, CityInfo>();
            List<string> cityInfo = new List<string>(File.ReadAllLines($"../../../Data/{fileName}"));

            for (int i = 1; i < cityInfo.Count; i++)
            {
                string[] city = cityInfo[i].Split(',');

                ulong Id = Convert.ToUInt64(city[8]);
                string cityName = city[0];
                string ascii = city[1];
                ulong pop = Convert.ToUInt64(city[7]);
                string prov = city[5];
                double lat = Convert.ToDouble(city[2]);
                double lon = Convert.ToDouble(city[3]);
                string capital = city[6];

                CityInfo info = new CityInfo(Id, cityName, ascii, pop, prov, lat, lon, capital);
                ParsedCities.Add(($"{cityName}|{prov}"), info);
            }
        }

        public Dictionary<string, CityInfo> ParseFile(string fileName, SupportedFileTypes fileType)
        {
            ParseHandler parseMethod = null;
            switch(fileType)
            {
                case SupportedFileTypes.JSON:
                    parseMethod = ParseJSON;
                    break;
                case SupportedFileTypes.XML:
                    parseMethod = ParseXML;
                    break;
                case SupportedFileTypes.CSV:
                    parseMethod = ParseCSV;
                    break;
            }

            parseMethod(fileName);
            return ParsedCities;
        }
    }
}
