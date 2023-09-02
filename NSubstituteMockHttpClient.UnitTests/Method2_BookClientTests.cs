using AutoFixture;
using FluentAssertions;
using NSubstitueMockHttpClient;
using NSubstitute;
using System.Net;

namespace NSubstituteMockHttpClient.UnitTests;

// Using reflection to make NSubstitute set up the SendAsync method through extensions methods
public class Method2_BookClientTests
{
    private readonly BookClient _bookClient;
    private readonly Fixture _fixture = new();
    private readonly HttpMessageHandler _handler = Substitute.For<HttpMessageHandler>();
    private readonly string _baseAddress = "https://www.example.com";

    public Method2_BookClientTests()
    {
        var httpClient = new HttpClient(_handler)
        {
            BaseAddress = new Uri(_baseAddress)
        };
        _bookClient = new BookClient(httpClient);
    }

    [Fact]
    public async Task When_Call_To_Get_Books_Succeeds_Then_Returns_Books()
    {
        var genreId = _fixture.Create<int>();
        var expectedBooks = _fixture.CreateMany<Book>();
        var endpoint = $"{_baseAddress}/books/{genreId}";

        _handler
            .SetupRequest(HttpMethod.Get, endpoint)
            .ReturnsResponse(HttpStatusCode.OK, expectedBooks);

        var actualBooks = await _bookClient.GetBooks(genreId);

        _handler.ShouldHaveReceived(HttpMethod.Get, endpoint);
        actualBooks.Should().BeEquivalentTo(expectedBooks);
    }

    [Fact]
    public async Task When_Call_To_Get_Books_Returns_404_Then_Returns_Empty_Array()
    {
        var genreId = _fixture.Create<int>();
        var endpoint = $"{_baseAddress}/books/{genreId}";

        _handler
            .SetupRequest(HttpMethod.Get, endpoint)
            .ReturnsResponse(HttpStatusCode.NotFound);

        var actualBooks = await _bookClient.GetBooks(genreId);

        _handler.ShouldHaveReceived(HttpMethod.Get, endpoint);
        actualBooks.Should().BeEmpty();
    }
}
