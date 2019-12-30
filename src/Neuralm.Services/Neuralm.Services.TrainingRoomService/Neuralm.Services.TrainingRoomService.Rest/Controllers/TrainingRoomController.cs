using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neuralm.Services.Common.Rest;
using Neuralm.Services.TrainingRoomService.Application.Interfaces;
using Neuralm.Services.TrainingRoomService.Messages;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;

namespace Neuralm.Services.TrainingRoomService.Rest.Controllers
{
    public class TrainingRoomController : RestController<TrainingRoomDto>
    {
        private readonly ITrainingRoomService _trainingRoomService;
        private readonly ITrainingSessionService _trainingSessionService;

        public TrainingRoomController(ITrainingRoomService trainingRoomService, ITrainingSessionService trainingSessionService) : base(trainingRoomService)
        {
            _trainingRoomService = trainingRoomService;
            _trainingSessionService = trainingSessionService;
        }

        /// <summary>
        /// Starts a training sessions asynchronously.
        /// </summary>
        /// <param name="startTrainingSessionRequest">The start training session request object.</param>
        /// <returns>
        /// Returns OkObject with response.
        /// </returns>
        [HttpPost("startTrainingSession")]
        public async Task<IActionResult> StartTrainingSessionAsync(StartTrainingSessionRequest startTrainingSessionRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());
            StartTrainingSessionResponse response = await _trainingSessionService.StartTrainingSessionAsync(startTrainingSessionRequest);
            return new OkObjectResult(response);
        }
        
        /// <summary>
        /// Ends a training sessions asynchronously.
        /// </summary>
        /// <param name="endTrainingSessionRequest">The end training session request object.</param>
        /// <returns>
        /// Returns OkObject with response.
        /// </returns>
        [HttpPut("endTrainingSession")]
        public async Task<IActionResult> EndTrainingSessionsAsync(EndTrainingSessionRequest endTrainingSessionRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());
            EndTrainingSessionResponse response = await _trainingSessionService.EndTrainingSessionAsync(endTrainingSessionRequest);
            return new OkObjectResult(response);
        }
    }
}
