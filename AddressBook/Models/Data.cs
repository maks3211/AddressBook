using System.Text.Json;
using System.Collections.Generic;
using System.IO;
namespace AddressBook.Models
{
    public class Data
    {
        private static List<City> Cities = new List<City>();
        private static List<Person> Persons = new List<Person>();
        private static List<Book> AddressBook = new List<Book>();

        /// <summary>
        /// Gets the list of cities.
        /// </summary>
        /// <returns>The list of cities.</returns>
        public static List<City> GetCityList()
        {
            return Cities;
        }

        /// <summary>
        /// Gets the list of persons.
        /// </summary>
        /// <returns>The list of persons.</returns>
        public static List<Person> GetPersonList()
        {
            return Persons;
        }

        /// <summary>
        /// Gets the list of books.
        /// </summary>
        /// <returns>The list of books.</returns>
        public static List<Book> GetBook() { 
            return AddressBook;
        }

        /// <summary>
        /// Saves the provided list to a JSON file.
        /// </summary>
        /// <typeparam name="T">The type of objects in the list.</typeparam>
        /// <param name="list">The list to save.</param>
        /// <param name="fileName">The name of the file to save to. (with no file extension)</param>
        public static void SaveToJson<T>(List<T>list, string fileName)
        {
            string jsonString = JsonSerializer.Serialize(list);
            File.WriteAllText(fileName+".json", jsonString);
        }

        /// <summary>
        /// Reads data from a JSON file and populates the corresponding list.
        /// </summary>
        /// <param name="objectName">The name of the object type to read ( "City", "Person", "Book").</param>
        /// <param name="fileName">The name of the JSON file to read from (with no file extension).</param>
        /// <returns>True if reading was successful, otherwise false.</returns>
        public static bool ReadFromJson(string objectName, string fileName)
        {
            string jsonFile = fileName + ".json";
            if (File.Exists(jsonFile))
            {
                Console.WriteLine("open");
                try {
                    string jsonString = File.ReadAllText(jsonFile);
                    switch (objectName)
                    {
                        case "Book":
                            AddressBook = JsonSerializer.Deserialize<List<Book>>(jsonString);
                            return true;
                        case "City":
                            Cities = JsonSerializer.Deserialize<List<City>>(jsonString);
                            return true;
                        case "Person":
                            Persons = JsonSerializer.Deserialize<List<Person>>(jsonString);
                            return true;
                        default:
                            return false;
                    } 
                }
                catch {
                    Console.WriteLine("exception read from json");
                }
            }
            Console.WriteLine("exception read from json");
            return false;
        }
    }
}
