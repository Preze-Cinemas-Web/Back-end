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

        [Fact]
        public async Task ReservationRequest()
        {
            var route = "https://localhost:7236/API/Reservations/Reservation-Request";
            var client = _factory.CreateClient();

            ReservationRequestDTO reservationRequestDTO = new ReservationRequestDTO()
            {
                MovieTitle = "Damsel",
                NumberOfTickets = 2
            };

            var result = await TestUtilities.Post(client, route, reservationRequestDTO);
            var reservStatus = await ReadReservationStatus(result);

            Assert.True(reservStatus.Equals("Reservation request accepted."));
        }

        [Fact]
        public async Task ConfirmReservation()
        {
            var route = "https://localhost:7236/API/Reservations/Confirm-Reservation";
            var client = _factory.CreateClient();

            ConfirmReservationDTO confirmReservDTO = new ConfirmReservationDTO()
            {
                FirstName = "George",
                LastName = "Prezerakos",
                Email = "prezecinems@ethereal.email",
                Phone = "6971366764",
                Birthdate = "1970-07-29"
            };

            var result = await TestUtilities.Post(client, route, confirmReservDTO);
            var reservStatus = await ReadReservationStatus(result);

            Assert.True(reservStatus.Equals("Reservation confirmed."));
        }

        [Fact]
        public async Task DownloadTickets()
        {
            var route = "https://localhost:7236/API/Reservations/Download-Tickets";
            var client = _factory.CreateClient();

            var result = await TestUtilities.Get(client, route);
            var tickets = await ReadTickets(result);

            Assert.True(tickets.Count() >= 0);
        }

        [Fact]


        private async Task<IEnumerable<ReservationDTO>> ReadReservations(HttpResponseMessage result)
        {
            Assert.True(result.IsSuccessStatusCode);

            var content = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ReservationDTO>>(content);
        }

        private async Task<IEnumerable<DownloadTicketsDTO>> ReadTickets(HttpResponseMessage result)
        {
            Assert.True(result.IsSuccessStatusCode);

            var content = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<DownloadTicketsDTO>>(content);
        }

        private async Task<string> ReadReservationStatus(HttpResponseMessage result)
        {
            Assert.True(result.IsSuccessStatusCode);

            return await result.Content.ReadAsStringAsync();
        }
    }
}
