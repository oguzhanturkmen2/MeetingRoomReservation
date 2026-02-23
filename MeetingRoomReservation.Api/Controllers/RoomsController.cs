using MeetingRoomReservation.Api.DTOs;
using MeetingRoomReservation.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _service;

        public RoomsController(IRoomService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoomDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(id);
        }
    }

}
