import Response from './Response';
import TrainingRoom from '@/models/TrainingRoom';

/**
 * Represents the get enabled training rooms response class.
 */
export default class GetEnabledTrainingRoomsResponse extends Response {
  public trainingRooms: TrainingRoom[];

  /**
   * Initializes a new instance of the GetEnabledTrainingRoomsResponse class.
   */
  constructor(id: string, requestId: string, dateTime: Date, message: string, success: boolean, trainingRooms: TrainingRoom[]) {
    super(id, requestId, dateTime, message, success);
    this.trainingRooms = trainingRooms;
  }
}
