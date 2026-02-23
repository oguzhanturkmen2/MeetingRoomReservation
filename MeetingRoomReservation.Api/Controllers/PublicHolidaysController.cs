using MeetingRoomReservation.Api.Common;
using MeetingRoomReservation.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomReservation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicHolidaysController : ControllerBase
    {
        private readonly IPublicHolidayService _service;

        public PublicHolidaysController(IPublicHolidayService service)
        {
            _service = service;
        }

        // GET: api/publicholidays
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var holidays = await _service.GetAllAsync();

            return Ok(ApiResponse.Ok(holidays));
        }

        // GET: api/publicholidays/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var holiday = await _service.GetByIdAsync(id);

            if (holiday == null)
                return NotFound(ApiResponse.Error("Resmi tatil bulunamadı."));

            return Ok(ApiResponse.Ok(holiday));
        }

        // POST: api/publicholidays
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DateTime date)
        {
            await _service.CreateAsync(date);

            return Ok(ApiResponse.Ok("Resmi tatil başarıyla eklendi."));
        }

        // PUT: api/publicholidays/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DateTime date)
        {
            await _service.UpdateAsync(id, date);

            return Ok(ApiResponse.Ok("Resmi tatil güncellendi."));
        }

        // DELETE: api/publicholidays/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(ApiResponse.Ok("Resmi tatil silindi."));
        }
    }

}
