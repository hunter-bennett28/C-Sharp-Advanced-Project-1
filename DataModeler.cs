using System.Collections.Generic;

namespace Project1_Group_17
{
    public class DataModeler
    {
        public delegate void ParseHandler(string fileName);
        public void ParseXML(string fileName)
        {
            //TODO: Read file and populate the dictionary
        }

        public void ParseJSON(string fileName)
        {

        }

        public void ParseCSV(string fileName)
        {

        }

        public Dictionary<string, CityInfo> ParseFile(string fileName, string fileType)
        {
            Dictionary<string, CityInfo> cities = new Dictionary<string, CityInfo>();

            //TODO

            return cities;
        }
    }
}
