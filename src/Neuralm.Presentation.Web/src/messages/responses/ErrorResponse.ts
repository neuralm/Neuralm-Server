import Response from './Response';

/**
 * Represents the ErrorResponse class.
 */
export default class ErrorResponse extends Response {
  public requestName: string;
  public responseName: string;

  /**
   * Initializes a new instance of the ErrorResponse class.
   */
  constructor(id: string, requestId: string, dateTime: Date, message: string, success: boolean, requestName: string, responseName: string) {
    super(id, requestId, dateTime, message, success);
    this.requestName = requestName;
    this.responseName = responseName;
  }
}
