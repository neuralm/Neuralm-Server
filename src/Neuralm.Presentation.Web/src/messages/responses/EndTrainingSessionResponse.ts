import Response from './Response';

/**
 * Represents the EndTrainingSessionResponse class.
 */
export default class EndTrainingSessionResponse extends Response {
  /**
   * Initializes a new instance of the EndTrainingSessionResponse class.
   */
  constructor(id: string, requestId: string, dateTime: Date, message: string, success: boolean) {
    super(id, requestId, dateTime, message, success);
  }
}
