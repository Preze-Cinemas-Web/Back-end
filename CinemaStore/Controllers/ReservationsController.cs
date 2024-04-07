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
        [HttpGet]
        [Route("Get-Reservations-by-UserId")]
        public IActionResult GetReservationsByUserId()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Invalid token.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var userId = jwtToken.Payload["userId"].ToString();

            var reservations = _reservationService.FindReservationsByUserId(Int32.Parse(userId));

            return Ok(reservations);
        }

        [Authorize]
        [HttpPost]
        [Route("Reservation-Request")]
        public async Task<IActionResult> MakeReservation([FromBody] ReservationRequestDTO reserv)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                    return Unauthorized("Invalid token.");

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var userId = jwtToken.Payload["userId"].ToString();

                var reservStatus = await _reservationService.MakeReservationAsync(reserv, Int32.Parse(userId));
                
                switch(reservStatus)
                {
                    case "Movie not found.":
                        return NotFound(reservStatus);
                    case "Number of tickets request denied.":
                        return BadRequest(reservStatus);
                    case "You are allowed to book 1-9 tickets.":
                        return BadRequest(reservStatus);
                    case "Email not verified.":
                        return Unauthorized(reservStatus);
                    case "Reservation request accepted.":
                        return Ok(reservStatus);
                    default:
                        return BadRequest(reservStatus);
                }
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

        [Authorize]
        [HttpPost]
        [Route("Confirm-Reservation")]
        public IActionResult ConfirmReservation([FromBody] ConfirmReservationDTO reserv)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                    return Unauthorized("Invalid token.");

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var userId = jwtToken.Payload["userId"].ToString();

                var confirmStatus = _reservationService.ValidateReservation(reserv, Int32.Parse(userId));

                var bookingId = "";
                switch (confirmStatus)
                {
                    case "User not found.":
                        return NotFound(confirmStatus);
                    case "Reservation not found.":
                        return NotFound(confirmStatus);
                    case "First name unconfirmed.":
                        return BadRequest(confirmStatus);
                    case "Last name unconfirmed.":
                        return BadRequest(confirmStatus);
                    case "Email unconfirmed.":
                        return BadRequest(confirmStatus);
                    case "Phone unconfirmed.":
                        return BadRequest(confirmStatus);
                    case "Birthdate unconfirmed.":
                        return BadRequest(confirmStatus);
                    case "Total price unconfirmed.":
                        return BadRequest(confirmStatus);
                    case "Not enough tickets.":
                        return BadRequest(confirmStatus);
                    case "Reservation confirmed.":
                        bookingId = _reservationService.ReturnBookingId(Int32.Parse(userId));
                        break;
                }

                return Ok($"Reservation with id {bookingId} confirmed.");
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

        [Authorize]
        [HttpGet]
        [Route("Download-Tickets-by-BookingId")]
        public IActionResult DownloadTicketsByBookingId(string bookingId)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                    return Unauthorized("Invalid token.");

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var userId = jwtToken.Payload["userId"].ToString();

                var ticketsStatus = _reservationService.DownloadTicketsByBookingId(bookingId, Int32.Parse(userId));

                if (ticketsStatus.Equals("Reservation not found."))
                    return NotFound(ticketsStatus);
                
                return Ok(ticketsStatus);
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

        [Authorize]
        [HttpDelete]
        [Route("Cancel-Confirmed-Reservation")]
        public IActionResult DeleteReservations(string bookingId)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                    return Unauthorized("Invalid token.");

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var userId = jwtToken.Payload["userId"].ToString();

                var cancelStatus = _reservationService.DeleteReservationByBookingId(bookingId);

                if (cancelStatus == "Reservation not found.")
                    return NotFound(cancelStatus);
                else
                    return Ok(cancelStatus);
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

        [Authorize]
        [HttpDelete]
        [Route("Cancel-Unconfirmed-Reservation")]
        public IActionResult DeleteUnconfirmedReservations()
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                    return Unauthorized("Invalid token.");

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var userId = jwtToken.Payload["userId"].ToString();

                var cancelStatus = _reservationService.DeleteUnconfirmedReservation(Int32.Parse(userId));

                if (cancelStatus == "Reservation not found.")
                    return NotFound(cancelStatus);
                else
                    return Ok(cancelStatus);
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
