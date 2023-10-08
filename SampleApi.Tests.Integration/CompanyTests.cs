using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using SampleApi.Entities;

namespace SampleApi.Tests.Integration;

public class CompanyTests:IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CompanyTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/2")]
    public async Task Get_CompanyReturnsCreatedCompany(string url)
    {
        //arrange
        var client=_factory.CreateClient();

        //act
        var response = await client.GetAsync(url);

        //assert
        response.EnsureSuccessStatusCode();

        var company = await response.Content.ReadFromJsonAsync<Company>();
        Assert.NotNull(company);
        Assert.Equal(company.Id,2);
        Assert.Equal(company.Name, "Awesome Company");
    }
}