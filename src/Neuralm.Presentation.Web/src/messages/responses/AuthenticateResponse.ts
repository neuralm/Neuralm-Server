import Response from './Response';
import Guid from '@/helpers/Guid';

/**
 * Represents the AuthenticateResponse interface.
 */
export default interface AuthenticateResponse extends Response {
  accessToken: string;
  userId: Guid;
}
