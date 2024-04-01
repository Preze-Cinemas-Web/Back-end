using CinemaStore;
using CinemaStore.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace CinemaStoreIntegrationTests.ControllerTests
{
    public class ReservationsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private WebApplicationFactory<Program> _factory;
        private const string EnvironmentVariable = "ASPNETCORE_ENVIRONMENT";

        public ReservationsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            Environment.SetEnvironmentVariable(EnvironmentVariable, "Staging");
        }

        [Fact]
        public async Task GetAllReservations()
        {
            var route = "https://localhost:7236/API/Reservations/Get-All-Reservations";
            var client = _factory.CreateClient();

            var result = await TestUtilities.Get(client, route);
            var reservations = await ReadReservations(result);

            Assert.True(reservations.Count() >= 0);
        }

        private async Task<IEnumerable<ReservationDTO>> ReadReservations(HttpResponseMessage result)
        {
            Assert.True(result.IsSuccessStatusCode);

            var content = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ReservationDTO>>(content);
        }
    }
}
