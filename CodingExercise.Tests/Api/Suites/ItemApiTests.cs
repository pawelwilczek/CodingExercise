using CodingExercise.Infrastructure;
using CodingExercise.Model;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CodingExercise.Tests.Api
{
    public class ItemApiTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly string basePath = "/api/item";

        public ItemApiTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_Return200StatusCodeAndAllItemsFromDb()
        {
            // Given
            var client = _factory.ReinitializeDb();
            var expectedItemsCount = DataSeeder.Items.Count;

            // When
            var response = await client.GetAsync(basePath);
            var items = await ClientHelper.DeserializeResponseAsync<List<Item>>(response);

            // Then
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expectedItemsCount, items.Count);
        }

        [Fact]
        public async Task Get_Return200StatusCodeAndCorrectItem_WhenItemExist()
        {
            // Given
            var client = _factory.ReinitializeDb();
            var expectedItem = DataSeeder.Items.First();

            // When
            var response = await client.GetAsync($"{basePath}/{expectedItem.Key}");
            var item = await ClientHelper.DeserializeResponseAsync<Item>(response);

            // Then
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expectedItem.Key, item.Key);
            Assert.Equal(expectedItem.Value, item.Value);
        }

        [Fact]
        public async Task Get_Return404StatusCode_WhenItemNotExist()
        {
            // Given
            var client = _factory.ReinitializeDb();
            var notExistingItemKey = "notExistingKey"; 

            // When
            var response = await client.GetAsync($"{basePath}/{notExistingItemKey}");

            // Then
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Post_Return201StatusCodeAndAddNewItem_WhenItemNotExistPreviously()
        {
            // Given
            var client = _factory.ReinitializeDb();
            var expectedItemsCountAfterAddition = DataSeeder.Items.Count + 1;
            var itemToAdd = new Item { Key = "newKey", Value = "newValue" };

            // When
            var postResponse = await client.PostAsync(basePath, ClientHelper.SerializeObject(itemToAdd));

            var getResponse = await client.GetAsync(basePath);
            var items = await ClientHelper.DeserializeResponseAsync<List<Item>>(getResponse);

            // Then
            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            Assert.Equal(expectedItemsCountAfterAddition, items.Count);
        }

        [Fact]
        public async Task Post_Return409StatusCode_WhenItemAlreadyExist()
        {
            // Given
            var client = _factory.ReinitializeDb();
            var expectedItemsCountAfterAddition = DataSeeder.Items.Count;
            var itemToAdd = DataSeeder.Items.First();

            // When
            var postResponse = await client.PostAsync(basePath, ClientHelper.SerializeObject(itemToAdd));

            var getResponse = await client.GetAsync(basePath);
            var items = await ClientHelper.DeserializeResponseAsync<List<Item>>(getResponse);

            // Then
            Assert.Equal(HttpStatusCode.Conflict, postResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            Assert.Equal(expectedItemsCountAfterAddition, items.Count);
        }

        [Fact]
        public async Task Post_Return400StatusCode_WhenItemHasFieldsWithIncorrectType()
        {
            // Given
            var client = _factory.ReinitializeDb();
            var expectedItemsCountAfterAddition = DataSeeder.Items.Count;
            var itemToAdd = new { Key = 1234, Value = 21441 };

            // When
            var postResponse = await client.PostAsync(basePath, ClientHelper.SerializeObject(itemToAdd));

            var getResponse = await client.GetAsync(basePath);
            var items = await ClientHelper.DeserializeResponseAsync<List<Item>>(getResponse);

            // Then
            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            Assert.Equal(expectedItemsCountAfterAddition, items.Count);
        }

        [Fact]
        public async Task Put_Return204StatusCode_WhenUpdateWasSucceded()
        {
            // Given
            var client = _factory.ReinitializeDb();
            var itemToUpdate = DataSeeder.Items.First();
            itemToUpdate.Value = "updatedValue";

            // When
            var putResponse = await client.PutAsync($"{basePath}/{itemToUpdate.Key}", ClientHelper.SerializeObject(itemToUpdate));

            var getResponse = await client.GetAsync($"{basePath}/{itemToUpdate.Key}");
            var item = await ClientHelper.DeserializeResponseAsync<Item>(getResponse);

            // Then
            Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            Assert.Equal(item.Value, itemToUpdate.Value);
        }

        [Fact]
        public async Task Put_Return400StatusCode_WhenItemHasFieldsWithIncorrectType()
        {
            // Given
            var client = _factory.ReinitializeDb();
            var itemToUpdateKey = DataSeeder.Items.First().Key;
            var itemToUpdate = new { Key = itemToUpdateKey, Value = 21441 };

            // When
            var putResponse = await client.PutAsync($"{basePath}/{itemToUpdate.Key}", ClientHelper.SerializeObject(itemToUpdate));

            // Then
            Assert.Equal(HttpStatusCode.BadRequest, putResponse.StatusCode);
        }

        [Fact]
        public async Task Put_Return400StatusCode_WhenKeyInPathIsDifferentThanInBody()
        {
            // Given
            var client = _factory.ReinitializeDb();
            var itemToUpdate = DataSeeder.Items.First();
            var notMatchingKey = "notMatchingKey";

            // When
            var putResponse = await client.PutAsync($"{basePath}/{notMatchingKey}", ClientHelper.SerializeObject(itemToUpdate));

            // Then
            Assert.Equal(HttpStatusCode.BadRequest, putResponse.StatusCode);
        }

        [Fact]
        public async Task Put_Return404StatusCode_WhenItemNotExist()
        {
            // Given
            var client = _factory.ReinitializeDb();
            var notExistingItem = new Item { Key = "notExistingKey", Value = "notExistingValue" };

            // When
            var putResponse = await client.PutAsync($"{basePath}/{notExistingItem.Key}", ClientHelper.SerializeObject(notExistingItem));

            // Then
            Assert.Equal(HttpStatusCode.NotFound, putResponse.StatusCode);
        }

        [Fact]
        public async Task Delete_Return204StatusCode_WhenDeleteWasSucceded()
        {
            // Given
            var client = _factory.ReinitializeDb();
            var itemToDelete = DataSeeder.Items.First();
            var expectedItemsCountAfterDeletion = DataSeeder.Items.Count - 1;

            // When
            var deleteResponse = await client.DeleteAsync($"{basePath}/{itemToDelete.Key}");

            var getResponse = await client.GetAsync(basePath);
            var items = await ClientHelper.DeserializeResponseAsync<List<Item>>(getResponse);

            // Then
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            Assert.Equal(expectedItemsCountAfterDeletion, items.Count);
        }

        [Fact]
        public async Task Delete_Return404StatusCode_WhenItemNotExist()
        {
            // Given
            var client = _factory.ReinitializeDb();
            var notExistingItem = new Item { Key = "notExistingKey", Value = "notExistingValue" };
            var expectedItemsCountAfterDeletion = DataSeeder.Items.Count;

            // When
            var deleteResponse = await client.DeleteAsync($"{basePath}/{notExistingItem.Key}");

            var getResponse = await client.GetAsync(basePath);
            var items = await ClientHelper.DeserializeResponseAsync<List<Item>>(getResponse);

            // Then
            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            Assert.Equal(expectedItemsCountAfterDeletion, items.Count);
        }

    }
}
