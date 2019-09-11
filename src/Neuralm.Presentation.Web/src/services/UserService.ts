import IUserService from '@/interfaces/IUserService';
import User from '@/models/User';
import axios from 'axios';
import AuthenticateRequest from '@/messages/requests/AuthenticateRequest';
import AuthenticateResponse from '@/messages/responses/AuthenticateResponse';
import RegisterRequest from '@/messages/requests/RegisterRequest';
import RegisterResponse from '@/messages/responses/RegisterResponse';
import BaseRestService from './BaseRestService';

/**
 * Represents the UserService class, an implementation of the IUserService interface.
 */
export default class UserService extends BaseRestService implements IUserService {
  public logout(): void {
    // remove user from local storage to log user out
    localStorage.removeItem('user');
  }

  public login(authenticateRequest: AuthenticateRequest): Promise<AuthenticateResponse> {
    const body: string = JSON.stringify(authenticateRequest);
    return axios({
      method: 'POST',
      url: `${process.env.VUE_APP_API_URL}/users/authenticate`,
      headers: {
        'Content-Type': 'application/json'
      },
      data: body
    })
    .then(this.handleResponse)
    .then((response: AuthenticateResponse) => {
      // login successful if there's a jwt token in the response
      if (response.accessToken.length > 0) {
        // store user details and jwt token in local storage to keep user logged in between page refreshes
        const user: User = { username: authenticateRequest.username, accessToken: response.accessToken, userId: response.userId };
        localStorage.setItem('user', JSON.stringify(user));
      }
      return response;
    });
  }

  public register(registerRequest: RegisterRequest): Promise<RegisterResponse> {
    const body: string = JSON.stringify(registerRequest);
    return axios({
      method: 'POST',
      url: `${process.env.VUE_APP_API_URL}/users/register`,
      headers: {
        'Content-Type': 'application/json'
      },
      data: body
    })
    .then(this.handleResponse);
  }
}
