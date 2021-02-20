/// PopulationChangeEventArgs.cs
/// Authors: Connor Black, Hunter Bennett, James Dunton
/// Desc: The arguements for an event to update the population of a city

using System;

namespace Project1_Group_17
{
    public class PopulationChangeEventArgs : EventArgs
    {
        //Properties
        public string CityName { get; set; }
        public string ProvinceName { get; set; }
        public ulong NewPopulation { get; set; }
        public ulong OldPopulation { get; set; }

        //Constructor
        public PopulationChangeEventArgs(string cityName, string provinceName, ulong oldPopulation, ulong newPopulation)
        {
            CityName = cityName;
            ProvinceName = provinceName;
            NewPopulation = newPopulation;
            OldPopulation = oldPopulation;
        }
    }
}
