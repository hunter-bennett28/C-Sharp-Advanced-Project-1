/// Program.cs
/// Authors: James Dunton, Hunter Bennett, Connor Black
/// Desc: A program that allows city data to be loaded through various file types
///         and displays various statistics on the data provided.

using System;
using System.Text;

namespace Project1_Group_17
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Canadian Cities Data Analayzer © Hunter Bennett, Connor Black, James Dunton");

            Statistics citiesStats = LoadMenu();
            MainMenu(citiesStats);
        }

        /// <summary>
        /// First menu that the user sees.
        /// </summary>
        /// <returns>Statitics object</returns>
        static Statistics LoadMenu()
        {
            Statistics citiesStats = null;
            bool valid = false;
            string titleText = "Main Menu";
            string selection;
            string dash = new string('-', titleText.Length);
            Console.WriteLine($"\n{dash}\n{titleText}\n{dash}\n");
            Console.WriteLine("\nAvailable data files:");
            Console.WriteLine("\t1) Canadacities-JSON.json");
            Console.WriteLine("\t2) Canadacities-XML.xml");
            Console.WriteLine("\t3) Canadacities.csv");
            Console.Write("Please make a selection(ex. 1, 2): ");
            do
            {
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
                    default:
                        Console.Write("\nInvalid selection, please enter a valid selection: ");
                        break;
                }
            } while (!valid);
            return citiesStats;
        }

        /// <summary>
        /// Display a main menu to allow users to go to a province menu or city menu
        /// </summary>
        /// <param name="cityStats"></param>
        static void MainMenu(Statistics cityStats)
        {
            string selection;
            string titleText = "Mode Selection Menu";
            string dash = new string('-', titleText.Length);

            bool displayMenu = true;
            do
            {
                if (displayMenu)
                {
                    Console.Clear();
                    Console.WriteLine($"{dash}\n{titleText}\n{dash}\n");
                    Console.WriteLine("\nAvailable selections:");
                    Console.WriteLine("\t1) Display city statistics menu.");
                    Console.WriteLine("\t2) Display province statistics menu.");
                    Console.WriteLine("\texit - Exit the program");
                    Console.Write("Please make a selection: ");
                }
                displayMenu = true;
                selection = Console.ReadLine();
                switch (selection.ToLower())
                {
                    case "1":
                        CityMenu(cityStats);
                        break;
                    case "2":
                        ProvinceMenu(cityStats);
                        break;
                    case "exit":
                        return;
                    default:
                        Console.Write("\nInvalid selection, please enter a valid selection: ");
                        displayMenu = false;
                        break;
                }
            } while (true);

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

            bool displayMenu = true;
            do
            {
                if (displayMenu)
                {
                    Console.Clear();
                    Console.WriteLine($"\n{dash}\n{titleText}\n{dash}\n");
                    Console.WriteLine("\nAvailable selections:");
                    Console.WriteLine("\t1) Display the population for a given province.");
                    Console.WriteLine("\t2) Display all the cities in a given province.");
                    Console.WriteLine("\t3) Rank provinces by population.");
                    Console.WriteLine("\t4) Rank provinces by the number of cities.");
                    Console.WriteLine("\t5) Display the capital city for the given province.");
                    Console.WriteLine("\treturn - Return to the main menu");
                    Console.Write("Please make a selection(ex. 1, 2): ");
                }
                displayMenu = true;

                selection = Console.ReadLine();
                string response;
                switch (selection.ToLower())
                {
                    case "1":
                        response = ProvinceValidator(cityStats);
                        cityStats.DisplayProvincePopulation(response);
                        Console.WriteLine("Press a key to continue");
                        Console.ReadKey();
                        break;
                    case "2":
                        response = ProvinceValidator(cityStats);
                        cityStats.DisplayProvinceCities(response);
                        Console.WriteLine("Press a key to continue");
                        Console.ReadKey();
                        break;
                    case "3":
                        Console.WriteLine("Here's the rank breakdown of each province by population: ");
                        cityStats.RankProvincesByPopulation();
                        Console.WriteLine("Press a key to continue");
                        Console.ReadKey();
                        break;
                    case "4":
                        Console.WriteLine("Here's the rank breakdown of each province by number of cities: ");
                        cityStats.RankProvincesByCities();
                        Console.WriteLine("Press a key to continue");
                        Console.ReadKey();
                        break;
                    case "5":
                        Console.Write("\nPlease enter a province to view the capital city of that province: ");
                        response = ProvinceValidator(cityStats);
                        cityStats.GetCapital(response);
                        Console.WriteLine("Press a key to continue");
                        Console.ReadKey();
                        break;
                    case "return":
                        return;

                    default:
                        Console.Write("\nInvalid selection, please enter a valid selection: ");
                        displayMenu = false;
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

            string selection;
            string titleText = "Cities Menu";
            bool displayMenu = true;


            do
            {
                if (displayMenu)
                {
                    Console.Clear();
                    string dash = new string('-', titleText.Length);
                    Console.WriteLine($"\n{dash}\n{titleText}\n{dash}\n");
                    Console.WriteLine("\nAvailable selections:");
                    Console.WriteLine("\t1) Display city's info.");
                    Console.WriteLine("\t2) Display the city with the largest population within a given province.");
                    Console.WriteLine("\t3) Display the city with the smallest population within a given province.");
                    Console.WriteLine("\t4) Compare the population of two cities.");
                    Console.WriteLine("\t5) Display the city on a map.");
                    Console.WriteLine("\t6) Calculate the distance between two cities.");
                    Console.WriteLine("\treturn - Return to the main menu");
                    Console.Write("Please make a selection(ex. 1, 2): ");
                }
                selection = Console.ReadLine();
                string response;
                string[] resp;
                switch (selection.ToLower())

                {
                    case "1":
                        Console.Write("\nPlease enter a city's name to display the city's info: ");
                        response = OneCityValidator(cityStats);
                        cityStats.DisplayCityInformation(response);
                        Console.WriteLine("Press a key to continue");
                        Console.ReadKey();
                        break;
                    case "2":
                        Console.Write("\nPlease enter a province name to return the city with the largest population in that given province: ");
                        response = ProvinceValidator(cityStats);
                        cityStats.DisplayLargestPopulationCity(response);
                        Console.WriteLine("Press a key to continue");
                        Console.ReadKey();
                        break;
                    case "3":
                        Console.Write("\nPlease enter a province name to return the city with the smallest population in that given province: ");
                        response = ProvinceValidator(cityStats);
                        cityStats.DisplaySmallestPopulationCity(response);
                        Console.WriteLine("Press a key to continue");
                        Console.ReadKey();
                        break;
                    case "4":
                        Console.Write("\nPlease enter two cities, separated by ',' to compare the population of those two cities: ");
                        resp = TwoCityValidator(cityStats);
                        cityStats.CompareCitiesPopulation(cityStats.GetSpecificCity(resp[0]), cityStats.GetSpecificCity(resp[1]));
                        Console.WriteLine("Press a key to continue");
                        Console.ReadKey();
                        break;
                    case "5":
                        Console.Write("\nPlease enter a city's name and a porvince's name seperated by a comma to display the city on a map(ex. 'London, Ontario'): ");
                        resp = CityProvValidator(cityStats);
                        cityStats.ShowCityOnMap($"{resp[0]}|{resp[1]}");
                        Console.WriteLine("Press a key to continue");
                        Console.ReadKey();
                        break;
                    case "6":
                        Console.Write("\nPlease enter two cities, separated by ',' to compare the population of those two cities: ");
                        resp = TwoCityValidator(cityStats);
                        cityStats.CalculateDistanceBetweenCities(resp[0], resp[1]).Wait();
                        Console.WriteLine("Press a key to continue");
                        Console.ReadKey();
                        break;
                    case "return":
                        return;

                    default:
                        Console.Write("\nInvalid selection, please enter a valid selection: ");
                        displayMenu = false;
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
                Console.Write("\nEnter city name or type 'list' to display all cities: ");
                response = Console.ReadLine().Trim();
                if (response.ToLower() == "list")
                {
                    Console.Write("\n Enter the province name to see the cities located in that given province: ");
                    response = ProvinceValidator(cityStats);
                    cityStats.DisplayProvinceCities(response);
                    continue;
                }
                if (cityStats.GetSpecificCity(response) == null)
                {
                    Console.WriteLine($"\nNo city exists called {response}.");
                }
                else
                {
                    return response;
                }
            } while (true);

        }

        /// <summary>
        /// Province validator
        /// </summary>
        /// <param name="cityStats">The city stats object.</param>
        /// <returns>the user's validated response</returns>
        static string ProvinceValidator(Statistics cityStats)
        {
            do
            {
                Console.Write("\nEnter province name or type 'list' to display all provinces: ");
                string response = Console.ReadLine().Trim();
                if (response.ToLower() == "list")
                {
                    cityStats.DisplayProvinceList();
                    continue;
                }

                if (!cityStats.IsValidProvince(response))
                {
                    Console.Write($"\nNo province exists called {response}.");
                }
                else
                {
                    return response;
                }
            } while (true);
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
            StringBuilder str = new StringBuilder("");
            do
            {
                Console.Write("\nEnter two cities split by a comma (ex: London, Victoria), or type 'list' to see a list of cities: ");
                response = Console.ReadLine().Trim();
                if (response.ToLower() == "list")
                {
                    Console.Write("\nPlease enter the province name to see the cities located in that given province: ");
                    response = ProvinceValidator(cityStats);
                    cityStats.DisplayProvinceCities(response);
                    continue;
                }
                cities = response.Split(',', 2);
                for(int i = 0; i < cities.Length; i++)
                {
                    cities[i] = cities[i].Trim();
                }
                if (cities.Length != 2)
                {
                    Console.WriteLine($"\nInvalid number of arguements. ");
                }

                else if (cityStats.GetSpecificCity(cities[0]) == null || cityStats.GetSpecificCity(cities[1])==null)
                {
                    Console.WriteLine($"\nInvalid cities given. ");
                    continue;
                }
                else
                    return cities;
            } while (true);

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
                Console.Write("Please enter a city and province separated by ','(ex: 'city, prov'): ");
                response = Console.ReadLine().Trim();
                if (response.ToLower() == "list")
                {
                    Console.Write("\nPlease enter the province name to see the cities located in that given province: ");
                    response = ProvinceValidator(cityStats);
                    cityStats.DisplayProvinceCities(response);
                    continue;
                }
                cityProv = response.Split(',', 2);
                for (int i = 0; i < cityProv.Length; i ++)
                {
                    cityProv[i] = cityProv[i].Trim();
                }   
                
                if (!cityStats.IsValidCity(cityProv[0]) || !cityStats.IsValidProvince(cityProv[1]))
                {
                    Console.WriteLine("\nInvalid arguements given.");
                }
                else if( cityProv.Length != 2)
                    Console.WriteLine("\nInvalid number of arguements.");

                else
                {
                    return cityProv;
                }
            } while (true);
        }
    }
}
