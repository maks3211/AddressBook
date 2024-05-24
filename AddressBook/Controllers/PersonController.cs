using AddressBook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AddressBook.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private static List<Person> People = Data.GetPersonList();
        
        private readonly ILogger<PersonController> _logger;
        public PersonController(ILogger<PersonController> logger)
        {
            _logger = logger;
        }


        /// <summary>
        /// Adds a new person.
        /// </summary>
        /// <param name="person">The person object to add.</param>
        /// <returns>The IActionResult representing the operation result.</returns>
        [HttpPost("add")]
        public IActionResult AddPerson([FromBody] Person person)
        {          
            if (person == null)
            {
               _logger.LogError("FAILURE-Received HTTP POST request 'add' with invalid data: person == null");
               return BadRequest("Invalid data");
            }

            if (string.IsNullOrEmpty(person.Name) || string.IsNullOrEmpty(person.LastName))
            {
                _logger.LogError("FAILURE-Received HTTP POST request with invalid data: Missing required parameters");
                return BadRequest("Missing required parameters");
            }
             People.Add(person);
             _logger.LogInformation($"SUCCESS- Received HTTP POST request to add a new person- id: {person.Id} name:{person.Name} lastName:{person.LastName}");
            return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, person);
        }


        /// <summary>
        /// Removes a person by ID.
        /// </summary>
        /// <param name="id">The ID of the person to remove.</param>
        /// <returns>The IActionResult representing the operation result.</returns>
        [HttpPost("remove")]
        public IActionResult RemovePerson(int id)
        {

            if (Person.RemoveById(id, People))
            {
                _logger.LogInformation("SUCCESS- Received HTTP POST request to delete a person");
                return Ok();
            }
            _logger.LogError("FAILURE- Received HTTP POST request to delete a person- Person not found");
            return NotFound();

        }

        /// <summary>
        /// Edits a specific property of a person.
        /// </summary>
        /// <param name="id">The ID of the person to edit.</param>
        /// <param name="field">The field to edit (name, lastName).</param>
        /// <param name="newVal">The new value for the field.</param>
        /// <returns>The IActionResult representing the operation result.</returns>
        [HttpPost("edit/{id}/{field}={newVal}")]
        public IActionResult EditPerson(int id,string field, string newVal)
        {
            field = field.ToLower();
            int index = Person.GetIndex(id, People);
            if (index == -1)
            {

                _logger.LogError("FAILURE- Received HTTP POST request to edit a person- Id not found");
                return NotFound();
            }
            if (field == "name")
            {
                People[index].Name = newVal;
                _logger.LogInformation("SUCCESS- Received HTTP POST request to edit Person name");
                return Ok();
            }
            if (field == "lastName")
            {
                People[index].LastName = newVal;
                _logger.LogInformation("SUCCESS- Received HTTP POST request to edit Person lastName");
                return Ok();
            }
            _logger.LogError($"FAILURE- Received HTTP POST request to edit a person- {field} property not found");
            return NotFound();
        }

        /// <summary>
        /// Retrieves all people.
        /// </summary>
        /// <returns>The IEnumerable of all people.</returns>
        [HttpGet("showAll")]
        public IEnumerable<Person> GetAllPeople()
        {
            _logger.LogInformation("SUCCESS- Received HTTP GET request to show all people");
            return People;
        }

        /// <summary>
        /// Retrieves a person by ID.
        /// </summary>
        /// <param name="id">The ID of the person to retrieve.</param>
        /// <returns>The IActionResult representing the operation result.</returns>
        [HttpGet("show/{id}")]
        public IActionResult GetPerson(int id)
        {
            int index = Person.GetIndex(id, People);
            if (index != -1)
            {
                _logger.LogInformation("SUCCESS- Received HTTP GET request to get a person by id");
                return Ok(People[index]);
            }
            _logger.LogError("FAILURE- Received HTTP GET request to get a person by id- Person not found");
            return NotFound();    
        }

        /// <summary>
        /// Retrieves all people filtered by a specific property.
        /// </summary>
        /// <param name="by">The property to filter by (name, lastname).</param>
        /// <param name="val">The value to filter by.</param>
        /// <returns>The IActionResult representing the operation result.</returns>
        [HttpGet("show/{by}={val}")]
        public IActionResult GetAllBy(string by,string val)
        {
            var indexes = Person.GetIndexesOf(val, by, People);
            if (indexes.Count == 0)
            {
                _logger.LogError($"FAILURE- Received HTTP GET request to get a list of people by {by} property - no match found");
                return NotFound(); 
            }
            List<Person> found = new List<Person>();
            foreach (var i in indexes)
            {
                if (i >= 0 && i < People.Count)
                {
                    found.Add(People[i]);
                }
            }
            _logger.LogInformation($"SUCCESS- Received HTTP GET request to get a list of people by {by} property");
            return Ok(found); 
        }
    }
}
