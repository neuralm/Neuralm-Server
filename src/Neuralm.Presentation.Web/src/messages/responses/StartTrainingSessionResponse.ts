import Response from './Response';
import TrainingSession from '../../models/TrainingSession';

/**
 * Represents the StartTrainingSessionResponse class.
 */
export default class StartTrainingSessionResponse extends Response {
  public trainingSession: TrainingSession;

  /**
   * Initializes a new instance of the StartTrainingSessionResponse class.
   */
  constructor(id: string, requestId: string, dateTime: Date, message: string, success: boolean, trainingSession: TrainingSession) {
    super(id, requestId, dateTime, message, success);
    this.trainingSession = trainingSession;
  }
}
