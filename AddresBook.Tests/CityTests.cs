using AddressBook.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AddresBook.Tests
{
    public class CityTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            //Arragne
            var name = "Test City Name";
            var zip = "Test zip 123";
            var country = "Test Country";

            //Act
            var city = new City(name, zip, country);   

            //Assert
            city.Name.Should().Be(name);
            city.Zip.Should().Be(zip);
            city.Country.Should().Be(country);
        }

        [Fact]
        public void RemoveById_ShouldRemove_WhenExists()
        {
            //Arragne 
            var city1 = new City("City1", "11111", "Country1");
            var city2 = new City("City2", "22222", "Country2");
            var cities = new List<City> { city1, city2 };

            //Act
            var result = City.RemoveById(city1.Id, cities);

            //Assert
            result.Should().BeTrue();
            cities.Should().NotContain(city1);
        }

        [Fact]
        public void RemoveById_ShouldReturnFalse_WhenNotExists()
        {
            //Arragne
            var city1 = new City("City1", "11111", "Country1");
            var city2 = new City("City2", "22222", "Country2");
            var cities = new List<City> { city1, city2 };
            var count = cities.Count();
            //Act
            var result = City.RemoveById(-1, cities);

            //Assert
            result.Should().BeFalse();
            cities.Should().HaveCount(count);
        }

        [Fact]
        public void GetIndexesOf_ShouldReturnIndexes_ByAllProperties()
        {
            // Arrange
            var city1 = new City("City1", "1", "Country1");
            var city2 = new City("City1", "2", "Country2");
            var city3 = new City("City3", "2", "Country1");
            var cities = new List<City> { city1, city2, city3 };

            //Act
            var indexesByCity = City.GetIndexesOf("City1", "name", cities);
            var indexesByZip = City.GetIndexesOf("2", "zip", cities);
            var indexesByCountry = City.GetIndexesOf("Country1", "country", cities);

            //Assert
            indexesByCity.Should().HaveCount(2);
            indexesByCity.Should().BeEquivalentTo(new List<int> { 0, 1 });

            indexesByZip.Should().HaveCount(2);
            indexesByZip.Should().BeEquivalentTo(new List<int> { 1, 2 });

            indexesByCountry.Should().HaveCount(2);
            indexesByCountry.Should().BeEquivalentTo(new List<int> { 0, 2 });
        }

        [Fact]
        public void GetIndexesOf_ShouldReturnEmptyList_WhenNoMatch()
        {
            // Arrange
            var city1 = new City("City1", "1", "Country1");
            var cities = new List<City> { city1};

            //Act
            var indexesByCity = City.GetIndexesOf("NonExistingCity", "name", cities);
            var indexesByZip = City.GetIndexesOf("NonExistingZip", "zip", cities);
            var indexesByCountry = City.GetIndexesOf("NonExistingCountry", "country", cities);

            //Assert
            indexesByCity.Should().BeEmpty();
            indexesByZip.Should().BeEmpty();
            indexesByCountry.Should().BeEmpty();
        }


        [Fact]
        public void GetIndexesOf_ShouldReturnEmptyList_WhenIncorrectProperty()
        {
            //Arrange 
            var city1 = new City("City1", "1", "Country1");
            var cities = new List<City> { city1 };

            //Act     
            var indexesByNonExistingProperty = City.GetIndexesOf("City1", "NonExistingProperty", cities);
            //Assert

            indexesByNonExistingProperty.Should().BeEmpty();
        }


        [Fact]
        public void GetIndex_ShouldReturnCorrectIndex_WhenExists()
        {
            // Arrange
            var city1 = new City("City1", "11111", "Country1");
            var city2 = new City("City2", "22222", "Country2");
            var cities = new List<City> { city1, city2 };

            // Act
            var index = City.GetIndex(city1.Id, cities);

            // Assert
            index.Should().Be(0);
        }

        [Fact]
        public void GetIndex_ShouldReturnMinusOne_WhenNotExists()
        {
            // Arrange
            var city1 = new City("City1", "11111", "Country1");
            var cities = new List<City> { city1 };

            // Act
            var index = City.GetIndex(-1, cities);

            // Assert
            index.Should().Be(-1);
        }


    }
}
