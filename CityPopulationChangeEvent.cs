using System;

namespace Project1_Group_17
{
    public class CityPopulationChangeEvent
    {
        public event EventHandler<PopulationChangeEventArgs> NotifyPopulationChange;

        public void OnPopulationChange(PopulationChangeEventArgs e)
        {
            NotifyPopulationChange?.Invoke(this, e);
        }
    }
}
