using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using AddressBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;
using Moq; 

namespace AddressBookTest
{
    public class BookTest
    {
        private readonly Book _controller;
        private readonly Mock<ILogger<Book>> _mockLogger;
        public BookTest() {
      
        }

        [Fact]
        public void AddEntry_ValidData_ReturnsOk()
        {
            // Arrange
            var bookDto = new BookDto { CityId = 1, PersonId = 1, Description = "Test" };

            // Act
            var result = Book.AddEntry(bookDto.CityId, bookDto.PersonId, bookDto.Description);

            // Assert
            var okResult = Xunit.Assert.IsType<OkResult>(result);
            Xunit.Assert.Equal(200, okResult.StatusCode);
        }
    }


}
