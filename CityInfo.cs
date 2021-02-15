namespace Project1_Group_17
{
    public class CityInfo
    {
        // Properties
        private ulong CityID;
        private string CityName;
        private string CityAscii;
        private int Population;
        private string Province;
        private float Latitude;
        private float Longitude;

        // Constructor
        public CityInfo(
            ulong cityID, string cityName, string cityAscii, int population,
            string province, float latitude, float longitude
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

        public int GetPopulation()
        {
            return Population;
        }

        public void GetLocation() // TODO: figure out return type, returns lat AND long
        {

        }
    }
}
