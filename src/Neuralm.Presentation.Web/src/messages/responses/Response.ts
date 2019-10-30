/**
 * Represents the Response class.
 */
export default class Response {
  public id: string;
  public requestId: string;
  public dateTime: Date;
  public message: string;
  public success: boolean;

  /**
   * Initializes a new instance of the Response class.
   */
  constructor(id: string, requestId: string, dateTime: Date, message: string, success: boolean) {
    this.id = id;
    this.requestId = requestId;
    this.dateTime = dateTime;
    this.message = message;
    this.success = success;
  }
}
