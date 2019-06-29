namespace Neuralm.Application.Messages.Requests
{
    public class DisableTrainingRoomRequest : Request
    {
        public int TrainingRoomId { get; }
        public int UserId { get; }

        public DisableTrainingRoomRequest(int trainingRoomId, int userId)
        {
            TrainingRoomId = trainingRoomId;
            UserId = userId;
        }
    }
}
