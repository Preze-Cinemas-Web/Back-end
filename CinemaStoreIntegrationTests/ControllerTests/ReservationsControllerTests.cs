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
        public async Task GetReservationsByUserId()
        {
            var route = "https://localhost:7236/API/Reservations/Get-Reservations-by-UserId";
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
                MovieTitle = "The Weapon",
                NumberOfTickets = 5
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
                FirstName = "Giorgos",
                LastName = "Prezerakos",
                Email = "prezecinems@ethereal.email",
                Phone = "6971366764",
                Birthdate = "1970-07-29",
                Price = 40
            };

            var result = await TestUtilities.Post(client, route, confirmReservDTO);
            var reservStatus = await ReadReservationStatus(result);

            Assert.False(reservStatus.Equals("User not found."));
            Assert.False(reservStatus.Equals("Reservation not found."));
            Assert.False(reservStatus.Equals("First name unconfirmed."));
            Assert.False(reservStatus.Equals("Last name unconfirmed."));
            Assert.False(reservStatus.Equals("Email unconfirmed."));
            Assert.False(reservStatus.Equals("Phone unconfirmed."));
            Assert.False(reservStatus.Equals("Birthdate unconfirmed."));
            Assert.False(reservStatus.Equals("Total price unconfirmed."));
            Assert.False(reservStatus.Equals("Not enough tickets."));
        }

        [Fact]
        public async Task DownloadTicketsByBookingId()
        {
            // Remove folder Tickets cuz 500 
            var route = "https://localhost:7236/API/Reservations/Download-Tickets-by-BookingId?bookingId=";
            var client = _factory.CreateClient();

            var bookingId = "E7XSP9";
            route += bookingId;

            var result = await TestUtilities.Get(client, route);
            var ticketStatus = await ReadReservationStatus(result);

            Assert.False(ticketStatus.Equals("Reservation not found."));
        }

        [Fact]
        public async Task CancelReservation()
        {
            var route = "https://localhost:7236/API/Reservations/Cancel-Reservation?bookingId=";
            var client = _factory.CreateClient();

            string bookingId = "OBO9YA";
            route += bookingId;

            var result = await TestUtilities.Delete(client, route);

            Assert.True(result.IsSuccessStatusCode);
        }

        private async Task<IEnumerable<ReservationDTO>> ReadReservations(HttpResponseMessage result)
        {
            Assert.True(result.IsSuccessStatusCode);

            var content = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ReservationDTO>>(content);
        }

        private async Task<string> ReadReservationStatus(HttpResponseMessage result)
        {
            Assert.True(result.IsSuccessStatusCode);

            return await result.Content.ReadAsStringAsync();
        }
    }
}
