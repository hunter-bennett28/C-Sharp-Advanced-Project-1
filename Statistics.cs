/// Statistics.cs
/// Authors: Hunter Bennett, Connor Black, James Dunton
/// Desc: Various methods to display statistics on cityInfo objects

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace Project1_Group_17
{
    public class Statistics
    {
        // Properties
        static readonly HttpClient client = new HttpClient(); //Used to make requests to the bing API
        private const int ColumnWidth = 26; // width per column when printing multiple entries per row
        private Dictionary<string, CityInfo> CityCatalogue;
        private CityPopulationChangeEvent PopulationChangeEvent;

        // Constructor
        public Statistics(string fileName, DataModeler.SupportedFileTypes fileType)
        {
            //Parse the file and set up the event
            CityCatalogue = new DataModeler().ParseFile(fileName, fileType);
            PopulationChangeEvent = new CityPopulationChangeEvent();
            PopulationChangeEvent.NotifyPopulationChange += NotifyPopulationChanged;
        }

        // City Methods

        /// <summary>
        /// Checks if the city is contained in the dictionary
        /// </summary>
        /// <param name="cityName">The city name.</param>
        /// <returns>A bool representing if the city given is valid.</returns>
        public bool IsValidCity(string cityName)
        {
            cityName = cityName.ToLower();
            foreach (var city in CityCatalogue)
                if (cityName == city.Value.GetCityName().ToLower() || cityName == city.Value.GetCityAscii().ToLower())
                    return true;
            return false;
        }

        /// <summary>
        /// Helper method to output city details
        /// </summary>
        /// <param name="cityName">City name to be displayed</param>
        /// <param name="province">Province to be displayed</param>
        /// <param name="population">Population to be displayed</param>
        private void PrintCityDetails(string cityName, string province, ulong population)
        {
            Console.WriteLine($"\nCity:\t\t{CapitalizeString(cityName)}");
            Console.WriteLine($"Province:\t{CapitalizeString(province)}");
            Console.WriteLine($"Population:\t{string.Format("{0:n0}", population)}\n");
        }

        /// <summary>
        /// Displays the city information.
        /// </summary>
        /// <param name="cityName">Name of the city.</param>
        public void DisplayCityInformation(string cityName)
        {
            CityInfo chosenCity = GetSpecificCity(cityName);
            if (chosenCity != null)
                PrintCityDetails(chosenCity.GetCityName(), chosenCity.GetProvince(), chosenCity.GetPopulation());
        }

        /// <summary>
        /// Displays the largest population city for a given province.
        /// </summary>
        /// <param name="province">The province.</param>
        public void DisplayLargestPopulationCity(string province)
        {
            ulong largestPopulation = 0;
            string cityName = "";

            //Find the largest city in the province
            foreach (KeyValuePair<string, CityInfo> city in CityCatalogue)
            {
                if (city.Value.GetProvince().ToLower() == province.ToLower() && city.Value.GetPopulation() > largestPopulation)
                {
                    largestPopulation = city.Value.GetPopulation();
                    cityName = city.Key.Split('|')[0];
                }
            }

            province = CapitalizeString(province);
            Console.WriteLine($"\nThe largest population in {CapitalizeString(province)} is:");
            PrintCityDetails(cityName, province, largestPopulation);
        }

        /// <summary>
        /// Displays the smallest population city in the given province.
        /// </summary>
        /// <param name="province">The province.</param>
        public void DisplaySmallestPopulationCity(string province)
        {
            ulong lowestPopulation = ulong.MaxValue;
            string cityName = "";

            //Get the smallest city in the province
            foreach (KeyValuePair<string, CityInfo> city in CityCatalogue)
            {
                if (city.Value.GetProvince().ToLower() == province.ToLower() && city.Value.GetPopulation() < lowestPopulation)
                {
                    lowestPopulation = city.Value.GetPopulation();
                    cityName = city.Key.Split('|')[0];
                }
            }

            province = CapitalizeString(province);
            Console.WriteLine($"\nThe smallest population in {CapitalizeString(province)} is:");
            PrintCityDetails(cityName, province, lowestPopulation);
        }

        /// <summary>
        /// Reports which city between two given cities has a higher population
        /// </summary>
        /// <param name="city1">First city object to compare</param>
        /// <param name="city2">Second city object to compare</param>
        public void CompareCitiesPopulation(CityInfo city1, CityInfo city2)
        {
            if (city1 == city2)
            {
                Console.WriteLine("The two cities provided are the same");
                return;
            }
            
            CityInfo smallerCity = city1.GetPopulation() < city2.GetPopulation() ? city1 : city2;
            CityInfo largerCity = smallerCity == city1 ? city2 : city1;

            Console.WriteLine($"\nLarger City:\t{CapitalizeString(largerCity.GetCityName())}, {CapitalizeString(largerCity.GetProvince())} ({string.Format("{0:n0}", largerCity.GetPopulation())})");
            Console.WriteLine($"Smaller City:\t{CapitalizeString(smallerCity.GetCityName())}, {CapitalizeString(smallerCity.GetProvince())} ({string.Format("{0:n0}", smallerCity.GetPopulation())})\n");
        }

        /// <summary>
        /// Shows a location of a city on the google maps site
        /// </summary>
        /// <param name="cityKey">City to be shown</param>
        public void ShowCityOnMap(string cityName)
        {
            Tuple<double, double> cityLocation = null;

            //Find the city by name
            foreach (var item in CityCatalogue)
                if (item.Key.ToLower() == cityName.ToLower())
                    cityLocation = item.Value.GetLocation();

            //No city was found
            if (cityLocation == null)
                Console.WriteLine($"No city was found with the name {CapitalizeString(cityName)}");

            //Build the url with the latitude and longitude
            string url = $"https://www.google.com/maps/@{cityLocation.Item1},{cityLocation.Item2},15z";
            try
            {
                //Launch the browser
                Console.WriteLine("\nLaunching Google Maps in browser...\n");
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error launching Browser: {ex.Message}\n");
            }
        }

        /// <summary>
        /// Determines the distance between two points using the Bing Map API
        /// </summary>
        /// <param name="city1">First city</param>
        /// <param name="city2">Second city</param>
        /// <returns>An awaitable task to ensure that the method is called</returns>
        public async Task CalculateDistanceBetweenCities(CityInfo city1, CityInfo city2)
        {
            //Fetch from the url, in this case the bing API
            //https://docs.microsoft.com/en-us/bingmaps/rest-services/routes/calculate-a-distance-matrix#response
            try
            {
                //Get the lattitudes and logitudes from the cities
                Tuple<double, double> c1Loc = city1.GetLocation();
                double lat1 = c1Loc.Item1, lng1 = c1Loc.Item2;

                Tuple<double, double> c2Loc = city2.GetLocation();
                double lat2 = c2Loc.Item1, lng2 = c2Loc.Item2;

                //Initiate the key and populate the URL to call
                const string bingApiKey = "Ao0BK4GXiMwRy_4CGUMODJcwKwsHzEluEPLwIA5XpVJVxjpZyoY9NOujRdaLRtEM";
                string destMatrix = $"https://dev.virtualearth.net/REST/v1/Routes/DistanceMatrix?origins=" +
                    $"{lat1},{lng1}&destinations={lat2},{lng2}" +
                    $"&travelMode=driving&startTime=&timeUnit=&key={bingApiKey}"; //Travel mode is required but does not impact distance

                //The structure of the return is in a DistanceMatrix Resource
                //https://docs.microsoft.com/en-us/bingmaps/rest-services/routes/distance-matrix-data
                //Note: returns in KM

                //Get the resource object, held within the returned data (element 3 is the response)
                HttpResponseMessage response = await client.GetAsync(destMatrix);
                JObject json = JObject.Parse($"{{ data:{response.Content.ReadAsStringAsync().Result}}}");

                //Get the resource object from returned data (element 3 is response)
                JToken returnResources = json["data"].Children().ElementAt(3).First().Children().First()["resources"];

                //Determine the travel distance from the result
                JToken results = returnResources.Children().First()["results"].Children().First()["travelDistance"];

                if (Convert.ToInt32(results) == -1)
                    throw new Exception("Could not find a valid route between the two passed co-ordinates");

                Console.WriteLine($"\nThe distance between the two cities is: {string.Format("{0:n0}", results)}km\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError determining the distance between cities: {ex.Message}\n");
            }
        }

        // Province Methods

        /// <summary>
        /// Determines whether the province entered by the user exists in the catalogue.
        /// </summary>
        /// <param name="province">The province.</param>
        /// <returns>Returns true if it is a valid province</returns>
        public bool IsValidProvince(string province)
        {
            foreach (var city in CityCatalogue)
                if (city.Value.GetProvince().ToLower() == province.ToLower())
                    return true;

            return false;
        }

        /// <summary>
        /// Displays the population for the entered province
        /// </summary>
        /// <param name="province">Province to display province for</param>
        public void DisplayProvincePopulation(string province)
        {
            ulong totalPopulation = 0;
            foreach (KeyValuePair<string, CityInfo> city in CityCatalogue)
                if (city.Value.GetProvince().ToLower() == province.ToLower())
                    totalPopulation += city.Value.GetPopulation();

            Console.WriteLine($"\nProvince: {CapitalizeString(province)}");
            Console.WriteLine($"Population: {string.Format("{0:n0}", totalPopulation)}\n");
        }

        /// <summary>
        /// Displays the cities for the given province in 3 columns
        /// </summary>
        /// <param name="province">Province to find the cities for</param>
        public void DisplayProvinceCities(string province)
        {
            //Get all the cities in the province
            SortedSet<string> cityNames = new SortedSet<string>();
            foreach (KeyValuePair<string, CityInfo> city in CityCatalogue)
                if (city.Value.GetProvince().ToLower() == province.ToLower())
                    cityNames.Add(city.Value.GetCityName());

            //Output the cities in the province
            Console.WriteLine($"\nCities in {CapitalizeString(province)}");
            int index = 0;
            foreach (string cityName in cityNames)
            {
                if (index++ % 3 == 0)
                    Console.WriteLine();
                Console.Write(string.Format($"{{0,-{ColumnWidth}}}", cityName)); // Print in specified length column
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Displays the ranked provinces by population in ascending order
        /// </summary>
        public void RankProvincesByPopulation()
        {
            Dictionary<string, ulong> provincePopulation = new Dictionary<string, ulong>();

            //Tally up the province for each province
            foreach (var city in CityCatalogue)
            {
                if (provincePopulation.ContainsKey(city.Value.GetProvince()))
                    provincePopulation[city.Value.GetProvince()] += city.Value.GetPopulation();
                else
                    provincePopulation.Add(city.Value.GetProvince(), city.Value.GetPopulation());
            }

            //Table header
            Console.WriteLine("\n{0,-35}|{1,14}", "Province", "Population");
            Console.WriteLine(new string('-', 50));

            //Display table contents
            foreach (var province in provincePopulation.OrderBy(item => item.Value))
            {
                Console.WriteLine("{0,-35}|{1,14}", province.Key, string.Format("{0:n0}", province.Value));
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Displays the ranked provinces by cities in ascending order
        /// </summary>
        public void RankProvincesByCities()
        {
            Dictionary<string, ulong> provinceCities = new Dictionary<string, ulong>();
            foreach (var city in CityCatalogue)
            {
                //Count cities in each province
                if (provinceCities.ContainsKey(city.Value.GetProvince()))
                    provinceCities[city.Value.GetProvince()]++;
                else
                    provinceCities.Add(city.Value.GetProvince(), 1);
            }

            //Table header
            Console.WriteLine("\n{0,-35}|{1,14}", "Province", "Cities");
            Console.WriteLine(new string('-', 50));

            //Display table contents
            foreach (var province in provinceCities.OrderBy(item => item.Value))
                Console.WriteLine("{0,-35}|{1,14}", province.Key, string.Format("{0:n0}", province.Value));

            Console.WriteLine();
        }

        /// <summary>
        /// Reports which city is the capital of the given province
        /// </summary>
        /// <param name="province">Province name</param>
        public void GetCapital(string province)
        {
            foreach (KeyValuePair<string, CityInfo> city in CityCatalogue)
            {
                if (city.Value.GetProvince().ToLower() == province.ToLower() && city.Value.GetCapitalStatus() == "admin")
                {
                    Console.WriteLine($"\nThe capital of {CapitalizeString(province)} is {city.Value.GetCityName()}.\n");
                    return;
                }
            }

            Console.WriteLine($"\nNo capital found for {CapitalizeString(province)}.\n");
        }

        /// <summary>
        /// Updates the population of a valid city if one is found and the new population is
        /// different than the current population. Updates both the related CityInfo object
        /// as well as the file containing the city information specified by the user.
        /// </summary>
        /// <param name="cityName">The city to be found</param>
        /// <param name="population">The population to change it to</param>
        /// <param name="fileName">The file to alter</param>
        public void UpdatePopulation(string cityName, ulong population, string fileName)
        {
            try
            {
                CityInfo cityToUpdate = GetSpecificCity(cityName);
                if (cityToUpdate != null)
                {
                    if (cityToUpdate.GetPopulation() == population)
                    {
                        Console.WriteLine($"{cityToUpdate.GetCityName()}, {cityToUpdate.GetProvince()} already has a population of {string.Format("{0:n0}", population)}.");
                        return;
                    }
                    switch (fileName)
                    {
                        case "Canadacities-XML.xml":
                            UpdateXMLFile(cityName, cityToUpdate.GetProvince(), population, fileName);
                            break;
                        case "Canadacities-JSON.json":
                            UpdateJSONFile(cityName, cityToUpdate.GetProvince(), population, fileName);
                            break;
                        case "Canadacities.csv":
                            UpdateCSVFile(cityName, cityToUpdate.GetProvince(), population, fileName);
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
                    Console.WriteLine($"\nNo city exists in the collection called {CapitalizeString(cityName)}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Population Update Failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Event implementation that notifies the user when a population has changed
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="eventArgs">The event arguements</param>
        public void NotifyPopulationChanged(object sender, PopulationChangeEventArgs eventArgs)
        {
            string oldPopulation = string.Format("{0:n0}", eventArgs.OldPopulation);
            string newPopulation = string.Format("{0:n0}", eventArgs.NewPopulation);
            Console.WriteLine($"\n{CapitalizeString(eventArgs.CityName)}, {CapitalizeString(eventArgs.ProvinceName)} Population Changed From {oldPopulation} to {newPopulation}\n");
        }

        /// <summary>
        /// Finds the city being updated in the CSV file and saves the updated population to it
        /// </summary>
        /// <param name="cityName">The city to be altered</param>
        /// <param name="population">The new population value</param>
        /// <param name="fileName">The file to be altered</param>
        private void UpdateCSVFile(string cityName, string province, ulong population, string fileName)
        {
            string[] fileText = File.ReadAllLines($"../../../Data/{fileName}");
            for (int i = 0; i < fileText.Length; i++)
            {
                //Find the city
                string[] cityParts = fileText[i].Split(',');
                if (cityParts[0].ToLower() == cityName.ToLower() || cityParts[1].ToLower() == cityName.ToLower())
                {
                    //Make sure it is the same province
                    if (cityParts[5].ToLower() == province.ToLower())
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
        /// <param name="cityName">The city to be altered</param>
        /// <param name="population">The new population value</param>
        /// <param name="fileName">The file to be altered</param>
        private void UpdateJSONFile(string cityName, string province, ulong population, string fileName)
        {
            string rawJson = File.ReadAllText($"../../../Data/{fileName}");
            JObject json = JObject.Parse($"{{ data: {rawJson}}}");
            IList<JToken> results = json["data"].Children().ToList();

            foreach (JToken result in results)
            {
                //Find the city
                if (result["city"].ToString().ToLower() == cityName.ToLower() || result["city_ascii"].ToString().ToLower() == cityName.ToLower())
                {
                    //Make sure province is correct
                    if (result["admin_name"].ToString().ToLower() == province.ToLower())
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
        /// <param name="cityName">The city to be altered</param>
        /// <param name="population">The new population value</param>
        /// <param name="fileName">The file to be altered</param>
        private void UpdateXMLFile(string cityName, string province, ulong population, string fileName)
        {
            string[] nameParts = cityName.Split('|');
            XmlDocument document = new XmlDocument();
            document.Load($"../../../Data/{fileName}");
            XmlNode cityNode = document.SelectSingleNode(
                $"//CanadaCity[city='{CapitalizeString(cityName)}' or city_ascii='{CapitalizeString(cityName)}' and admin_name='{CapitalizeString(province)}']"
            );
            XmlNode cityPopulation = cityNode.SelectSingleNode("population");
            cityPopulation.InnerText = population.ToString();
            document.Save("../../../Data/Canadacities-XML.xml");
        }

        /// <summary>
        /// Displays the province list.
        /// </summary>
        public void DisplayProvinceList()
        {
            Console.WriteLine("\nAll Provinces:\n");
            SortedSet<string> provinces = new SortedSet<string>();
            foreach (var item in CityCatalogue)
                provinces.Add(item.Value.GetProvince());

            foreach (string prov in provinces)
                Console.WriteLine(prov);
        }

        /// <summary>
        /// Gets the specific city. If more than one city has that name, ask the user what city they would like to select.
        /// </summary>
        /// <param name="cityName">Name of the city.</param>
        /// <returns>The city in a CityInfo object</returns>
        public CityInfo GetSpecificCity(string cityName)
        {
            List<CityInfo> matchedCities = new List<CityInfo>();

            //Either show all cities with same name or ask user which to show
            foreach (KeyValuePair<string, CityInfo> city in CityCatalogue)
            {
                if (cityName.ToLower() == city.Value.GetCityName().ToLower() || cityName.ToLower() == city.Value.GetCityAscii().ToLower())
                    matchedCities.Add(city.Value);
            }

            if (matchedCities.Count == 0)
            {
                //No cities were found
                Console.WriteLine($"\nNo city exists named {CapitalizeString(cityName)}.\n");
                return null;
            }
            else if (matchedCities.Count > 1)
            {
                //Display the options for multiple cities under the same name
                Console.WriteLine($"\nMultiple Cities Named {matchedCities[0].GetCityName()}. Please Select Desired City.\n");

                for (int i = 0; i < matchedCities.Count; i++)
                    Console.WriteLine($"\t{i + 1}) {matchedCities[i].GetCityName()}, {matchedCities[i].GetProvince()}");

                //Prompt for user input
                do
                {
                    Console.Write("\nPlease Select Desired City (ex 1, 2): ");
                    int choice;
                    if (int.TryParse(Console.ReadLine(), out choice) && choice > 0 && choice <= matchedCities.Count)
                        return matchedCities[choice - 1];

                    Console.WriteLine("Invalid choice. Please try again.");
                } while (true);

            }
            else
                return matchedCities[0];
        }

        /// <summary>
        /// Capitalize the first letter of a string
        /// </summary>
        /// <param name="toCapitalize">The string to capitalize</param>
        /// <returns>The first letter capitalized</returns>
        public string CapitalizeString(string toCapitalize)
        {
            if (toCapitalize.Length == 0)
                return toCapitalize;
            else if (toCapitalize.Length == 1)
                return toCapitalize.ToUpper();
            else
            {
                string capitalized = "";
                foreach (string word in toCapitalize.Split(' '))
                    capitalized += word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower() + " ";

                return capitalized.Trim();
            }
        }
    }
}
