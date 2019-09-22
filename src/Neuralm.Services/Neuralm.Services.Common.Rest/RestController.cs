using Microsoft.AspNetCore.Mvc;
using Neuralm.Services.Common.Application.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Neuralm.Services.Common.Rest
{
    [ApiController]
    [Route("[controller]")]
    public abstract class RestController<TDto> : ControllerBase where TDto : class
    {
        private readonly IService<TDto> _service;

        protected RestController(IService<TDto> service)
        {
            _service = service;
        }

        [HttpGet("{id:guid}")]
        public virtual async Task<IActionResult> GetAsync(Guid id)
        {
            TDto dto = await _service.FindSingleOrDefaultAsync(id);
            return dto == null ? (IActionResult)new NotFoundResult() : new OkObjectResult(dto);
        }

        [HttpGet("all")]
        public virtual async Task<IActionResult> GetAllAsync()
        {
            return new OkObjectResult(await _service.GetAllAsync());
        }

        [HttpPut("update")]
        public virtual async Task<IActionResult> UpdateAsync(TDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());
            (bool success, Guid id, bool updated) = await _service.UpdateAsync(dto);
            if (!success)
                return new ConflictResult();
            if (!updated)
                return new CreatedAtActionResult(nameof(GetAsync), GetType().Name, new { id }, "");
            return new NoContentResult();
        }

        [HttpDelete("delete")]
        public virtual async Task<IActionResult> DeleteAsync(TDto dto)
        {
            (bool success, bool found) = await _service.DeleteAsync(dto);
            if (!found)
                return new NotFoundResult();
            return success ? new NoContentResult() : new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}
