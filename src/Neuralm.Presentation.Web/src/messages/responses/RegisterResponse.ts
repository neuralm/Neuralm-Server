import Response from './Response';

/**
 * Represents the RegisterResponse class.
 */
export default class RegisterResponse extends Response {
  /**
   * Initializes a new instance of the RegisterResponse class.
   */
  constructor(id: string, requestId: string, dateTime: Date, message: string, success: boolean) {
    super(id, requestId, dateTime, message, success);
  }
}
