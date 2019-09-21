import Response from './Response';

/**
 * Represents the AuthenticateResponse interface.
 */
export default interface AuthenticateResponse extends Response {
  accessToken: string;
  userId: string;
}
