using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AddressBook.Models;
using Microsoft.AspNetCore.Mvc.Diagnostics;
namespace AddressBook.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private static List<Book> AddressBook = Data.GetBook();
        private readonly ILogger<BookController> _logger;
        public BookController(ILogger<BookController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Adds a new entry to the address book.
        /// </summary>
        /// <param name="bookDto">The data transfer object for the new entry</param>
        /// <returns>The IActionResult representing the operation result.</returns>
        [HttpPost("add")]
        public IActionResult AddEntry([FromBody] BookDto bookDto)
        {
            if (bookDto == null)
            {
                _logger.LogError("FAILURE- Invalid data");
                return BadRequest("Invalid data");
            }

            var newEntry = Book.AddEntry(bookDto.CityId, bookDto.PersonId, bookDto.Description, Data.GetCityList(), Data.GetPersonList());
            if (newEntry != null)
            {
                AddressBook.Add(newEntry);
                return Ok();
            }
            _logger.LogError("Received HTTP POST request 'add' with invalid data: person == null or city == null");
            return BadRequest("Invalid cityId or personId");
        }

        /// <summary>
        /// Removes an entry by ID.
        /// </summary>
        /// <param name="id">The ID of the entry to remove.</param>
        /// <returns>The IActionResult representing the operation result.</returns>
        [HttpPost("remove")]
        public IActionResult RemovePerson(int id)
        {

            if (Book.RemoveById(id, AddressBook))
            {
                _logger.LogInformation("SUCCESS- Received HTTP POST request to delete an address in book");
                return Ok();
            }
            _logger.LogError("FAILURE- Received HTTP POST request to delete an entity in book- Address not found");
            return NotFound();

        }

        /// <summary>
        /// Edits a specific property of a entry in address book.
        /// </summary>
        /// <param name="id">The ID of the entry to edit.</param>
        /// <param name="field">The field to edit (personid, cityid, description).</param>
        /// <param name="newVal">The new value for the field.</param>
        /// <returns>The IActionResult representing the operation result.</returns>
        [HttpPost("edit/{id}/{field}={newVal}")]
        public IActionResult EditPerson(int id, string field, string newVal)
        {
            field = field.ToLower();
            int newIntVal = 0;
            if (field != "description")
            {
                try
                {
                    newIntVal = Int32.Parse(newVal);
                }
                catch {
                    _logger.LogError("FAILURE- Received HTTP POST request to edit an address - newVal is not a number");
                    return BadRequest("Invalid ID format");
                }
            }
            
            int index = Book.GetIndex(id, AddressBook);
            if (index == -1)
            {
                _logger.LogError("FAILURE- Received HTTP POST request to edit an address - Id not found");
                return NotFound();
            }
            if (field == "personid")
            {
                AddressBook[index].PersonId = newIntVal;
                _logger.LogInformation("SUCCESS- Received HTTP POST request to edit Person ID in address");
                return Ok();
            }
            if (field == "cityid")
            {
                AddressBook[index].CityId = newIntVal;
                _logger.LogInformation("SUCCESS- Received HTTP POST request to edit City ID in address");
                return Ok();
            }
            if (field == "description")
            {
                AddressBook[index].Description = newVal;
                _logger.LogInformation("SUCCESS- Received HTTP POST request to edit Description in address");
                return Ok();
            }

            _logger.LogError($"FAILURE- Received HTTP POST request to edit an address- {field} property not found");
            return NotFound();
        }

        /// <summary>
        /// Retrieves all entires.
        /// </summary>
        /// <returns>The IEnumerable of all entires.</returns>
        [HttpGet("showAll")]
        public IEnumerable<Book> GetAllPeople()
        {
            _logger.LogInformation("SUCCESS- Received HTTP GET request to show all adresses");
            return AddressBook;
        }

        /// <summary>
        /// Retrieves an entry from the address book by ID.
        /// </summary>
        /// <param name="id">The ID of the entry to retrieve.</param>
        /// <returns>The IActionResult representing the operation result.</returns>
        [HttpGet("show/{id}")]
        public IActionResult GetEntry(int id)
        {
            int index = Book.GetIndex(id, AddressBook);
            if (index != -1)
            {
                _logger.LogInformation("SUCCESS- Received HTTP GET request to get a addres by id");
                return Ok(AddressBook[index]);
            }
            _logger.LogError("FAILURE- Received HTTP GET request to get an addres by id- Person not found");
            return NotFound();
        }

        /// <summary>
        /// Retrieves the last added entry from the address book.
        /// </summary>
        /// <returns>The IActionResult representing the operation result.</returns>
        [HttpGet("getLast")]
        public IActionResult GetAllLast()
        {
            _logger.LogInformation("SUCCESS- Received HTTP GET request to get an last added adress");
            return Ok(AddressBook.LastOrDefault());
        }


        /// <summary>
        /// Retrieves all entries from the address book filtered by a specific property.
        /// </summary>
        /// <param name="by">The property to filter by (cityid, personid).</param>
        /// <param name="val">The value to filter by.</param>
        /// <returns>The IActionResult representing the operation result.</returns>
        [HttpGet("show/{by}={val}")]
        public IActionResult GetAllBy(string by, string val)
        {
            var indexes = Book.GetIndexesOf(val, by, AddressBook);
            if (indexes.Count == 0)
            {
                _logger.LogError($"FAILURE- Received HTTP GET request to get a list of addresses by {by} property - no match found");
                return NotFound(); 
            }
            List<Book> found = new List<Book>();
            foreach (var i in indexes)
            {
                if (i >= 0 && i < AddressBook.Count)
                {
                    found.Add(AddressBook[i]);
                }
            }
            _logger.LogInformation($"SUCCESS- Received HTTP GET request to get a list of addresses by {by} property");
            return Ok(found); 
        }
    }
}
