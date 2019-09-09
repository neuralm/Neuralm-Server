import Request from './Request';

/**
 * Represents the RegisterRequest class.
 */
export default class RegisterRequest extends Request {
  public Username: string;
  public Password: string;
  public CredentialTypeCode: string;

  /**
   * Initializes the RegisterRequest class.
   */
  constructor(username: string, password: string) {
    super();
    this.Username = username;
    this.Password = password;
    this.CredentialTypeCode = 'Name';
  }
}
