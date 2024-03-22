using CinemaStore.Models;
using Microsoft.Extensions.Options;

namespace CinemaStore.Business.Movies
{
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
}
