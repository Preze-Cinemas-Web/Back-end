using Cinema.Models;
using CinemaStore;
using CinemaStore.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace CinemaStoreIntegrationTests.ControllerTests
{
    public class MoviesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private WebApplicationFactory<Program> _factory;
        private const string EnvironmentVariable = "ASPNETCORE_ENVIRONMENT";

        public MoviesControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            Environment.SetEnvironmentVariable(EnvironmentVariable, "Staging");
        }

        [Fact]
        public async Task GetAllMoviesViews()
        {
            var route = "https://localhost:7236/API/Movies/Get-All-Movies-Views";
            var client = _factory.CreateClient();

            var result = await TestUtilities.Get(client, route);
            var movies = await ReadMoviesViews(result);

            Assert.True(movies.Count() > 0);
        }

        [Fact]
        public async Task GerMovieViewsByTitle()
        {
            var route = "https://localhost:7236/API/Movies/Get-Movie-Views-by-Title?title=";
            var client = _factory.CreateClient();

            string title = "Damsel";
            route += title;

            var result = await TestUtilities.Get(client, route);
            var movie = await ReadMovie(result);

            Assert.True(movie.Title == title);
        }

        private async Task<IEnumerable<MovieDTO>> ReadMoviesViews(HttpResponseMessage result)
        {
            Assert.True(result.IsSuccessStatusCode);
            string responseContent = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<MovieDTO>>(responseContent);
        }

        private async Task<MovieDTO> ReadMovie(HttpResponseMessage result)
        {
            Assert.True(result.IsSuccessStatusCode);
            string responseContent = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<MovieDTO>(responseContent);
        }
    }
}
