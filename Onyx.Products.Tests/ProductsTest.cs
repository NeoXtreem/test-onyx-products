using System.Net.Http.Headers;
using System.Net.Http.Json;
using AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Onyx.Products.Models;

namespace Onyx.Products.Tests;

[TestClass]
public class ProductsTest
{
    private readonly Fixture _fixture = new();
    private readonly WebApplicationFactory<Program> _factory = new();
    private readonly IConfiguration _configuration = new ConfigurationBuilder().AddUserSecrets<ProductsTest>().Build();
    private const string TestColour1 = "Red";
    private const string TestColour2 = "Blue";
    private const string TestColour3 = "Green";

    [TestMethod]
    public async Task HealthCheckEndpoint_ShouldReturnOK()
    {
        // Arrange
        using var client = GetClient();

        // Act
        var response = await client.GetAsync("/");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("OK", content);
    }

    [TestMethod]
    public async Task GetProducts_ShouldReturnAllProducts()
    {
        // Arrange
        using var client = GetClient();

        // Act
        var response = await client.GetAsync("/products");

        // Assert
        response.EnsureSuccessStatusCode();
        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        Assert.IsNotNull(products);
        Assert.AreEqual(TestColour1.Length + TestColour2.Length + TestColour3.Length, products.Count);
    }

    [TestMethod]
    [DataRow(TestColour1)]
    [DataRow(TestColour2)]
    [DataRow(TestColour3)]
    public async Task GetProductsByValidColour_ShouldReturnFilteredProducts(string colour)
    {
        // Arrange
        using var client = GetClient();

        // Act
        var response = await client.GetAsync($"/products?colour={colour}");

        // Assert
        response.EnsureSuccessStatusCode();
        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        Assert.IsNotNull(products);
        Assert.AreEqual(colour.Length, products.Count);
    }

    [TestMethod]
    public async Task GetProductsByInvalidColour_ShouldReturnNoProducts()
    {
        // Arrange
        using var client = GetClient();

        // Act
        var response = await client.GetAsync($"/products?colour={_fixture.Create<string>()}");

        // Assert
        response.EnsureSuccessStatusCode();
        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        Assert.IsNotNull(products);
        Assert.AreEqual(0, products.Count);
    }

    private HttpClient GetClient()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["Products:ApiKey"]);
        return client;
    }
}