using CinemaStore.Business.Reservations;
using CinemaStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaStore.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("API/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Get-All-Reservations")]
        public IActionResult GetAllReservations()
        {
            var reservations = _reservationService.FindAllReservations();
            
            return Ok(reservations);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Reservation")]
        public async Task<IActionResult> MakeReservation([FromBody] int numberOfTickets)
        {
            var success = await _reservationService.MakeReservationAsync(numberOfTickets);

            if (success)
                return Ok("Reservation successful.");
            else
                return BadRequest("Failed to make reservation.");
        }


    }
}
