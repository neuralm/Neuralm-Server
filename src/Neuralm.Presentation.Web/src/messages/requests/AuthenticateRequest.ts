import Request from './Request';

/**
 * Represents the AuthenticateRequest class.
 */
export default class AuthenticateRequest extends Request {
  public Username: string;
  public Password: string;
  public CredentialTypeCode: string;

  /**
   * Initializes the AuthenticateRequest class.
   */
  constructor(username: string, password: string) {
    super();
    this.Username = username;
    this.Password = password;
    this.CredentialTypeCode = 'Name';
  }
}
