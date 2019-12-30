import Response from './Response';

/**
 * Represents the CreateTrainingRoomResponse class.
 */
export default class CreateTrainingRoomResponse extends Response {
  /**
   * Initializes a new instance of the CreateTrainingRoomResponse class.
   */
  constructor(id: string, requestId: string, dateTime: Date, message: string, success: boolean) {
    super(id, requestId, dateTime, message, success);
  }
}
