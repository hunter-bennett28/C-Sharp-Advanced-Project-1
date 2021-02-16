using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Project1_Group_17
{
    public class DataModeler
    {
        private Dictionary<string, CityInfo> ParsedCities;
        public delegate void ParseHandler(string fileName);
        public enum SupportedFileTypes { JSON, XML, CSV };

        public void ParseXML(string fileName)
        {
             
        }

        public void ParseJSON(string fileName)
        {

            string rawJson = File.ReadAllText($"../../../Data/{fileName}");
            JObject json = JObject.Parse($"{{ data:{rawJson}}}"); // Wrap JSON in braces for valid Parse syntax
            IList<JToken> results = json["data"].Children().ToList();

            Dictionary<string, CityInfo> allCities = new Dictionary<string, CityInfo>();
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
                if(allCities.ContainsKey(cityName))
                {
                    cityName += $"|{result["admin_name"].ToString()}";
                }
                allCities.Add(cityName, city);
            }

            ParsedCities = allCities;
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
