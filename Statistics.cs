using System.Collections.Generic;

namespace Project1_Group_17
{
    public class Statistics
    {
        // Properties
        private Dictionary<string, CityInfo> CityCatalogue;

        // Constructor
        public Statistics(string fileName, string fileType)
        {
            
        }

        // City Methods
        public CityInfo DisplayCityInfo(string cityName)
        {
            //Either show all cities with same name or ask user which to show
            if (CityCatalogue.ContainsKey(cityName))
                return CityCatalogue[cityName];
            return null;
        }

        public int DisplayLargestPopulationCity(string province)
        {
            int largestPopulation = 0;
            foreach(KeyValuePair<string, CityInfo> city in CityCatalogue)
            {
                if(city.Value.GetProvince() == province && city.Value.GetPopulation() > largestPopulation)
                {
                    largestPopulation = city.Value.GetPopulation();
                }
            }

            return largestPopulation;
        }

        public int DisplaySmallestPopulationCity(string province)
        {
            int lowestPopulation = int.MaxValue;
            foreach (KeyValuePair<string, CityInfo> city in CityCatalogue)
            {
                if (city.Value.GetProvince() == province && city.Value.GetPopulation() < lowestPopulation)
                {
                    lowestPopulation = city.Value.GetPopulation();
                }
            }

            return lowestPopulation;
        }

        public void CompareCitiesPopulation()
        {

        }

        public void ShowCityOnMap()
        {

        }

        public void CalculateDistanceBetweenCities()
        {

        }

        // Province Methods
        public int DisplayProvincePopulation(string province)
        {
            int totalPopulation = 0;
            foreach(KeyValuePair<string, CityInfo> city in CityCatalogue)
            {
                if (city.Value.GetProvince() == province)
                    totalPopulation += city.Value.GetPopulation();
            }

            return totalPopulation;
        }
        
        public List<string> DisplayProvinceCities(string province)
        {
            List<string> allCitiesInProvince = new List<string>();
            foreach (KeyValuePair<string, CityInfo> city in CityCatalogue)
            {
                if (city.Value.GetProvince() == province)
                    allCitiesInProvince.Add(city.Key);
            }

            return allCitiesInProvince;
        }

        public void RankProvincesByPopulation()
        {

        }

        public void RankProvincesByCities()
        {

        }
    }
}
