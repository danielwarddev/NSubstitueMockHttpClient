using System.Net;
using System.Net.Http.Json;

namespace NSubstitueMockHttpClient;

public record Book(int Id, string Name, DateTime PublishedDate);

public interface IBookClient
{
    Task<Book[]> GetBooks(int genreId);
}

public class BookClient : IBookClient
{
    private readonly HttpClient _httpClient;

    public BookClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Book[]> GetBooks(int genreId)
    {
        try
        {
            return (await _httpClient.GetFromJsonAsync<Book[]>($"books/{genreId}"))!;
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound)
            {
                return Array.Empty<Book>();
            }
            throw;
        }
    }
}
