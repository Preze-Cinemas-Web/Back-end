using System.Text.Json;

namespace CinemaStore.Business.Movies
{
    public class TmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public TmdbService(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
            _httpClient.BaseAddress = new Uri("https://api.themoviedb.org/3/");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<dynamic> GetPopularMoviesAsync()
        {
            var requestUri = $"movie/popular?api_key={_apiKey}";

            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            var contentStream = await response.Content.ReadAsStreamAsync();
            var movies = await JsonSerializer.DeserializeAsync<dynamic>(contentStream);
           
            return movies;
        }
    }
}


