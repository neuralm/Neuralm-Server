import Request from './Request';

/**
 * Represents the StartTrainingSessionRequest class.
 */
export default class StartTrainingSessionRequest extends Request {
  public userId: string;
  public trainingRoomId: string;

  /**
   * Initializes the StartTrainingSessionRequest class.
   * @param userId The user id.
   * @param trainingRoomId The training room id.
   */
  constructor(userId: string, trainingRoomId: string) {
    super();
    this.userId = userId;
    this.trainingRoomId = trainingRoomId;
  }
}
