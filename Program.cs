using System;

namespace Project1_Group_17
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Canadian Cities Data Analayzer © Hunter Bennett, Connor Black, James Dunton");
            char selection;
            do
            {
                Console.WriteLine("\nAvailable data files:");
                Console.WriteLine("\t1) Canadacities-JSON.json");
                Console.WriteLine("\t2) Canadacities-XML.xml");
                Console.WriteLine("\t3) Canadacities.csv");

                Console.Write("Select a data file to parse: ");
                selection = Console.ReadKey().KeyChar;
                if (selection >= '1' && selection <= '3')
                    break;
                Console.WriteLine("Invalid option. Valid options are 1, 2, 3.");
            } while (true);
            Console.WriteLine();

            Statistics citiesStats = null;
            switch (selection)
            {
                case '1':
                    citiesStats = new Statistics("Canadacities-JSON.json", DataModeler.SupportedFileTypes.JSON);
                    break;
                case '2':
                    citiesStats = new Statistics("Canadacities-XML.xml", DataModeler.SupportedFileTypes.XML);
                    break;
                case '3':
                    citiesStats = new Statistics("Canadacities.csv", DataModeler.SupportedFileTypes.CSV);
                    break;
            }

            citiesStats.DisplayLargestPopulationCity("Ontario");
            citiesStats.DisplaySmallestPopulationCity("Ontario");
            citiesStats.DisplayProvincePopulation("Ontario");
            citiesStats.DisplayCityInfo("London");
            citiesStats.DisplayCityInfo("Windsor");
            //citiesStats.DisplayProvinceCities("Ontario");
            citiesStats.GetCapital("Ontario");
            citiesStats.CompareCitiesPopulation("Toronto", "London");

            //citiesStats.UpdatePopulation("Selkirk", 9986, "Canadacities-JSON.json");
            //citiesStats.UpdatePopulation("Selkirk", 9986, "Canadacities-XML.xml");
            citiesStats.UpdatePopulation("Selkirk", 9986, "Canadacities.csv");

            Console.WriteLine(citiesStats.IsValidCity("London"));
            Console.WriteLine(citiesStats.IsValidCity("Ontario"));
            Console.WriteLine(citiesStats.IsValidProvince("London"));
            Console.WriteLine(citiesStats.IsValidProvince("Ontario"));

            //TODO: implement options
        }
    }
}
