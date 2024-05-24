namespace AddressBook.Models
{
    public class City: IEntityOperations<City>
    {
        private static int nextId = 0;
        public int Id { get;}
        public string Name { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }


        public City(string name, string zip, string country) {
            Id = nextId++;
            Name = name;
            Zip = zip;
            Country = country;
        }

        /// <summary>
        /// Removes a city from the list of cities based on the specified ID.
        /// </summary>
        /// <param name="id">Objet to remove</param>
        /// <param name="cities">List of Objects to remove from</param>
        /// <returns>True if the city was successfully removed; otherwise, false.</returns>
        public static bool RemoveById(int id, List<City> cities)
        {
            var cityToRemove = cities.FirstOrDefault(c => c.Id == id);
            if (cityToRemove != null)
            {
                cities.Remove(cityToRemove);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Retrieves the indexes of cities in the list based on the specified name and property.
        /// </summary>
        /// <param name="name">The value of searched property</param>
        /// <param name="by">The property by which to search ( "name", "zip", "country")</param>
        /// <param name="cities">The list of cities to search</param>
        /// <returns>The list of indexes that match the specified name and property.</returns>
        public static List<int> GetIndexesOf(string name, string by, List<City> cities)
        {
            string lowerName= name.ToLower();
            by.ToLower();
            switch (by)
            {
                case "name":
                    return GetIndexesOf(cities, city => city.Name.ToLower() == lowerName);
                case "zip":
                    return GetIndexesOf(cities, city => city.Zip.ToLower() == lowerName);
                case "country":
                    return GetIndexesOf(cities, city => city.Country.ToLower() == lowerName);
            }
            List<int> emptyList = new List<int>();
            return emptyList;
        }

        /// <summary>
        /// Gets the index of the city with the specified ID in the list of cities.
        /// </summary>
        /// <param name="id">The ID of the city to find.</param>
        /// <param name="cities">The list of cities to search.</param>
        /// <returns>The index of the city in the list, or -1 if the city is not found.</returns>
        public static int GetIndex(int id, List<City> cities)
        {
            return cities.FindIndex(city => city.Id == id);
        }

        /// <summary>
        /// Gets the indexes of cities in the list that match the specified filter.
        /// </summary>
        /// <param name="cities">The list of cities to search.</param>
        /// <param name="filter">The filter to apply to the cities.</param>
        /// <returns>The indexes of the cities that match the filter.</returns>
        private static List<int> GetIndexesOf(List<City> cities, Func<City, bool> filter)
        {
            List<int> indexesOf = cities
                .Select((city, index) => new { City = city, Index = index })
                .Where(pair => filter(pair.City))
                .Select(pair => pair.Index)
                .ToList();
            return indexesOf;

        }
        /// <summary>
        /// Resets the ID counter for cities- made for testing.
        /// </summary>
        public static void ResetId()
        {
            nextId = 0;
        }

    }
}
