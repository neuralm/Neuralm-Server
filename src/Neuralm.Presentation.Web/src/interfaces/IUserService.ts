import AuthenticateRequest from '@/messages/requests/AuthenticateRequest';
import AuthenticateResponse from '@/messages/responses/AuthenticateResponse';
import RegisterRequest from '@/messages/requests/RegisterRequest';
import RegisterResponse from '@/messages/responses/RegisterResponse';

/**
 * Represents the user service interface.
 */
export default interface IUserService {
  /**
   * Logs in the user with the given user name and password.
   * @param authenticateRequest The authenticate request.
   * @returns The authenticate response.
   */
  login(authenticateRequest: AuthenticateRequest): Promise<AuthenticateResponse>;

  /**
   * Register the user.
   * @param user The user.
   * @returns A value whether the user was succesfully registered.
   */
  register(registerRequest: RegisterRequest): Promise<RegisterResponse>;
}
