using AutoFixture;
using FluentAssertions;
using NSubstitueMockHttpClient;
using System.Net;

namespace NSubstituteMockHttpClient.UnitTests;

// Using a self-made custom class for a fake HttpMessageHandler
public class Method1_BookClientTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task When_Call_To_Get_Books_Succeeds_Then_Returns_Books()
    {
        var genreId = _fixture.Create<int>();
        var expectedBooks = _fixture.CreateMany<Book>();

        var handler = new MyMockHttpMessageHandler(HttpStatusCode.OK, expectedBooks);
        var client = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://www.example.com")
        };
        var bookClient = new BookClient(client);

        var actualBooks = await bookClient.GetBooks(genreId);

        actualBooks.Should().BeEquivalentTo(expectedBooks);
    }

    [Fact]
    public async Task When_Call_To_Get_Books_Returns_404_Then_Returns_Empty_Array()
    {
        var genreId = _fixture.Create<int>();

        var handler = new MyMockHttpMessageHandler(HttpStatusCode.NotFound);
        var client = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://www.example.com")
        };
        var bookClient = new BookClient(client);

        var actualBooks = await bookClient.GetBooks(genreId);

        actualBooks.Should().BeEmpty();
    }
}