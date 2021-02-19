using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1_Group_17
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Canadian Cities Data Analayzer © Hunter Bennett, Connor Black, James Dunton");

            Statistics citiesStats = StartMenu();
            do
            {
                string selection;
                string titleText = "Mode Selection Menu";
                string dash = new string('-', titleText.Length);
                Console.WriteLine($"{dash}\n{titleText}\n{dash}\n");
                Console.WriteLine("\nAvailable selections:");
                Console.WriteLine("\t1) Display city statistics menu.");
                Console.WriteLine("\t2) Display province statistics menu.");
                Console.Write("Please make a selection: ");
                selection = Console.ReadLine();
                switch (selection.ToLower())
                {
                    case "1":
                        CityMenu(citiesStats);
                        break;
                    case "2":
                        ProvinceMenu(citiesStats);
                        break;
                    case "exit":
                        return;
                    case "restart":
                        citiesStats = StartMenu();
                        break;
                    default:
                        Console.Write("\nInvalid selection, please enter a valid selection: ");
                        break;
                }
            } while (true);

        }
        /// <summary>
        /// First menu that the user sees.
        /// </summary>
        /// <returns>Statitics object</returns>
        static Statistics StartMenu()
        {
            Statistics citiesStats = null;
            bool valid = false;
            string titleText = "Main Menu";
            string selection;
            string dash = new string('-', titleText.Length);
            do
            {
                Console.WriteLine($"\n{dash}\n{titleText}\n{dash}\n");
                Console.WriteLine("-Enter 'exit' to exit the application at any menu in this program.");
                Console.WriteLine("-Enter 'restart' at any menu to return to this menu.");
                Console.WriteLine("\nAvailable data files:");
                Console.WriteLine("\t1) Canadacities-JSON.json");
                Console.WriteLine("\t2) Canadacities-XML.xml");
                Console.WriteLine("\t3) Canadacities.csv");
                Console.Write("Please make a selection(ex. 1, 2): ");
                selection = Console.ReadLine();
                switch (selection.ToLower())
                {
                    case "1":
                        citiesStats = new Statistics("Canadacities-JSON.json", DataModeler.SupportedFileTypes.JSON);
                        valid = true;
                        break;
                    case "2":
                        citiesStats = new Statistics("Canadacities-XML.xml", DataModeler.SupportedFileTypes.XML);
                        valid = true;
                        break;
                    case "3":
                        citiesStats = new Statistics("Canadacities.csv", DataModeler.SupportedFileTypes.CSV);
                        valid = true;
                        break;
                    case "exit":
                        System.Environment.Exit(0);
                        break;
                    case "restart":
                        break;
                    default:
                        Console.Write("\nInvalid selection, please enter a valid selection: ");
                        break;
                }
            } while (!valid);
            return citiesStats;
        }
        /// <summary>
        /// Menu for province stats.
        /// </summary>
        /// <param name="cityStats"></param>
        static void ProvinceMenu(Statistics cityStats)
        {
            string selection;
            string titleText = "Provinces Menu";
            string dash = new string('-', titleText.Length);

            do
            {
                Console.WriteLine($"\n{dash}\n{titleText}\n{dash}\n");
                Console.WriteLine("\nAvailable selections:");
                Console.WriteLine("\t1) Display the population for a given province.");
                Console.WriteLine("\t2) Display all the cities in a given province.");
                Console.WriteLine("\t3) Rank provinces by population.");
                Console.WriteLine("\t4) Rank provinces by the number of cities.");
                Console.WriteLine("\t5) Display the capital city for the given province.");
                Console.Write("Please make a selection(ex. 1, 2): ");
                selection = Console.ReadLine();
                string response;
                switch (selection.ToLower())
                {
                    case "1":
                        Console.Write("\nPlease enter the province name to see the population for that given province: ");
                        response = ProvinceValidator(cityStats);
                        cityStats.DisplayProvincePopulation(response);
                        Console.Write("\nEnter 'y' to return to the Mode Selection Menu. Press 'Enter' to continue in current menu.");
                        if (Console.ReadKey().KeyChar == 'y')
                            return;
                        break;
                    case "2":
                        Console.Write("\nPlease enter the province name to see the cities located in that given province: ");
                        response = ProvinceValidator(cityStats);
                        cityStats.DisplayProvinceCities(response);
                        Console.Write("\nEnter 'y' to return to the Mode Selection Menu. Press 'Enter' to continue in current menu.");
                        if (Console.ReadKey().KeyChar == 'y')
                            return;
                        break;
                    case "3":
                        Console.WriteLine("Here's the rank breakdown of each province by population: ");
                        cityStats.RankProvincesByPopulation();
                        Console.Write("\nEnter 'y' to return to the Mode Selection Menu. Press 'Enter' to continue in current menu.");
                        if (Console.ReadKey().KeyChar == 'y')
                            return;
                        break;
                    case "4":
                        Console.WriteLine("Here's the rank breakdown of each province by number of cities: ");
                        cityStats.RankProvincesByCities();
                        Console.Write("\nEnter 'y' to return to the Mode Selection Menu. Press 'Enter' to continue in current menu.");
                        if (Console.ReadKey().KeyChar == 'y')
                            return;
                        break;
                    case "5":
                        Console.Write("\nPlease enter a province to view the capital city of that province: ");
                        response = ProvinceValidator(cityStats);
                        cityStats.GetCapital(response);
                        Console.Write("\nEnter 'y' to return to the Mode Selection Menu. Press 'Enter' to continue in current menu.");
                        if (Console.ReadKey().KeyChar == 'y')
                            return;
                        break;
                    case "exit":
                        System.Environment.Exit(0);
                        break;
                    case "restart":
                        cityStats = StartMenu();
                        return;
                    default:
                        Console.Write("\nInvalid selection, please enter a valid selection: ");
                        break;
                }
            } while (true);
        }
        /// <summary>
        /// Menu for city statistics.
        /// </summary>
        /// <param name="cityStats"></param>
        static void CityMenu(Statistics cityStats)
        {
            bool valid = false;
            string selection;
            string titleText = "Cities Menu";
            string dash = new string('-', titleText.Length);
          
            do
            {
                Console.WriteLine($"\n{dash}\n{titleText}\n{dash}\n");
                Console.WriteLine("\nAvailable selections:");
                Console.WriteLine("\t1) Display city's info.");
                Console.WriteLine("\t2) Display the city with the largest population within a given province.");
                Console.WriteLine("\t3) Display the city with the smallest population within a given province.");
                Console.WriteLine("\t4) Compare the population of two cities.");
                Console.WriteLine("\t5) Display the city on a map.");
                Console.WriteLine("\t6) Calculate the distance between two cities.");
                Console.Write("Please make a selection(ex. 1, 2): ");
                selection = Console.ReadLine();
                string response;
                string[] resp;
                switch (selection.ToLower())

                {
                    case "1":
                        Console.Write("\nPlease enter a city's name to display the city's info: ");
                        response = OneCityValidator(cityStats);
                        cityStats.DisplayCityInfo(response);
                        Console.WriteLine("Enter 'y' to return to the Mode Selection Menu. Press 'Enter' to continue in current menu.");
                        if (Console.ReadKey().KeyChar == 'y')
                            return;
                        break;
                    case "2":
                        Console.Write("\nPlease enter a province name to return the city with the largest population in that given province: ");
                        response = ProvinceValidator(cityStats);
                        cityStats.DisplayLargestPopulationCity(response);
                        Console.Write("Enter 'y' to return to the Mode Selection Menu. Press 'Enter' to continue in current menu.");
                        if (Console.ReadKey().KeyChar == 'y')
                            return;
                        break;
                    case "3":
                        Console.Write("\nPlease enter a province name to return the city with the smallest population in that given province: ");
                        response = ProvinceValidator(cityStats);
                        cityStats.DisplaySmallestPopulationCity(response);
                        Console.Write("Enter 'y' to return to the Mode Selection Menu. Press 'Enter' to continue in current menu. ");
                        if (Console.ReadKey().KeyChar == 'y')
                            return;
                        break;
                    case "4":
                        Console.Write("\nPlease enter two cities, separated by ',' to compare the population of those two cities: ");
                        resp = TwoCityValidator(cityStats);
                        cityStats.CompareCitiesPopulation(resp[0], resp[1]);
                        Console.Write("Enter 'y' to return to the Mode Selection Menu. Press 'Enter' to continue in current menu. ");
                        if (Console.ReadKey().KeyChar == 'y')
                            return;
                        break;
                    case "5":
                        Console.Write("\nPlease enter a city's name and a porvince's name seperated by a comma to display the city on a map(ex. 'London, Ontario'): ");
                        resp = CityProvValidator(cityStats);
                        cityStats.ShowCityOnMap(resp[0], resp[1]);
                        Console.WriteLine(":");
                        if (Console.ReadKey().KeyChar == 'y')
                            return;
                        break;
                    case "6":
                        Console.Write("\nPlease enter two cities, separated by ',' to compare the population of those two cities: ");
                        resp = TwoCityValidator(cityStats);
                        //Task t = new Task();
                        Task result = cityStats.CalculateDistanceBetweenCities(resp[0], resp[1]); result.Wait();// how do I call await on this void method?
                        Console.Write("Enter 'y' to return to the Mode Selection Menu. Press 'Enter' to continue in current menu.");
                        if (Console.ReadKey().KeyChar == 'y')
                            return;
                        break;
                    case "exit":
                        System.Environment.Exit(0);
                        break;
                    case "restart":
                        cityStats = StartMenu();
                        return;

                    default:
                        Console.Write("\nInvalid selection, please enter a valid selection: ");
                        break;
                }
            } while (true);
        }

        /// <summary>
        /// One City Validator.
        /// </summary>
        /// <param name="cityStats">The city stats object.</param>
        /// <returns></returns>
        static string OneCityValidator(Statistics cityStats)
        {
            string response;
            do
            {
                response = Console.ReadLine();
                if (!cityStats.CityIsValid(response))
                {
                    Console.Write("\nInvalid response. Type 'y' to see a list of available cities, or press enter to continue: ");
                    char yOrN = Console.ReadKey().KeyChar;
                    if (yOrN == 'y')
                    {
                        cityStats.DisplayCitiesList();
                        Console.Write("\nPlease enter the name of the city from the above list: ");
                    }
                    else
                        Console.Write("\nPlease enter the name of the city: ");
                }
            } while (!cityStats.ProvinceIsValid(response));
            return response;
        }
        /// <summary>
        /// Province validator
        /// </summary>
        /// <param name="cityStats">The city stats object.</param>
        /// <returns>the user's validated response</returns>
        static string ProvinceValidator(Statistics cityStats)
        {
            string response;
            do
            {
                response = Console.ReadLine();
                if (!cityStats.ProvinceIsValid(response))
                {
                    Console.Write("\nInvalid response. Type 'y' to see a list of available provinces, or press enter to continue: ");
                    char yOrN = Console.ReadKey().KeyChar;
                    if (yOrN == 'y')
                    {
                        cityStats.DisplayProvinceList();
                        Console.Write("\nPlease enter the name of the province from the above list: ");
                    }
                    else
                        Console.Write("\nPlease enter the name of the province: ");
                }
            } while (!cityStats.ProvinceIsValid(response));
            return response;
        }
        /// <summary>
        /// Validates the names of two cities.
        /// </summary>
        /// <param name="cityStats">The city stats object.</param>
        /// <returns>the user's validated response</returns>
        static string[] TwoCityValidator(Statistics cityStats)
        {
            string[] cities;
            string response;
            do
            {
                response = Console.ReadLine();
                cities = response.Split(',', 2);
                if (!cityStats.CityIsValid(cities[0]) || !cityStats.CityIsValid(cities[1]))
                {
                    Console.Write("\nInvalid response. Type 'y' to see a list of available cities, or press enter to continue: ");
                    char yOrN = Console.ReadKey().KeyChar;
                    if (yOrN == 'y')
                    {
                        cityStats.DisplayCitiesList();
                        Console.Write("Please enter two cities, separated by ',' from the above list: ");
                    }
                    else
                        Console.Write("Please enter two cities, separated by ','(ex. 'London, Kingston'): ");
                }
            } while (!cityStats.CityIsValid(cities[0]) || (!cityStats.CityIsValid(cities[1])));
            return cities;
        }
        /// <summary>
        /// Validates the name of the city and that respecitve province.
        /// </summary>
        /// <param name="cityStats">The city stats object.</param>
        /// <returns>the user's validated response</returns>
        static string[] CityProvValidator(Statistics cityStats)
        {
            string[] cityProv;
            string response;
            do
            {
                response = Console.ReadLine();
                cityProv = response.Split(',', 2);
                if (!cityStats.CityIsValid(cityProv[0]) || !cityStats.ProvinceIsValid(cityProv[1]))
                {
                    Console.Write("\nInvalid response. Please enter a city and province separated by ','(ex: 'city, prov'): "); ;
                }
            } while (!cityStats.CityIsValid(cityProv[0]) || (!cityStats.ProvinceIsValid(cityProv[1])));
            return cityProv;

        }
    }
}
