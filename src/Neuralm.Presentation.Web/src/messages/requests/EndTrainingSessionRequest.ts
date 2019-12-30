import Request from './Request';

/**
 * Represents the EndTrainingSessionRequest class.
 */
export default class EndTrainingSessionRequest extends Request {
  public trainingSessionId: string;

  /**
   * Initializes the EndTrainingSessionRequest class.
   * @param userId The user id.
   * @param trainingRoomId The training room id.
   */
  constructor(trainingSessionId: string) {
    super();
    this.trainingSessionId = trainingSessionId;
  }
}
