using System;
using System.Linq;

namespace Project1_Group_17
{
    /// <summary>
    /// Project1_Group_17.Program
    /// Authors: Hunter Bennett, Connor Black, James Dunton
    /// Date: February 21, 2021
    /// </summary>
    class Program
    {

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">The args.</param>
        static void Main(string[] args)
        {
            Console.WriteLine("Canadian Cities Data Analayzer © Hunter Bennett, Connor Black, James Dunton");

            Statistics citiesStats = StartMenu();
            bool valid = false;
            citiesStats.DisplayProvinceList();
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
                        System.Environment.Exit(0);
                        break;
                    case "restart":
                        citiesStats = StartMenu();
                        break;
                    default:
                        Console.Write("\nInvalid selection, please key in a valid selection: ");
                        break;
                }
            } while (!valid);

            citiesStats.DisplayLargestPopulationCity("Ontario");
            citiesStats.DisplaySmallestPopulationCity("Ontario");
            citiesStats.DisplayProvincePopulation("Ontario");
            citiesStats.DisplayCityInfo("London");
            citiesStats.DisplayCityInfo("Windsor");
            citiesStats.DisplayProvinceCities("Ontario");

            //TODO: implement options
        }
        /// <summary>
        /// First menu that the user sees.
        /// </summary>
        /// <returns></returns>
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
                Console.WriteLine("-Key in 'exit' to exit the application at any menu in this program.");
                Console.WriteLine("-key in 'restart' at any menu to return to this menu.");
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
                        Console.Write("\nInvalid selection, please key in a valid selection: ");
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
            bool valid = false;
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
                        Console.Write("\nPlease key in the province name to see the population for that given province: ");
                        do
                        {
                            response = Console.ReadLine();
                            response = FormatAnswer(response);
                            if (!cityStats.ProvinceIsValid(response))
                            {
                                Console.Write("\nInvalid response. Please pick from one of the following provinces and key in the province name: ");
                                cityStats.DisplayProvinceList();
                                Console.Write("Please key in one of the provinces listed above: ");
                            }
                        } while (!cityStats.ProvinceIsValid(response));
                        cityStats.DisplayProvincePopulation(response);
                        Console.Write("\nDo you want to return to the Mode Selection Menu? (y/n):");
                        if (Console.ReadKey().KeyChar == 'y')
                            valid = true;
                        break;
                    case "2":
                        Console.Write("\nPlease key in the province name to see the cities located in that given province: ");
                        do
                        {
                            response = Console.ReadLine();
                            response = FormatAnswer(response);
                            if (!cityStats.ProvinceIsValid(response))
                            {
                                Console.Write("\nInvalid response. Please pick from one of the following cities and key in the city name: ");
                                cityStats.DisplayProvinceList();
                                Console.Write("Please key in one of the provinces listed above: ");
                            }
                        } while (!cityStats.ProvinceIsValid(response));
                        cityStats.DisplayProvinceCities(response);
                        Console.Write("\nDo you want to return to the Mode Selection Menu? (y/n):");
                        if (Console.ReadKey().KeyChar == 'y')
                            valid = true;
                        break;
                    case "3":
                        Console.WriteLine("Here's the rank breakdown of each province by population: ");

                        cityStats.RankProvincesByPopulation();
                        Console.Write("\nDo you want to return to the Mode Selection Menu? (y/n):");
                        if (Console.ReadKey().KeyChar == 'y')
                            valid = true;
                        break;
                    case "4":
                        Console.WriteLine("Here's the rank breakdown of each province by number of cities: ");

                        cityStats.RankProvincesByCities();
                        Console.Write("\nDo you want to return to the Mode Selection Menu? (y/n):");
                        if (Console.ReadKey().KeyChar == 'y')
                            valid = true;
                        break;
                    case "5":
                        Console.Write("\nPlease key in a province to view the capital city of that province: ");
                        do
                        {
                            response = Console.ReadLine();
                            response = FormatAnswer(response);
                            if (!cityStats.ProvinceIsValid(response))
                            {
                                Console.Write("\nInvalid response. Please pick from one of the following provinces and key in the province name: ");
                                cityStats.DisplayProvinceList();
                                Console.Write("Please key in one of the provinces listed above: ");
                            }
                        } while (!cityStats.ProvinceIsValid(response));
                        cityStats.GetCapital(response);
                        Console.Write("\nDo you want to return to the Mode Selection Menu? (y/n):");
                        if (Console.ReadKey().KeyChar == 'y')
                            valid = true;
                        break;
                    case "exit":
                        System.Environment.Exit(0);
                        break;
                    case "restart":
                        valid = true;
                        cityStats = StartMenu();
                        break;
                    default:
                        Console.Write("\nInvalid selection, please key in a valid selection: ");
                        break;
                }
            } while (!valid);
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
                string cityOne;
                string cityTwo;
                switch (selection.ToLower())

                {
                    case "1":
                        Console.Write("\nPlease key in a city's name to display the city's info: ");
                        do
                        {
                            response = Console.ReadLine();
                            response = FormatAnswer(response);
                            if (!cityStats.CityIsValid(response))
                            {
                                Console.Write("\nInvalid response. Please pick from one of the following cities and key in the city name: ");
                                cityStats.DisplayCitiesList();
                                Console.WriteLine("Please write your selection from the above list: ");
                            }
                        } while (!cityStats.CityIsValid(response));
                        cityStats.DisplayCityInfo(response);
                        Console.WriteLine("Do you want to return to the Mode Selection Menu? (y/n):");
                        if (Console.ReadKey().KeyChar == 'y')
                            valid = true;
                        break;
                    case "2":
                        Console.Write("\nPlease key in a province name to return the city with the largest population in that given province: ");
                        do
                        {
                            response = Console.ReadLine();
                            response = FormatAnswer(response);
                            if (!cityStats.ProvinceIsValid(response))
                            {
                                Console.Write("\nInvalid response. Please pick from one of the following cities and key in the city name: ");
                                cityStats.DisplayCitiesList();
                                Console.WriteLine("Please write your selection from the above list: ");
                            }
                        } while (!cityStats.ProvinceIsValid(response));
                        cityStats.DisplayLargestPopulationCity(response);
                        Console.Write("Do you want to return to the Mode Selection Menu? (y/n):");
                        if (Console.ReadKey().KeyChar == 'y')
                            valid = true;
                        break;
                    case "3":
                        Console.Write("\nPlease key in a province name to return the city with the smallest population in that given province: ");
                        do
                        {
                            response = Console.ReadLine();
                            response = FormatAnswer(response);
                            if (!cityStats.ProvinceIsValid(response))
                            {
                                Console.Write("\nInvalid response. Please pick from one of the following cities and key in the city name: ");
                                cityStats.DisplayCitiesList();
                                Console.WriteLine("Please write your selection from the above list: ");
                            }
                        } while (!cityStats.ProvinceIsValid(response));
                        cityStats.DisplaySmallestPopulationCity(response);
                        Console.Write("Do you want to return to the Mode Selection Menu? (y/n): ");
                        if (Console.ReadKey().KeyChar == 'y')
                            valid = true;
                        break;
                    case "4":
                        Console.Write("\nPlease key in two cities, separated by ',' to compare the population of those two cities: ");
                        do
                        {
                            response = Console.ReadLine();
                            string[] cities = response.Split(',', 2);
                            cityOne = cities[0];
                            cityTwo = cities[1];
                            cityOne = FormatAnswer(cityOne);
                            cityTwo = FormatAnswer(cityTwo);
                            if (!cityStats.CityIsValid(cityOne) || !cityStats.CityIsValid(cityTwo))
                            {
                                Console.Write("\nInvalid response. Please pick from two of the following cities and key in the city name: ");
                                cityStats.DisplayCitiesList();
                                Console.WriteLine("Please key in two cities, separated by ',' from the above list: ");
                            }

                        } while (!cityStats.CityIsValid(cityOne) || (!cityStats.CityIsValid(cityTwo)));

                        cityStats.CompareCitiesPopulation(cityOne, cityTwo);
                        Console.Write("Do you want to return to the Mode Selection Menu? (y/n): ");
                        if (Console.ReadKey().KeyChar == 'y')
                            valid = true;
                        break;
                    case "5":
                        Console.Write("\nPlease key in a city's name and a porvinc's name seperated by a comma to display the city on a map(ex. 'London, Ontario'): ");
                        string cityName;
                        string provName;
                        do
                        {
                            response = Console.ReadLine();
                            string[] cityProv = response.Split(',', 2);
                            cityName = cityProv[0];
                            provName = cityProv[1];
                            cityName = FormatAnswer(cityName);
                            provName = FormatAnswer(provName);
                            if (!cityStats.CityIsValid(cityName) || (!cityStats.ProvinceIsValid(provName)))
                            {
                                Console.Write("\nInvalid response. Please pick a city and province from the following list, formatted as 'City, Province': ");
                            }
                        } while (!cityStats.CityIsValid(cityName) || (!cityStats.ProvinceIsValid(provName)));
                        cityStats.ShowCityOnMap(cityName, provName);
                        Console.WriteLine("Do you want to return to the Mode Selection Menu? (y/n):");
                        if (Console.ReadKey().KeyChar == 'y')
                            valid = true;
                        break;
                    case "6":
                        Console.Write("\nPlease key in two cities, separated by ',' to compare the population of those two cities: ");
                        do
                        {
                            response = Console.ReadLine();
                            string[] cities = response.Split(',', 2);
                            cityOne = cities[0];
                            cityTwo = cities[1];
                            cityOne = FormatAnswer(cityOne);
                            cityTwo = FormatAnswer(cityTwo);
                            if (!cityStats.CityIsValid(cityOne) || !cityStats.CityIsValid(cityTwo))
                            {
                                Console.Write("\nInvalid response. Please pick from two of the following cities and key in the city name: ");
                                cityStats.DisplayCitiesList();
                                Console.WriteLine("Please key in two cities, separated by ',' from the above list: ");
                            }
                        } while (!cityStats.CityIsValid(cityOne) || (!cityStats.CityIsValid(cityTwo)));
                        cityStats.CalculateDistanceBetweenCities(cityOne, cityTwo);// how do I call await on this void method?
                        Console.Write("Do you want to return to the Mode Selection Menu? (y/n):");
                        if (Console.ReadKey().KeyChar == 'y')
                            valid = true;
                        break;
                    case "exit":
                        System.Environment.Exit(0);
                        break;
                    case "restart":
                        valid = true;
                        cityStats = StartMenu();
                        break;
                    default:
                        Console.Write("\nInvalid selection, please key in a valid selection: ");
                        break;
                }
            } while (!valid);
        }
        /// <summary>
        /// Formats response string from user.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static string FormatAnswer(string s)
        {
            s = String.Concat(s.Where(c => !Char.IsWhiteSpace(c)));
            return char.ToUpper(s[0]) + s.Substring(1).ToLower();
        }
    }
}
