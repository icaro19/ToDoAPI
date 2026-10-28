using Application.DTOs.Tasks;
using Application.Interface;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskStatus = Domain.Enums.TaskStatus;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _service;

        public TasksController(
            ITaskService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(
                await _service.GetAllAsync());
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            Guid id)
        {
            return Ok(
                await _service.GetByIdAsync(id));
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(
            TaskStatus status)
        {
            return Ok(
                await _service.GetByStatusAsync(
                    status));
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateTaskDto dto)
        {
            var id =
                await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                id);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            UpdateTaskDto dto)
        {
            await _service.UpdateAsync(
                id,
                dto);

            return NoContent();
        }

        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(
            Guid id,
            UpdateTaskStatusDto dto)
        {
            await _service.ChangeStatusAsync(
                id,
                dto.Status);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            Guid id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
