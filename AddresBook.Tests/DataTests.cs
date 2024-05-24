using AddressBook.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddresBook.Tests
{
    public class DataTests
    {
        
    [Fact]
        public void GetCityList_ShouldReturnCityList()
        {
            // Arrange
            var cityList = Data.GetCityList();

            // Act & Assert
            cityList.Should().NotBeNull();
            cityList.Should().BeOfType<List<City>>();
        }

        [Fact]
        public void GetPersonList_ShouldReturnPersonList()
        {
            // Arrange
            var personList = Data.GetPersonList();

            // Act & Assert
            personList.Should().NotBeNull();
            personList.Should().BeOfType<List<Person>>();
        }

        [Fact]
        public void GetBook_ShouldReturnAddressBook()
        {
            // Arrange
            var bookList = Data.GetBook();

            // Act & Assert
            bookList.Should().NotBeNull();
            bookList.Should().BeOfType<List<Book>>();
        }

        [Fact]
        public void SaveToJson_ShouldSaveListToJsonFile()
        {
            // Arrange
            var testCities = new List<City>
        {
            new City("TestCity1", "11111", "Country1"),
            new City("TestCity2", "22222", "Country2")
        };
            string fileName = "TestCities";

            // Act
            Data.SaveToJson(testCities, fileName);

            // Assert
            File.Exists(fileName + ".json").Should().BeTrue();

            // Cleanup
            File.Delete(fileName + ".json");
        }

        [Fact]
        public void ReadFromJson_ShouldReadListFromJsonFile()
        {
            // Arrange
            var testCities = new List<City>
        {
            new City("TestCity1", "11111", "Country1"),
            new City("TestCity2", "22222", "Country2")
        };
            string fileName = "TestCities";
            Data.SaveToJson(testCities, fileName);

            // Act
            var result = Data.ReadFromJson("City", fileName);

            // Assert
            result.Should().BeTrue();
            var cities = Data.GetCityList();
            cities.Should().NotBeEmpty();
            cities.Should().Contain(c => c.Name == "TestCity1" && c.Zip == "11111" && c.Country == "Country1");
            cities.Should().Contain(c => c.Name == "TestCity2" && c.Zip == "22222" && c.Country == "Country2");

            // Cleanup
            File.Delete(fileName + ".json");
        }

        [Fact]
        public void ReadFromJson_ShouldReturnFalse_WhenFileDoesNotExist()
        {
            // Arrange
            string fileName = "NonExistentFile";

            // Act
            var result = Data.ReadFromJson("City", fileName);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void ReadFromJson_ShouldReturnFalse_WhenInvalidObjectName()
        {
            // Arrange
            var testCities = new List<City>
        {
            new City("TestCity1", "11111", "Country1"),
            new City("TestCity2", "22222", "Country2")
        };
            string fileName = "TestCities";
            Data.SaveToJson(testCities, fileName);

            // Act
            var result = Data.ReadFromJson("InvalidObject", fileName);

            // Assert
            result.Should().BeFalse();

            // Cleanup
            File.Delete(fileName + ".json");
        }
    }
}

