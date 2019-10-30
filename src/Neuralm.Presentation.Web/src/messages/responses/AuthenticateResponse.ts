import Response from './Response';

/**
 * Represents the AuthenticateResponse interface.
 */
export default class AuthenticateResponse extends Response {
  public accessToken: string;
  public userId: string;

  /**
   * Initializes a new instance of the AuthenticateResponse class.
   */
  constructor(id: string, requestId: string, dateTime: Date, message: string, success: boolean, userId: string, accessToken: string) {
    super(id, requestId, dateTime, message, success);
    this.userId = userId;
    this.accessToken = accessToken;
  }
}
