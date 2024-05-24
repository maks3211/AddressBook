using AddressBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AddresBook.Tests.ControllersTests
{
    public class CityControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        public CityControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Add_ShouldReturnCreated()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "/City/add");
            request.Content = new StringContent("{\"name\": \"Test City\", \"zip\": \"12345\", \"country\": \"Test Country\"}", Encoding.UTF8, "application/json");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }


        [Fact]
        public async Task RemoveById_ShouldReturnOk()
        {
            City.ResetId();
            //Arrange 
            var city1 = new City("City1", "11111", "Country1");
            var city2 = new City("City2", "22222", "Country2");
            Data.GetCityList().Add(city1);
            Data.GetCityList().Add(city2);
            var request = new HttpRequestMessage(HttpMethod.Post, "/City/remove0");

            //Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task RemoveById_ShouldReturnNotFound_WhenIdNotFound()
        {
            //Arrange 
            City.ResetId();          
            var city1 = new City("City1", "11111", "Country1");
            Data.GetCityList().Add(city1);
            var request = new HttpRequestMessage(HttpMethod.Post, "/City/remove10");

            //Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Edit_ShouldReturnOk_ByAllProperties()
        {
            //Arrange
            City.ResetId();
            var city1 = new City("City1", "11111", "Country1");
            Data.GetCityList().Add(city1);
            var nameEdit = new HttpRequestMessage(HttpMethod.Post, "/City/edit/0/name=New");
            var zipEdit = new HttpRequestMessage(HttpMethod.Post, "/City/edit/0/zip=New");
            var countryEdit = new HttpRequestMessage(HttpMethod.Post, "/City/edit/0/country=New");

            //Act
            var responseName = await _client.SendAsync(nameEdit);
            var responseZip = await _client.SendAsync(zipEdit);
            var responseCountry = await _client.SendAsync(countryEdit);

            // Assert
            Assert.Equal(HttpStatusCode.OK, responseName.StatusCode);
            Assert.Equal(HttpStatusCode.OK, responseZip.StatusCode);
            Assert.Equal(HttpStatusCode.OK, responseCountry.StatusCode);
        }

        [Fact]
        public async Task Edit_ShouldRetrunNotFound_BadIdOrBadProperty()
        {
            //Arrange
            City.ResetId();
            var city1 = new City("City1", "11111", "Country1");
            Data.GetCityList().Add(city1);
            var badId = new HttpRequestMessage(HttpMethod.Post, "/City/edit/1110/name=New");
            var badProperty = new HttpRequestMessage(HttpMethod.Post, "/City/edit/0/bad=New");

            //Act
            var responseBadId = await _client.SendAsync(badId);
            var responseBadProperty = await _client.SendAsync(badProperty);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, responseBadId.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, responseBadProperty.StatusCode);
        }

        [Fact]
        public async Task ShowAll_ShouldRetrunCities()
        {
            //Arrange
            City.ResetId();
            Data.GetCityList().Clear();
            var city1 = new City("City1", "11111", "Country1");
            var city2 = new City("City2", "11111", "Country1");
            Data.GetCityList().Add(city1);
            Data.GetCityList().Add(city2);
   
            //Act
            var response = await _client.GetAsync("/City/showAll");
            var contentString = await response.Content.ReadAsStringAsync();
            var cities = JsonSerializer.Deserialize<List<City>>(contentString);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, cities.Count);
        }
        [Fact]
        public async Task GetCity_ShouldReturnCityById()
        {
            // Arrange
            City.ResetId();
            Data.GetCityList().Clear();
            var city = new City("Test City", "12345", "Test Country");
            Data.GetCityList().Add(city);
            var request = new HttpRequestMessage(HttpMethod.Get, $"/City/show/{city.Id}");
            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }
        [Fact]
        public async Task GetCity_ShouldReturnCityById_NotFound()
        {
            // Arrange
            City.ResetId();
            Data.GetCityList().Clear();
            var city = new City("Test City", "12345", "Test Country");
            Data.GetCityList().Add(city);
            var request = new HttpRequestMessage(HttpMethod.Get, $"/City/show/{123123}");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetAllBy_Property()
        {
            // Arrange
            City.ResetId();
            Data.GetCityList().Clear();
            var city1 = new City("city1", "1", "country1");
            var city2 = new City("city1", "3", "country2");
            var city3 = new City("city3", "2", "country3");
            Data.GetCityList().Add(city1);
            Data.GetCityList().Add(city2);
            Data.GetCityList().Add(city2);
            var request = new HttpRequestMessage(HttpMethod.Get, $"/City/show/name=city1");

            // Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var cities = Newtonsoft.Json.JsonConvert.DeserializeObject<List<City>>(content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(cities);         
            Assert.Equal("city1", cities[0].Name);
            Assert.Equal("city1", cities[1].Name);
        }
    }
}



