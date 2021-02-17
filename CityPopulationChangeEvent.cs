using System;

namespace Project1_Group_17
{
    public class CityPopulationChangeEvent
    {
        public event EventHandler<PopulationChangeEventArgs> NotifyPopulationChange;

        /// <summary>
        /// Invokes all event subscribers registered to NotifyPopulationChange when called
        /// </summary>
        /// <param name="e"></param>
        public void OnPopulationChange(PopulationChangeEventArgs e)
        {
            NotifyPopulationChange?.Invoke(this, e);
        }
    }
}
