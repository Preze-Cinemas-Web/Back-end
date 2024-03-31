using CinemaStore;
using CinemaStore.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace CinemaStoreIntegrationTests.ControllerTests
{
    public class HallsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private WebApplicationFactory<Program> _factory;
        private const string EnvironmentVariable = "ASPNETCORE_ENVIRONMENT";

        public HallsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            Environment.SetEnvironmentVariable(EnvironmentVariable, "Staging");
        }

        [Fact]
        public async Task GetAllHalls()
        {
            var route = "https://localhost:7236/API/Halls/Get-All-Halls";
            var client = _factory.CreateClient();

            var result = await TestUtilities.Get(client, route);
            var halls = await ReadHalls(result);

            Assert.True(halls.Count() > 0);
        }

        private async Task<IEnumerable<HallDTO>> ReadHalls(HttpResponseMessage result)
        {
            Assert.True(result.IsSuccessStatusCode);
            string responseContent = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<HallDTO>>(responseContent);
        }

    }
}
