import Response from './Response';
import TrainingRoom from '@/models/TrainingRoom';

/**
 * Represents the get enabled training rooms response class.
 */
export default class GetTrainingRoomResponse extends Response {
  public trainingRoom: TrainingRoom;

  /**
   * Initializes a new instance of the GetEnabledTrainingRoomsResponse class.
   */
  constructor(id: string, requestId: string, dateTime: Date, message: string, success: boolean, trainingRoom: TrainingRoom) {
    super(id, requestId, dateTime, message, success);
    this.trainingRoom = trainingRoom;
  }
}
