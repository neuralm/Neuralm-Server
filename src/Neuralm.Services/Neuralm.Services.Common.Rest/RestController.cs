using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neuralm.Services.Common.Application.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Neuralm.Services.Common.Rest
{
    /// <summary>
    /// Represents the <see cref="RestController{TDto}"/> class.
    /// The abstract implementation of a rest api controller.
    /// </summary>
    /// <typeparam name="TDto">The dto type.</typeparam>
    [AllowAnonymous, ApiController, Route("[controller]")]
    public abstract class RestController<TDto> : ControllerBase where TDto : class
    {
        private readonly IService<TDto> _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestController{TDto}"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        protected RestController(IService<TDto> service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets the dto by id asynchronously.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Returns the dto if found with OK status code else NotFound status code.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> GetAsync(Guid id)
        {
            TDto dto = await _service.FindSingleOrDefaultAsync(id);
            return dto == null ? (IActionResult)new NotFoundResult() : new OkObjectResult(dto);
        }

        /// <summary>
        /// Gets all dtos asynchronously.
        /// </summary>
        /// <returns>Returns all dtos with status code OK.</returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual async Task<IActionResult> GetAllAsync()
        {
            return new OkObjectResult(await _service.GetAllAsync());
        }

        /// <summary>
        /// Updates the dto asynchronously.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>
        /// Returns NoContent upon successfully updating,
        /// returns Conflict if it failed to update; otherwise,
        /// Created with a LocationHeader to the created resource.
        /// </returns>
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> UpdateAsync(TDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());
            (bool success, Guid id, bool updated) = await _service.UpdateAsync(dto);
            if (!success)
                return new ConflictResult();
            if (!updated)
                return new CreatedAtRouteResult(new {id}, dto);
            return new NoContentResult();
        }

        /// <summary>
        /// Deletes the dto asynchronously.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>
        /// Returns NotFound if deleted successfully,
        /// if the dto was not found the NotFound result;
        /// otherwise, an InternalServerError status code.
        /// </returns>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> DeleteAsync(TDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());
            (bool success, bool found) = await _service.DeleteAsync(dto);
            if (!found)
                return new NotFoundResult();
            return success
                ? new NoContentResult()
                : new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Creates a dto asynchronously.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>
        /// Returns Created with a LocationHeader to the created resource if created successfully;
        /// otherwise, an InternalServerError status code.
        /// </returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> CreateAsync(TDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());
            (bool success, Guid id) = await _service.CreateAsync(dto);
            //string str = (this.UrlHelper ?? HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(context)).Action(this.ActionName, this.ControllerName, (object) this.RouteValues, request.Scheme, request.Host.ToUriComponent());
            //this.HttpContext.Response.Headers["Location"] = "";
            return success
                ? new CreatedAtRouteResult(new { id }, dto)
                //? new CreatedAtActionResult(nameof(GetAsync), this.GetType().Name, new { id }, dto)
                : (IActionResult)new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}
