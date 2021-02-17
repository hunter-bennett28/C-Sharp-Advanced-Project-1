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
                    captial = canadaCity.GetElementsByTagName("capital").Item(0).InnerText;

                double lat = Convert.ToDouble(canadaCity.GetElementsByTagName("lat").Item(0).InnerText),
                    lng = Convert.ToDouble(canadaCity.GetElementsByTagName("lng").Item(0).InnerText);

                ulong pop = Convert.ToUInt64(canadaCity.GetElementsByTagName("population").Item(0).InnerText),
                    id = Convert.ToUInt64(canadaCity.GetElementsByTagName("id").Item(0).InnerText);

                //add the city
                if (ParsedCities.ContainsKey(city))
                    ParsedCities.Add($"{city}|{adminName}", new CityInfo(id, city, cityAscii, pop, adminName, lat, lng));
                else
                    ParsedCities.Add(city, new CityInfo(id, city, cityAscii, pop, adminName, lat, lng));

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
                CityInfo city = new CityInfo(
                    ulong.Parse(GetJTokenPropertyValue(result, "id", "0")),
                    cityName,
                    GetJTokenPropertyValue(result, "city_ascii", ""),
                    ulong.Parse(GetJTokenPropertyValue(result, "population", "0")),
                    GetJTokenPropertyValue(result, "admin_name", ""),
                    double.Parse(GetJTokenPropertyValue(result, "lat", "0")),
                    double.Parse(GetJTokenPropertyValue(result, "lng", "0"))
                );

                // If a city with the given name already exists in the dictionary, append the province name
                if(ParsedCities.ContainsKey(cityName))
                {
                    cityName += $"|{result["admin_name"].ToString()}";
                }
                ParsedCities.Add(cityName, city);
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
