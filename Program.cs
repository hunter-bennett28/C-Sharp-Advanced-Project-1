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
            string titleText = "Canadian Cities Data Analayzer Main Menu";
            string selection;
            string dash = new string('-', titleText.Length);
            Console.WriteLine($"{dash}\n{titleText}\n{dash}\n");
            Console.WriteLine("Available data files:\n");
            Console.WriteLine("\t1) Canadacities-JSON.json");
            Console.WriteLine("\t2) Canadacities-XML.xml");
            Console.WriteLine("\t3) Canadacities.csv");
            Console.Write("\nPlease select a data file to get city information from (ex. 1, 2): ");
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
                    Console.WriteLine("Available selections:\n");
                    Console.WriteLine("\t1) Display city statistics menu.");
                    Console.WriteLine("\t2) Display province statistics menu.");
                    Console.WriteLine("\texit) Exit the program");
                    Console.Write("\nPlease make a selection (ex. 1, exit): ");
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
                    Console.WriteLine("Available selections:\n");
                    Console.WriteLine("\t1) Display the population for a given province.");
                    Console.WriteLine("\t2) Display all the cities in a given province.");
                    Console.WriteLine("\t3) Rank provinces by population.");
                    Console.WriteLine("\t4) Rank provinces by the number of cities.");
                    Console.WriteLine("\t5) Display the capital city for the given province.");
                    Console.WriteLine("\treturn) Return to the main menu");
                    Console.Write("\nPlease make a selection (ex. 1, return): ");
                }
                displayMenu = true;

                selection = Console.ReadLine();
                string response;
                switch (selection.ToLower())
                {
                    case "1":
                        response = ProvinceValidator(cityStats);
                        cityStats.DisplayProvincePopulation(response);
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case "2":
                        response = ProvinceValidator(cityStats);
                        cityStats.DisplayProvinceCities(response);
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case "3":
                        Console.WriteLine("Here's the rank breakdown of each province by population:");
                        cityStats.RankProvincesByPopulation();
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case "4":
                        Console.WriteLine("Here's the rank breakdown of each province by number of cities:");
                        cityStats.RankProvincesByCities();
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case "5":
                        Console.WriteLine("\nWhich province do you wish to find the capital of?");
                        response = ProvinceValidator(cityStats);
                        cityStats.GetCapital(response);
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case "return":
                        return;

                    default:
                        Console.Write("\nInvalid selection, please enter a valid selection.");
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
                    Console.WriteLine($"{dash}\n{titleText}\n{dash}\n");
                    Console.WriteLine("Available selections:\n");
                    Console.WriteLine("\t1) Display city's info.");
                    Console.WriteLine("\t2) Display the city with the largest population within a given province.");
                    Console.WriteLine("\t3) Display the city with the smallest population within a given province.");
                    Console.WriteLine("\t4) Compare the population of two cities.");
                    Console.WriteLine("\t5) Display the city on a map.");
                    Console.WriteLine("\t6) Calculate the distance between two cities.");
                    Console.WriteLine("\t7) Update the population of a specific city.");
                    Console.WriteLine("\treturn) Return to the main menu");
                    Console.Write("\nPlease make a selection (ex. 1, return): ");
                }
                selection = Console.ReadLine();
                string response;
                switch (selection.ToLower())

                {
                    case "1":
                        response = OneCityValidator(cityStats);
                        cityStats.DisplayCityInformation(response);
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case "2":
                        Console.WriteLine("\nWhich province do you wish to see the largest population city of?");
                        response = ProvinceValidator(cityStats);
                        cityStats.DisplayLargestPopulationCity(response);
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case "3":
                        Console.WriteLine("\nWhich province do you wish to see the smallest population city of?");
                        response = ProvinceValidator(cityStats);
                        cityStats.DisplaySmallestPopulationCity(response);
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case "4":
                        Console.WriteLine("\nWhich city do you want to pick first?\n");
                        CityInfo city1 = GetCityChoice(cityStats);
                        Console.WriteLine("\nWhich city do you want to compare it to?\n");
                        CityInfo city2 = GetCityChoice(cityStats);
                        cityStats.CompareCitiesPopulation(city1, city2);
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case "5":
                        Console.WriteLine("\nWhich city do you want to see on the map?\n");
                        CityInfo city = GetCityChoice(cityStats);
                        cityStats.ShowCityOnMap($"{city.GetCityName().ToLower()}|{city.GetProvince().ToLower()}");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case "6":
                        Console.WriteLine("\nWhich city do you want to start at?\n");
                        CityInfo startingCity = GetCityChoice(cityStats);
                        Console.WriteLine("\nWhich city do you want to calculate the distance to?\n");
                        CityInfo endingCity = GetCityChoice(cityStats);
                        cityStats.CalculateDistanceBetweenCities(startingCity.GetCityName(), endingCity.GetCityName()).Wait();
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case "7":
                        UpdatePopulation(cityStats);
                        Console.WriteLine("Press any key to continue");
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

        static void UpdatePopulation(Statistics cityStats){
            bool valid = false;
            string fileName = "";
            ulong newPopulation = 0;
            Console.WriteLine("Available data files:\n");
            Console.WriteLine("\t1) Canadacities-JSON.json");
            Console.WriteLine("\t2) Canadacities-XML.xml");
            Console.WriteLine("\t3) Canadacities.csv");
            Console.Write("Please make a selection(ex. 1, 2): ");
            string selection;
            do
            {
                selection = Console.ReadLine();
                switch (selection)
                {
                    case "1":
                        fileName = "Canadacities-JSON.json";
                        valid = true;
                        break;
                     case "2":
                        fileName = "Canadacities-XML.xml";
                                    valid = true;
                                    break;
                                case "3":
                                    fileName = "Canadacities.csv";
                                    valid = true;
                                    break;
                                default:
                                    Console.Write("\nInvalid selection, please enter a valid selection: ");
                                    break;
                            }
                        } while (!valid);
                        Console.WriteLine("\nWhich city do you want to change the population?\n");
                        //get the user's choice
                        response = OneCityValidator(cityStats);
                        do
                        {
                            Console.Write($"Please enter the new population for {response}: ");
                            if (!UInt64.TryParse(Console.ReadLine(), out newPopulation))
                            continue;
                            else break;
                        }
                        while (true);
                        try
                        {
                            cityStats.UpdatePopulation(response, newPopulation, fileName);
            }catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
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
                Console.Write("\nEnter city name or type 'list' to see available cities: ");
                response = Console.ReadLine().Trim();
                if (response.ToLower() == "list")
                {
                    Console.WriteLine("\nWhich province do you wish to see the cities in?");
                    response = ProvinceValidator(cityStats);
                    cityStats.DisplayProvinceCities(response);
                    continue;
                }
                if (!cityStats.IsValidCity(response))
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
                    Console.Write($"\nNo province exists called {response}.\n");
                }
                else
                {
                    return response;
                }
            } while (true);
        }

        /// <summary>
        /// Validates the name of the city and that respecitve province.
        /// </summary>
        /// <param name="cityStats">The city stats object.</param>
        /// <returns>the user's validated response</returns>
        static CityInfo GetCityChoice(Statistics cityStats)
        {
            CityInfo city = null;
            string response = "";
            do
            {
                Console.Write("Please enter the city name or 'list' to see all available options: ");
                response = Console.ReadLine().Trim().ToLower();
                if (response == "list")
                {
                    Console.WriteLine("Which province do you wish to see the cities of?");
                    response = ProvinceValidator(cityStats);
                    cityStats.DisplayProvinceCities(response);
                }

                city = cityStats.GetSpecificCity(response);
            } while (city == null);

            return city;
        }
    }
}
