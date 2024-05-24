using AddressBook.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddresBook.Tests
{
    public class PersonTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            //Arragne
            var name = "TestName";
            var lastName = "TestLastName";

            //Act
            var person = new Person(name, lastName);

            //Assert
            person.Name.Should().Be(name);
            person.LastName.Should().Be(lastName);           
        }

        [Fact]
        public void RemoveById_ShouldRemove_WhenExists()
        {
            //Arrange
            var person1 = new Person("Name1", "LastName1");
            var person2 = new Person("Name2", "LastName2");
            var people = new List<Person> { person1, person2 };    

            //Act
            var result = Person.RemoveById(person1.Id, people);

            //Assert
            result.Should().BeTrue();   
            people.Should().NotContain(person1);
        }

        [Fact]
        public void RemoveById_ShouldReturnFalse_WhenNotExists()
        {
            //Arragne
            var person1 = new Person("Name1", "LastName1");
            var person2 = new Person("Name2", "LastName2");
            var people = new List<Person> { person1, person2 };
            var count = people.Count();
            //Act
            var result = Person.RemoveById(-1, people);

            //Assert
            result.Should().BeFalse();
            people.Should().HaveCount(count);
        }

        [Fact]
        public void GetIndexesOf_ShouldReturnIndexes_ByAllProperties()
        {
            // Arrange
            var person1 = new Person("Name1", "LastName1");
            var person2 = new Person("Name1", "LastName2");
            var person3 = new Person("Name3", "LastName2");
            var peopele = new List<Person> { person1, person2, person3 };

            //Act
            var indexesByName = Person.GetIndexesOf("Name1", "name", peopele);
            var indexesByLastName = Person.GetIndexesOf("LastName2", "lastname", peopele);


            //Assert
            indexesByName.Should().HaveCount(2);
            indexesByName.Should().BeEquivalentTo(new List<int> { 0, 1 });

            indexesByLastName.Should().HaveCount(2);
            indexesByLastName.Should().BeEquivalentTo(new List<int> { 1, 2 });
        }


        [Fact]
        public void GetIndexesOf_ShouldReturnEmptyList_WhenNoMatch()
        {
            // Arrange
            var person1 = new Person("Name1", "LastName1" );
            var peopele = new List<Person> { person1 };

            //Act
            var indexesByName = Person.GetIndexesOf("NonExistingName", "name", peopele);
            var indexesByLastName = Person.GetIndexesOf("NonExistingLastName", "lastname", peopele);


            //Assert
            indexesByName.Should().BeEmpty();
            indexesByLastName.Should().BeEmpty();
        }

        [Fact]
        public void GetIndexesOf_ShouldReturnEmptyList_WhenIncorrectProperty()
        {
            //Arrange 
            var person1 = new Person("Name1", "LastName1");
            var peopele = new List<Person> { person1 };

            //Act
            var indexesByNonExistingProperty = Person.GetIndexesOf("Name1", "NonExistingProperty", peopele);

            //Assert
            indexesByNonExistingProperty.Should().BeEmpty();
        }

        [Fact]
        public void GetIndex_ShouldReturnCorrectIndex_WhenExists()
        {
            // Arrange
            var person1 = new Person("Name1", "LastName1");
            var person2 = new Person("Name2", "LastName2");
            var peopele = new List<Person> { person1, person2};

            // Act
            var index = Person.GetIndex(person1.Id, peopele);

            // Assert
            index.Should().Be(0);
        }

        [Fact]
        public void GetIndex_ShouldReturnMinusOne_WhenNotExists()
        {
            // Arrange
            var person1 = new Person("Name1", "LastName1");
            var peopele = new List<Person> { person1 };

            // Act
            var index = Person.GetIndex(-1, peopele);

            // Assert
            index.Should().Be(-1);
        }

    }
}
