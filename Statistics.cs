using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Project1_Group_17
{
    public class Statistics
    {
        // Properties
        static readonly HttpClient client = new HttpClient(); //Used to make requests to the bing API
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
            foreach (KeyValuePair<string, CityInfo> city in CityCatalogue)
            {
                string[] nameParts = city.Key.Split('|');
                if (nameParts[0] == cityName)
                {
                    Console.WriteLine($"{cityName}, {city.Value.GetProvince()}. Population: {city.Value.GetPopulation()}");
                }
            }
        }

        public void DisplayLargestPopulationCity(string province)
        {
            ulong largestPopulation = 0;
            string cityName = "";
            foreach (KeyValuePair<string, CityInfo> city in CityCatalogue)
            {
                if (city.Value.GetProvince() == province && city.Value.GetPopulation() > largestPopulation)
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

        /// <summary>
        /// Show a location on the google maps site
        /// </summary>
        /// <param name="lat">Lattitude of the location</param>
        /// <param name="lng">Longitude of the location</param>
        public void ShowCityOnMap(double lat, double lng)
        {
            string url = $"https://www.google.com/maps/@{lat},{lng},15z";
            try
            {
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error launching Internet Explorer\nError: " + ex.Message);
            }
        }

        /// <summary>
        /// Determine the distance between two points using the Bing Map API
        /// </summary>
        /// <param name="lat1">Lattitude of the first city</param>
        /// <param name="lng1">Longitude of the first city</param>
        /// <param name="lat2">Lattitude of the second city</param>
        /// <param name="lng2">Longitude of the second city</param>
        /// <returns>A awaitable task to ensure that the method is called</returns>
        public async Task CalculateDistanceBetweenCities(double lat1, double lng1, double lat2, double lng2)
        {
            //Fetch from the url, in this case the bing API
            //https://docs.microsoft.com/en-us/bingmaps/rest-services/routes/calculate-a-distance-matrix#response
            try
            {
                //Initiate the key and populate the URL to call
                const string bingApiKey = "Ao0BK4GXiMwRy_4CGUMODJcwKwsHzEluEPLwIA5XpVJVxjpZyoY9NOujRdaLRtEM";
                string destMatrix = $"https://dev.virtualearth.net/REST/v1/Routes/DistanceMatrix?origins=" +
                    $"{lat1},{lng1}&destinations={lat2},{lng2}" +
                    $"&travelMode=driving&startTime=&timeUnit=&key={bingApiKey}"; //Travel mode is required but does not impact distance

                //Get the response from the server
                HttpResponseMessage response = await client.GetAsync(destMatrix);

                //Store in JSON
                JObject json = JObject.Parse($"{{ data:{response.Content.ReadAsStringAsync().Result}}}");

                //The structure of the return is in a DistanceMatrix Resource
                //https://docs.microsoft.com/en-us/bingmaps/rest-services/routes/distance-matrix-data
                //Note: returns in KM

                //Get the resource object, held within the returned data (element 3 is the response)
                JToken returnResources = json["data"].Children().ElementAt(3).First().Children().First()["resources"];

                //Determine the travel distance from the result
                JToken results = returnResources.Children().First()["results"].Children().First()["travelDistance"];

                //There was an error with the passed co-ordinates
                if (Convert.ToInt32(results) == -1)
                {
                    throw new Exception("Could not find a valid route between the two passed co-ordinates");
                }

                //Display the results
                Console.WriteLine($"The distance between the two points is: {results}km");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error determining the distance between cities\nError: " + ex.Message);
            }
        }
        // Province Methods
        public void DisplayProvincePopulation(string province)
        {
            ulong totalPopulation = 0;
            foreach (KeyValuePair<string, CityInfo> city in CityCatalogue)
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
