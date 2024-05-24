using AddressBook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AddressBook.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private static List<City> Cities = Data.GetCityList();
        private readonly ILogger<CityController> _logger;
        public CityController(ILogger<CityController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Adds a new city.
        /// </summary>
        /// <param name="city">The city object to add.</param>
        /// <returns>The IActionResult representing the operation result.</returns>
        [HttpPost("add")]
        public IActionResult AddCity([FromBody] City city)
        {
            if (city == null)
            {
                _logger.LogError("FAILURE-Received HTTP POST request 'add' with invalid data: city == null");
                return BadRequest("Invalid data");
            }
            Cities.Add(city);
            _logger.LogInformation($"SUCCESS- Received HTTP POST request to add a new city- id: {city.Id} name:{city.Name} zip:{city.Zip} country:{city.Country}");
            return CreatedAtAction(nameof(GetCity), new { id = city.Id }, city);
            
        }

        /// <summary>
        /// Removes a city by ID.
        /// </summary>
        /// <param name="id">The ID of the city to remove.</param>
        /// <returns>The IActionResult representing the operation result.</returns>
        [HttpPost("remove{id}")]
        public IActionResult RemoveCity(int id)
        {

            if (City.RemoveById(id, Cities))
            {
                _logger.LogInformation("SUCCESS- Received HTTP POST request to delete a city");
                return Ok();
            }
            _logger.LogError("FAILURE- Received HTTP POST request to delete a city- City not found");
            return NotFound();

        }

        /// <summary>
        /// Edits a specific property of a city.
        /// </summary>
        /// <param name="id">The ID of the city to edit.</param>
        /// <param name="field">The field to edit (name, zip, country).</param>
        /// <param name="newVal">The new value for the field.</param>
        /// <returns>The IActionResult representing the operation result.</returns>
        [HttpPost("edit/{id}/{field}={newVal}")]
        public IActionResult EditPerson(int id, string field, string newVal)
        {
            field = field.ToLower();
            int index = City.GetIndex(id, Cities);
            if (index == -1)
            {
                _logger.LogError("FAILURE- Received HTTP POST request to edit a city- Id not found");
                return NotFound();
            }
            if (field == "name")
            {
                Cities[index].Name = newVal;
                _logger.LogInformation("SUCCESS- Received HTTP POST request to edit city name");
                return Ok();
            }
            if (field == "zip")
            {
                Cities[index].Zip = newVal;
                _logger.LogInformation("SUCCESS- Received HTTP POST request to edit city zip");
                return Ok();
            }
            if (field == "country")
            {
                Cities[index].Country = newVal;
                _logger.LogInformation("SUCCESS- Received HTTP POST request to edit City country");
                return Ok();
            }

            _logger.LogError($"FAILURE- Received HTTP POST request to edit a city- {field} property not found");
            return NotFound();

        }

        /// <summary>
        /// Retrieves all cities.
        /// </summary>
        /// <returns>The IEnumerable of all people.</returns>
        [HttpGet("showAll")]
        public IEnumerable<City> GetAllCities()
        {
            _logger.LogInformation("SUCCESS- Received HTTP GET request to show all cities");
            return Cities;
        }

        /// <summary>
        /// Retrieves a city by ID.
        /// </summary>
        /// <param name="id">The ID of the city to retrieve.</param>
        /// <returns>The IActionResult representing the operation result.</returns>
        [HttpGet("show/{id}")]
        public IActionResult GetCity(int id)
        {
            int index = City.GetIndex(id, Cities);
            if (index != -1)
            {
                _logger.LogInformation("SUCCESS- Received HTTP GET request to get a city by id");
                return Ok(Cities[index]);
            }
            _logger.LogError("FAILURE- Received HTTP GET request to get a city by id- Person not found");
            return NotFound();
        }

        /// <summary>
        /// Retrieves all cities filtered by a specific property.
        /// </summary>
        /// <param name="by">The property to filter by (name, zip, country).</param>
        /// <param name="val">The value to filter by.</param>
        /// <returns>The IActionResult representing the operation result.</returns>
        [HttpGet("show/{by}={val}")]
        public IActionResult GetAllBy(string by, string val)
        {
            var indexes = City.GetIndexesOf(val, by, Cities);
            if (indexes.Count == 0)
            {
                _logger.LogError($"FAILURE- Received HTTP GET request to get a list of city by {by} property - no match found");
                return NotFound();
            }
            List<City> found = new List<City>();
            foreach (var i in indexes)
            {
                if (i >= 0 && i < Cities.Count)
                {
                    found.Add(Cities[i]);
                }
            }
            _logger.LogInformation($"SUCCESS- Received HTTP GET request to get a list of cities by {by} property");
            return Ok(found); 
        }

    }
}
