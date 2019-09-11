import Request from './Request';

/**
 * Represents the AuthenticateRequest class.
 */
export default class AuthenticateRequest extends Request {
  public username: string;
  public password: string;
  public credentialTypeCode: string;

  /**
   * Initializes the AuthenticateRequest class.
   */
  constructor(username: string, password: string) {
    super();
    this.username = username;
    this.password = password;
    this.credentialTypeCode = 'Name';
  }
}
