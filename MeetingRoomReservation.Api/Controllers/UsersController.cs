using MeetingRoomReservation.Api.Common;
using MeetingRoomReservation.Api.DTOs;
using MeetingRoomReservation.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(ApiResponse.Ok(data));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _service.GetByIdAsync(id);

            if (user == null)
                return NotFound(ApiResponse.Error("Kullanıcı bulunamadı."));

            return Ok(ApiResponse.Ok(user));
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateUserDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(ApiResponse.Ok(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateUpdateUserDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok(ApiResponse.Ok(null, "Kullanıcı güncellendi."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(ApiResponse.Ok(null, "Kullanıcı silindi."));
        }
    }

}
