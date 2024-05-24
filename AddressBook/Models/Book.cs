namespace AddressBook.Models
{

    public class BookDto
    {
        public int CityId { get; set; }
        public int PersonId { get; set; }
        public string ?Description { get; set; }
    }


    public class Book: IEntityOperations<Book>
    {
        private static int nextId = 0;  

        public int Id { get; }
        public int CityId { get; set; }
        public int PersonId { get; set; }
        public string Description { get; set; }

        public Book(int cityId, int personId, string description)
        {
            Id = nextId++;
            CityId = cityId;
            PersonId = personId;
            Description = description;
        }

        /// <summary>
        /// Creates new Book object, check if city and person exists
        /// </summary>
        /// <returns>Book if created, null if not</returns>
        public static Book? AddEntry(int cityId, int personId, string description, List<City> cityList, List<Person> peopleList)
        {
            if (City.GetIndex(cityId, cityList) != -1 && Person.GetIndex(personId, peopleList) != -1)
            {
                return new Book(cityId, personId, description);
            }
            return null;    
        }

        /// <summary>
        /// Removes a entity from the list of books based on the specified ID.
        /// </summary>
        /// <param name="id">Objet to remove</param>
        /// <param name="book">List of Objects to remove from</param>
        /// <returns>True if the entity was successfully removed, otherwise, false.</returns>
        public static bool RemoveById(int id, List<Book> book)
        {
            var entryToRemove = book.FirstOrDefault(e => e.Id == id);
            if (entryToRemove !=null)
            {
                book.Remove(entryToRemove);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the index of the entity with the specified ID in the list of books.
        /// </summary>
        /// <param name="id">The ID of the entity to find.</param>
        /// <param name="book">The list of cities to search.</param>
        /// <returns>The index of the entity in the list, or -1 if the entity is not found.</returns>
        public static int GetIndex(int id, List<Book> book)
        {
            return book.FindIndex(city => city.Id == id);
        }
        /// <summary>
        /// Retrieves the indexes of entities in the list based on the specified name and property.
        /// </summary>
        /// <param name="id">The Id of searched property</param>
        /// <param name="by">The property by which to search ( "cityid", "personid")</param>
        /// <param name="book">The list of entities to search</param>
        /// <returns>The list of indexes that match the specified name and property.</returns>
        public static List<int> GetIndexesOf(string id, string by, List<Book> book)
        {
            by.ToLower();
            try {
                int intId = Int32.Parse(id);
                switch (by)
                {
                    case "cityid":
                        return GetIndexesOf(book, entry => entry.CityId == intId);
                    case "personid":
                        return GetIndexesOf(book, entry => entry.PersonId == intId);
                }
                List<int> emptyList = new List<int>();
                return emptyList;
            }
            catch {
                List<int> emptyList = new List<int>();
                return emptyList;
            }
 
        }

        /// <summary>
        /// Gets the indexes of entities in the list that match the specified filter.
        /// </summary>
        /// <param name="book">The list of entities to search.</param>
        /// <param name="filter">The filter to apply to the entities.</param>
        /// <returns>The indexes of the entities that match the filter.</returns>
        private static List<int> GetIndexesOf(List<Book> book, Func<Book, bool> filter)
        {
            List<int> indexesOf = book
                .Select((book, index) => new { Book = book, Index = index })
                .Where(pair => filter(pair.Book))
                .Select(pair => pair.Index)
                .ToList();
            return indexesOf;

        }

        /// <summary>
        /// Resets the ID counter for cities- made for testing.
        /// </summary>
        public static void restId()
        {
            nextId = 0;
        }

    }
}
