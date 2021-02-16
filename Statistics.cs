using System;
using System.Collections.Generic;

namespace Project1_Group_17
{
    public class Statistics
    {
        // Properties
        private Dictionary<string, CityInfo> CityCatalogue;

        // Constructor
        public Statistics(string fileName, DataModeler.SupportedFileTypes fileType)
        {
            CityCatalogue = new DataModeler().ParseFile(fileName, fileType);
        }

        // City Methods
        public void DisplayCityInfo(string cityName)
        {
            //Either show all cities with same name or ask user which to show
            foreach(KeyValuePair<string, CityInfo> city in CityCatalogue)
            {
                string[] nameParts = city.Key.Split('|');
                if(nameParts[0] == cityName)
                {
                    Console.WriteLine($"{cityName}, {city.Value.GetProvince()}. Population: {city.Value.GetPopulation()}");
                }
            }
        }

        public void DisplayLargestPopulationCity(string province)
        {
            ulong largestPopulation = 0;
            string cityName = "";
            foreach(KeyValuePair<string, CityInfo> city in CityCatalogue)
            {
                if(city.Value.GetProvince() == province && city.Value.GetPopulation() > largestPopulation)
                {
                    largestPopulation = city.Value.GetPopulation();
                    cityName = city.Key.Split('|')[0];
                }
            }
            Console.WriteLine($"Largest Population: {cityName} Population: {largestPopulation}");
        }

        public void DisplaySmallestPopulationCity(string province)
        {
            ulong lowestPopulation = ulong.MaxValue;
            string cityName = "";
            foreach (KeyValuePair<string, CityInfo> city in CityCatalogue)
            {
                if (city.Value.GetProvince() == province && city.Value.GetPopulation() < lowestPopulation)
                {
                    lowestPopulation = city.Value.GetPopulation();
                    cityName = city.Key.Split('|')[0];
                }
            }

            Console.WriteLine($"Smallest Population: {cityName} Population: {lowestPopulation}");
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
        public void DisplayProvincePopulation(string province)
        {
            ulong totalPopulation = 0;
            foreach(KeyValuePair<string, CityInfo> city in CityCatalogue)
            {
                if (city.Value.GetProvince() == province)
                    totalPopulation += city.Value.GetPopulation();
            }

            Console.WriteLine($"{province} Population: {totalPopulation}");
        }
        
        public void DisplayProvinceCities(string province)
        {
            foreach (KeyValuePair<string, CityInfo> city in CityCatalogue)
            {
                if (city.Value.GetProvince() == province)
                {
                    Console.WriteLine(city.Key.Split('|')[0]);
                }
            }
        }

        public void RankProvincesByPopulation()
        {

        }

        public void RankProvincesByCities()
        {

        }

        public void GetCapital(string province)
        {

        }
    }
}
