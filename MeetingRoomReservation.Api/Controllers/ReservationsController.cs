using MeetingRoomReservation.Api.DTOs;
using MeetingRoomReservation.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _service;

        public ReservationsController(IReservationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(id);
        }
    }

}
