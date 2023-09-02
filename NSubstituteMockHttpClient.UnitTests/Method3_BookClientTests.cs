using AutoFixture;
using FluentAssertions;
using NSubstitueMockHttpClient;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http.Json;

namespace NSubstituteMockHttpClient.UnitTests;

// Using RichardSzalay.MockHttp
public class Method3_BookClientTests
{
    private readonly BookClient _bookClient;
    private readonly Fixture _fixture = new();
    private readonly MockHttpMessageHandler _handler = new();
    private readonly string _baseAddress = "https://www.example.com";

    public Method3_BookClientTests()
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

        _handler
            .Expect(HttpMethod.Get, $"{_baseAddress}/books/{genreId}")
            .Respond(HttpStatusCode.OK, JsonContent.Create(expectedBooks));

        var actualBooks = await _bookClient.GetBooks(genreId);

        actualBooks.Should().BeEquivalentTo(expectedBooks);
    }

    [Fact]
    public async Task When_Call_To_Get_Books_Returns_404_Then_Returns_Empty_Array()
    {
        var genreId = _fixture.Create<int>();

        _handler
            .Expect(HttpMethod.Get, $"{_baseAddress}/books/{genreId}")
            .Respond(HttpStatusCode.NotFound);

        var actualBooks = await _bookClient.GetBooks(genreId);

        actualBooks.Should().BeEmpty();
    }
}
