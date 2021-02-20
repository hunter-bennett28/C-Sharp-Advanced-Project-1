/// CityPopulationChangeEvent.cs
/// Authors: Hunter Bennett, Connor Black, James Dunton
/// Desc: The event that will trigger when the population is changed

using System;

namespace Project1_Group_17
{
    public class CityPopulationChangeEvent
    {
        public event EventHandler<PopulationChangeEventArgs> NotifyPopulationChange;

        /// <summary>
        /// Invokes all event subscribers registered to NotifyPopulationChange when called
        /// </summary>
        /// <param name="e">The event arguements</param>
        public void OnPopulationChange(PopulationChangeEventArgs e)
        {
            NotifyPopulationChange?.Invoke(this, e);
        }
    }
}
