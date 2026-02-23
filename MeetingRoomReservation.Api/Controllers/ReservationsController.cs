using MeetingRoomReservation.Api.Common;
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

        // POST /api/reservations
        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationDto dto)
        {
            var id = await _service.CreateAsync(dto);

            return Ok(ApiResponse.Ok(id, "Rezervasyon oluþturuldu."));
        }

        // GET /api/reservations
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();

            return Ok(ApiResponse.Ok(data));
        }

        // GET /api/reservations/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reservation = await _service.GetByIdAsync(id);

            if (reservation == null)
                return NotFound(ApiResponse.Error("Rezervasyon bulunamadý."));

            return Ok(ApiResponse.Ok(reservation));
        }

        // PUT /api/reservations/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateReservationDto dto)
        {
            await _service.UpdateAsync(id, dto);

            return Ok(ApiResponse.Ok(null, "Rezervasyon güncellendi."));
        }

        // DELETE /api/reservations/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(ApiResponse.Ok(null, "Rezervasyon silindi."));
        }

        // GET /api/reservations/conflicts?roomId=1&start=2026-03-01T10:00:00&end=2026-03-01T11:00:00
        [HttpGet("conflicts")]
        public async Task<IActionResult> CheckConflict(
            [FromQuery] int roomId,
            [FromQuery] DateTime start,
            [FromQuery] DateTime end)
        {
            var hasConflict = await _service.HasConflictAsync(roomId, start, end);

            return Ok(ApiResponse.Ok(hasConflict));
        }
    }
}