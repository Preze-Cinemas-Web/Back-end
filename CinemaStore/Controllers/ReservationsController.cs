using CinemaStore.Business;
using CinemaStore.Business.Reservations;
using CinemaStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace CinemaStore.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("Get-All-Reservations")]
        public IActionResult GetAllReservations()
        {
            var reservations = _reservationService.FindAllReservations();
            
            return Ok(reservations);
        }

        [Authorize]
        [HttpPost]
        [Route("Reservation")]
        public async Task<IActionResult> MakeReservation([FromBody] ReservationDTO reserv)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                    return Unauthorized("Invalid token.");

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var userId = jwtToken.Payload["userId"].ToString();

                var success = await _reservationService.MakeReservationAsync(reserv, Int32.Parse(userId));

                if (success)
                    return Ok("Reservation successful.");
                else
                    return BadRequest("Failed to make reservation.");
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = ex.Message });
            }
        }


    }
}
