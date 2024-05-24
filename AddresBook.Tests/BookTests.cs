using AddressBook.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddresBook.Tests
{
    public class BookTests
    {
        private readonly City _city;
        private readonly Person _person;
        private readonly List<City> _cityList = new List<City> { };
        private readonly List<Person> _personList = new List<Person> { };
        public BookTests()
        {
            Book.restId();
            _city = new City("City1", "11111", "Country1");
            _person = new Person("Name1", "LastName1");
            _cityList.Add(_city);
            _personList.Add(_person);
        }

        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            //Arragne 
            var cityId = 1;
            var personId = 1;
            var desc = "Description Test";
            //Act 
            var entity = new Book(cityId, personId, desc);

            //Assert
            entity.CityId.Should().Be(cityId);
            entity.PersonId.Should().Be(personId);
            entity.Description.Should().Be(desc);
        }

        [Fact]
        public void AddEntry_ShouldReturnBook_WhenCityPersonExists()
        {
            //Act
            var result = Book.AddEntry(_city.Id, _person.Id, "Description Test", _cityList, _personList);

            //Assert
            result.Should().NotBeNull();
            result.CityId.Should().Be(_city.Id);
            result.PersonId.Should().Be(_person.Id);
            result.Description.Should().Be("Description Test");
        }

        [Fact]
        public void AddEntry_ShouldReturnNull_WhenCityOrPersonNonExists()
        {
            //Act
            var resultNoCity = Book.AddEntry(-1, _person.Id, "Description Test", _cityList, _personList);
            var resultNoPerson = Book.AddEntry(_city.Id, -1, "Description Test", _cityList, _personList);

            //Assert
            resultNoCity.Should().BeNull();
            resultNoPerson.Should().BeNull();
        }

        [Fact]
        public void RemoveById_ShouldRemove_WhenExists()
        {
            //Arragne
            var entity1 = new Book(0, 0, "desc1");
            var entity2 = new Book(1, 1, "desc2");
            var book = new List<Book> { entity1, entity2 };

            //Act 
            var result = Book.RemoveById(0, book);

            //Assert
            result.Should().BeTrue();
            book.Should().NotContain(entity1);
        }

        [Fact]
        public void RemoveById_ShouldReturnFalse_WhenNotExists()
        {
            //Arragne
            var entity1 = new Book(0, 0, "desc1");
            var entity2 = new Book(1, 1, "desc2");
            var book = new List<Book> { entity1, entity2 };
            var count = book.Count();
            //Act
            var result = Book.RemoveById(-1, book);

            //Assert
            result.Should().BeFalse();
            book.Should().HaveCount(count);
        }

        [Fact]
        public void GetIndexesOf_ShouldReturnIndexes_ByAllProperties()
        {
            //Arrange
            var entity1 = new Book(0, 0, "desc1");
            var entity2 = new Book(0, 1, "desc2");
            var entity3 = new Book(1, 1, "desc3");
            var book = new List<Book> { entity1, entity2, entity3 };

            //Act
            var indexesByCityId = Book.GetIndexesOf("0", "cityid", book);
            var indexesByPersonId = Book.GetIndexesOf("1", "personid", book);

            //Assert
            indexesByCityId.Should().HaveCount(2);
            indexesByCityId.Should().BeEquivalentTo(new List<int> { 0, 1 });
            indexesByPersonId.Should().HaveCount(2);
            indexesByPersonId.Should().BeEquivalentTo(new List<int> { 1, 2 });
        }

        [Fact]
        public void GetIndexesOf_ShouldReturnEmptyList_WhenNoMatch()
        {
            // Arrange
            var entity1 = new Book(0, 0, "desc1");
            var book = new List<Book> { entity1};

            //Act
            var indexesByCityId = Book.GetIndexesOf("123123", "cityid", book);
            var indexesByPersonId = Book.GetIndexesOf("123123", "personid", book);

            //Assert
            indexesByCityId.Should().BeEmpty();
            indexesByPersonId.Should().BeEmpty();
        }

        [Fact]
        public void GetIndexesOf_ShouldReturnEmptyList_WhenIncorrectProperty()
        {
            //Arrange 
            var entity1 = new Book(0, 0, "desc1");
            var book = new List<Book> { entity1 };

            //Act     
            var indexesByNonExistingProperty = Book.GetIndexesOf("0", "NonExistingProperty", book);

            //Assert
            indexesByNonExistingProperty.Should().BeEmpty();
        }

        [Fact]
        public void GetIndexesOf_ShouldReturnEmptyList_WhenIdIsNON()
        {
            //Arrange 
            var entity1 = new Book(0, 0, "desc1");
            var book = new List<Book> { entity1 };

            //Act     
            var indexesByNonExistingProperty = Book.GetIndexesOf("Not a Number", "cityid", book);

            //Assert
            indexesByNonExistingProperty.Should().BeEmpty();
        }

        [Fact]
        public void GetIndex_ShouldReturnCorrectIndex_WhenExists()
        {
            // Arrange
            var entity1 = new Book(0, 0, "desc1");
            var entity2 = new Book(0, 1, "desc2");
            var book = new List<Book> { entity1, entity2 };

            // Act
            var index = Book.GetIndex(entity1.Id, book);

            // Assert
            index.Should().Be(0);
        }

        [Fact]
        public void GetIndex_ShouldReturnMinusOne_WhenNotExists()
        {
            // Arrange
            var entity1 = new Book(0, 0, "desc1");
            var book = new List<Book> { entity1 };

            // Act
            var index = Book.GetIndex(-1, book);

            // Assert
            index.Should().Be(-1);
        }
    }
}
