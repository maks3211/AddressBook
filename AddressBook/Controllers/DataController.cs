using AddressBook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AddressBook.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly ILogger<DataController> _logger;
        public DataController(ILogger<DataController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Saves the specified data type to a file.
        /// </summary>
        /// <param name="type">The type of data to save (city, person, book).</param>
        /// <param name="file">The name of the file to save the data to.</param>
        /// <returns>The IActionResult representing the operation result.</returns>
        [HttpPost("save")]
        public IActionResult SaveToFile(string type,string file)
        {
            type = type.ToLower();
            if (file != null && type != null)
            {
                switch (type)
                {
                    case "city":
                        Data.SaveToJson(Data.GetCityList(), file);
                        _logger.LogInformation($"SUCCESS- Received HTTP POST request to save City into {file}");
                        return Ok();
                    case "person":
                        Data.SaveToJson(Data.GetPersonList(), file);
                        _logger.LogInformation($"SUCCESS- Received HTTP POST request to save Person into {file}");
                        return Ok();
                    case "book":
                        Data.SaveToJson(Data.GetBook(), file);
                        _logger.LogInformation($"SUCCESS- Received HTTP POST request to save Book into {file}");
                        return Ok();
                    default:
                        _logger.LogError("FAILURE- Received HTTP POST request to save into - wrong object type");
                        return BadRequest("Invalid data");
                }            
            }
            _logger.LogError("FAILURE- Received HTTP POST request to save into - invalid data");
            return BadRequest("Invalid data");
        }

        /// <summary>
        /// Reads the specified data type from a file.
        /// </summary>
        /// <param name="type">The type of data to read (city, person, book).</param>
        /// <param name="file">The name of the file to read the data from.</param>
        /// <returns>The IActionResult representing the operation result.</returns>
        [HttpPost("read")]
        public IActionResult ReadFromFile(string type, string file)
        {
            if (Data.ReadFromJson(type, file))
            {
                return Ok();
            }
            _logger.LogError($"FAILURE- Received HTTP POST request to read from {file} - not found or invalid object - {type} type");
            return BadRequest("Invalid data");
        }
    }
}
