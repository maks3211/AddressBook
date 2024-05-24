using System.ComponentModel.DataAnnotations;

namespace AddressBook.Models
{
    public class Person: IEntityOperations<Person>
    {
        private static int nextId = 0;

        public int Id { get; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public Person(string name, string lastName)
        {
            Id = nextId++;
            Name = name;
            LastName = lastName;
        }


        /// <summary>
        /// Removes a person from the list of cities based on the specified ID.
        /// </summary>
        /// <param name="id">Objet to remove</param>
        /// <param name="people">List of Objects to remove from</param>
        /// <returns>True if the city was successfully removed; otherwise, false.</returns>
        public static bool RemoveById(int id, List<Person> people)
        {
            var personToRemove = people.FirstOrDefault(p => p.Id == id);
            if (personToRemove != null)
            {
                people.Remove(personToRemove);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Retrieves the indexes of people in the list based on the specified name and property.
        /// </summary>
        /// <param name="name">The value of searched property</param>
        /// <param name="by">The property by which to search ( "name", "lastname")</param>
        /// <param name="people">The list of people to search</param>
        /// <returns>The list of indexes that match the specified name and property.</returns>
        public static List<int> GetIndexesOf(string name, string by, List<Person> people)
        {
            string lowerName = name.ToLower();
            by.ToLower();
            switch (by) {
                case "name":
                    return GetIndexesOf(people, person => person.Name.ToLower() == lowerName);
                case "lastname":
                    return GetIndexesOf(people, person => person.LastName.ToLower() == lowerName);
            }
            List<int> emptyList = new List<int>();
            return emptyList;

        }

        /// <summary>
        /// Gets the index of the person with the specified ID in the list of people.
        /// </summary>
        /// <param name="id">The ID of the person to find.</param>
        /// <param name="people">The list of people to search.</param>
        /// <returns>The index of the person in the list, or -1 if the person is not found.</returns>
        public static int GetIndex(int id, List<Person> people)
        {
            return people.FindIndex(person => person.Id == id);
        }


        /// <summary>
        /// Gets the indexes of people in the list that match the specified filter.
        /// </summary>
        /// <param name="people">The list of people to search.</param>
        /// <param name="filter">The filter to apply to the people.</param>
        /// <returns>The indexes of the people that match the filter.</returns>
        private static List<int> GetIndexesOf(List<Person> people, Func<Person, bool> filter)
        {
            List<int> indexesOf = people
                .Select((person, index) => new { Person = person, Index = index }) 
                .Where(pair => filter(pair.Person)) 
                .Select(pair => pair.Index) 
                .ToList();
            return indexesOf;
        }


    }
}
