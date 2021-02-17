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

        // Constructor
        public CityInfo(
            ulong cityID, string cityName, string cityAscii, ulong population,
            string province, double latitude, double longitude
        )
        {
            CityID = cityID;
            CityName = cityName;
            CityAscii = cityAscii;
            Population = population;
            Province = province;
            Latitude = latitude;
            Longitude = longitude;
        }

        // Methods
        public string GetProvince()
        {
            return Province;
        }

        public ulong GetPopulation()
        {
            return Population;
        }

        public void SetPopulation(ulong population)
        {
            Population = population;
        }

        /// <summary>
        /// Get the latitude and longitude
        /// </summary>
        /// <returns>A tuple with the lat (item 1) and long (item 2)</returns>
        public Tuple<double, double> GetLocation() // TODO: figure out return type, returns lat AND long
        {
            return new Tuple<double, double>(Latitude, Longitude);
        }

        public string GetCityName()
        {
            return CityName;
        }
    }
}
