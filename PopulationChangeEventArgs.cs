using System;

namespace Project1_Group_17
{
    public class PopulationChangeEventArgs : EventArgs
    {
        public string CityName { get; set; }
        public string ProvinceName { get; set; }
        public ulong Population { get; set; }
    }
}
