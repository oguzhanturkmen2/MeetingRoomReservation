using MeetingRoomReservation.Api.Common;
using MeetingRoomReservation.Api.Entities;
using MeetingRoomReservation.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomReservation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentsController : ControllerBase
    {
        private readonly IEquipmentService _service;

        public EquipmentsController(IEquipmentService service)
        {
            _service = service;
        }

        // GET: api/equipments
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(ApiResponse.Ok(data));
        }

        // GET: api/equipments/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
                return NotFound(ApiResponse.Error("Ekipman bulunamadı."));

            return Ok(ApiResponse.Ok(data));
        }

        // POST: api/equipments
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Equipment dto)
        {
            await _service.CreateAsync(dto.Name, dto.Specification);
            return Ok(ApiResponse.Ok("Ekipman başarıyla eklendi."));
        }

        // PUT: api/equipments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Equipment dto)
        {
            await _service.UpdateAsync(id, dto.Name, dto.Specification);
            return Ok(ApiResponse.Ok("Ekipman güncellendi."));
        }

        // DELETE: api/equipments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(ApiResponse.Ok("Ekipman silindi."));
        }
    }

}
