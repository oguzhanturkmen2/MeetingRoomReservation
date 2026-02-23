using MeetingRoomReservation.Api.Common;
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

        // POST /api/rooms
        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateRoomDto dto)
        {
            var id = await _service.CreateAsync(dto);

            return Ok(ApiResponse.Ok(id, "Oda oluþturuldu."));
        }

        // GET /api/rooms
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();

            return Ok(ApiResponse.Ok(data));
        }

        // GET /api/rooms/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var room = await _service.GetByIdAsync(id);

            if (room == null)
                return NotFound(ApiResponse.Error("Oda bulunamadý."));

            return Ok(ApiResponse.Ok(room));
        }

        // PUT /api/rooms/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateUpdateRoomDto dto)
        {
            await _service.UpdateAsync(id, dto);

            return Ok(ApiResponse.Ok(null, "Oda güncellendi."));
        }

        // DELETE /api/rooms/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(ApiResponse.Ok(null, "Oda silindi."));
        }
    }


}
