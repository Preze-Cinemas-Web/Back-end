using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

public class TMDBService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public TMDBService(HttpClient httpClient, IOptions<TMDBSettings> tmdbSettings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.themoviedb.org/3/");
        _apiKey = tmdbSettings.Value.ApiKey;
    }

    public async Task<string> GetPopularMovies()
    {
        var response = await _httpClient.GetAsync($"movie/popular?api_key={_apiKey}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}

