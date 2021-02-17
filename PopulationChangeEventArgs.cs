using System;

namespace Project1_Group_17
{
    public class PopulationChangeEventArgs : EventArgs
    {
        public string CityName { get; set; }
        public string ProvinceName { get; set; }
        public ulong NewPopulation { get; set; }
        public ulong OldPopulation { get; set; }

        public PopulationChangeEventArgs(string cityName, string provinceName, ulong oldPopulation, ulong newPopulation)
        {
            CityName = cityName;
            ProvinceName = provinceName;
            NewPopulation = newPopulation;
            OldPopulation = oldPopulation;
        }
    }
}
