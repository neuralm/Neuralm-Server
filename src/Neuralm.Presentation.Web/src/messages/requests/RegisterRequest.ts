import Request from './Request';

/**
 * Represents the RegisterRequest class.
 */
export default class RegisterRequest extends Request {
  public username: string;
  public password: string;
  public credentialTypeCode: string;

  /**
   * Initializes the RegisterRequest class.
   * @param username The user name.
   * @param password The password.
   */
  constructor(username: string, password: string) {
    super();
    this.username = username;
    this.password = password;
    this.credentialTypeCode = 'Name';
  }
}
