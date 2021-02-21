/// CityInfo.cs
/// Authors: James Dunton, Hunter Bennett, Connor Black
/// Desc: An object representing a city

using System;

namespace Project1_Group_17
{
    public class CityInfo
    {
        // Properties
        private ulong CityID;
        private string CityName;
        private string CityAscii;
        private ulong Population;
        private string Province;
        private double Latitude;
        private double Longitude;
        private string CapitalStatus;

        // Constructor
        public CityInfo(
            ulong cityID, string cityName, string cityAscii, ulong population,
            string province, double latitude, double longitude, string capitalStatus
        )
        {
            CityID = cityID;
            CityName = cityName;
            CityAscii = cityAscii;
            Population = population;
            Province = province;
            Latitude = latitude;
            Longitude = longitude;
            CapitalStatus = capitalStatus;
        }

        // Methods
        /// <summary>
        /// Return province of the city
        /// </summary>
        /// <returns>Province name in a string</returns>
        public string GetProvince()
        {
            return Province;
        }

        /// <summary>
        /// Gets population of the city
        /// </summary>
        /// <returns>Population in a ulong</returns>
        public ulong GetPopulation()
        {
            return Population;
        }

        /// <summary>
        /// Set the population of the city
        /// </summary>
        /// <param name="population">The population to be set</param>
        public void SetPopulation(ulong population)
        {
            Population = population;
        }

        /// <summary>
        /// Get the latitude and longitude
        /// </summary>
        /// <returns>A tuple with the lat (item 1) and long (item 2)</returns>
        public Tuple<double, double> GetLocation()
        {
            return new Tuple<double, double>(Latitude, Longitude);
        }

        /// <summary>
        /// Get the city name
        /// </summary>
        /// <returns>The city name</returns>
        public string GetCityName()
        {
            return CityName;
        }

        /// <summary>
        /// Get the city ASCII characters with no accents
        /// </summary>
        /// <returns>The city name without accents</returns>
        public string GetCityAscii()
        {
            return CityAscii;
        }

        /// <summary>
        /// Determine if the city is capital (will hold "admin")
        /// </summary>
        /// <returns>The capital status</returns>
        public string GetCapitalStatus()
        {
            return CapitalStatus;
        }
    }
}
