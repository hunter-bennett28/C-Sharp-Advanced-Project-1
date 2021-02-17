using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Project1_Group_17
{
    public class Statistics
    {
        // Properties
        private Dictionary<string, CityInfo> CityCatalogue;
        private CityPopulationChangeEvent PopulationChangeEvent;// = new CityPopulationChangeEvent();
        

        // Constructor
        public Statistics(string fileName, DataModeler.SupportedFileTypes fileType)
        {
            CityCatalogue = new DataModeler().ParseFile(fileName, fileType);
            PopulationChangeEvent = new CityPopulationChangeEvent();
            PopulationChangeEvent.NotifyPopulationChange += NotifyPopulationChanged;
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

        /// <summary>
        /// Updates the population of a valid city if one is found and the new population is
        /// different than the current population. Updates both the related CityInfo object
        /// as well as the file containing the city information specified by the user.
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="population"></param>
        /// <param name="fileName"></param>
        public void UpdatePopulation(string cityName, ulong population, string fileName)
        {
            try
            {
                if (CityCatalogue.ContainsKey(cityName))
                {
                    CityInfo cityToUpdate = CityCatalogue[cityName];
                    if (cityToUpdate.GetPopulation() == population)
                    {
                        Console.WriteLine($"{cityToUpdate.GetCityName()}, {cityToUpdate.GetProvince()} already has a population of {population}.");
                        return;
                    }
                    switch (fileName)
                    {
                        case "Canadacities-XML.xml":
                            UpdateXMLFile(cityName, population, fileName);
                            break;
                        case "Canadacities-JSON.json":
                            UpdateJSONFile(cityName, population, fileName);
                            break;
                        case "Canadacities.csv":
                            UpdateCSVFile(cityName, population, fileName);
                            break;
                        default:
                            throw new Exception($"Invalid file name: {fileName}");
                    }
                    ulong oldPopulation = cityToUpdate.GetPopulation();
                    cityToUpdate.SetPopulation(population);
                    PopulationChangeEvent.OnPopulationChange(new PopulationChangeEventArgs(
                        cityToUpdate.GetCityName(),
                        cityToUpdate.GetProvince(),
                        oldPopulation,
                        population
                    ));
                }
                else
                {
                    Console.WriteLine($"No city exists in the collection called {cityName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        }

        /// <summary>
        /// Event implementation that notifies the user when a population has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        public void NotifyPopulationChanged(object sender, PopulationChangeEventArgs eventArgs)
        {
            Console.WriteLine($"{eventArgs.CityName}, {eventArgs.ProvinceName} Population Changed From {eventArgs.OldPopulation} to {eventArgs.NewPopulation}");
        }

        /// <summary>
        /// Finds the city being updated in the CSV file and saves the updated population to it
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="population"></param>
        private void UpdateCSVFile(string cityName, ulong population, string fileName)
        {
            string[] nameParts = cityName.Split('|');
            string[] fileText = File.ReadAllLines($"../../../Data/{fileName}");
            for (int i = 0; i < fileText.Length; i++)
            {
                string[] cityParts = fileText[i].Split(',');
                if(cityParts[0] == nameParts[0])
                {
                    if(nameParts.Length == 1 || (nameParts.Length == 2 && cityParts[5] == nameParts[1]))
                    {
                        cityParts[7] = population.ToString();
                        string outputText = $"{string.Join(',', cityParts)}";
                        fileText[i] = outputText;
                        break;
                    }
                }
            }

            File.WriteAllText("../../../Data/Canadacities.csv", $"{string.Join('\n', fileText)}\n");
        }

        /// <summary>
        /// Finds the city being updated in the JSON file and saves the updated population to it
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="population"></param>
        private void UpdateJSONFile(string cityName, ulong population, string fileName)
        {
            string[] nameParts = cityName.Split('|');
            string rawJson = File.ReadAllText($"../../../Data/{fileName}");
            JObject json = JObject.Parse($"{{ data: {rawJson}}}");
            IList<JToken> results = json["data"].Children().ToList();

            foreach (JToken result in results)
            {
                if (result["city"].ToString() == nameParts[0])
                {
                    if (nameParts.Length == 1 || (nameParts.Length == 2 && result["admin_name"].ToString() == nameParts[1]))
                    {
                        result["population"] = population.ToString();
                        break;
                    }
                }
            }
            File.WriteAllText("../../../Data/Canadacities-JSON.json", json["data"].ToString());
        }

        /// <summary>
        /// Finds the city being updated in the XML file and saves the updated population to it
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="population"></param>
        private void UpdateXMLFile(string cityName, ulong population, string fileName)
        {
            string[] nameParts = cityName.Split('|');
            XmlDocument document = new XmlDocument();
            document.Load($"../../../Data/{fileName}");
            XmlNode cityNode = document.SelectSingleNode($"//CanadaCity[city='{nameParts[0]}'{(nameParts.Length == 2 ? $" and admin_name='{nameParts[1]}'" : "")}]");
            XmlNode cityPopulation = cityNode.SelectSingleNode("population");
            cityPopulation.InnerText = population.ToString();
            document.Save("../../../Data/Canadacities-XML.xml");
        }
    }
}
