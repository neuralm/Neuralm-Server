using Neuralm.Services.Common.Rest;
using Neuralm.Services.TrainingRoomService.Application.Dtos;
using Neuralm.Services.TrainingRoomService.Application.Interfaces;

namespace Neuralm.Services.TrainingRoomService.Rest.Controllers
{
    public class TrainingRoomController : RestController<TrainingRoomDto>
    {
        private readonly ITrainingRoomService _trainingRoomService;

        public TrainingRoomController(ITrainingRoomService trainingRoomService) : base(trainingRoomService)
        {
            _trainingRoomService = trainingRoomService;
        }
    }
}
