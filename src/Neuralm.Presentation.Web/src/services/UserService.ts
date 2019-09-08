// import authHeader from '../helpers/auth-header';
import IUserService from '@/interfaces/IUserService';
import User from '@/models/user';
import axios, { AxiosResponse } from 'axios';
import AuthenticateRequest from '@/messages/requests/AuthenticateRequest';
import AuthenticateResponse from '@/messages/responses/AuthenticateResponse';
import RegisterRequest from '@/messages/requests/RegisterRequest';
import RegisterResponse from '@/messages/responses/RegisterResponse';

/**
 * Represents the UserService class, an implementation of the IUserService interface.
 */
export default class UserService implements IUserService {
  public logout(): void {
    // remove user from local storage to log user out
    localStorage.removeItem('user');
  }

  public async login(authenticateRequest: AuthenticateRequest): Promise<AuthenticateResponse> {
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
        const user: User = { accessToken: response.accessToken, userId: response.userId };
        localStorage.setItem('user', JSON.stringify(user));
      }
      return response;
    });
  }

  public async register(registerRequest: RegisterRequest): Promise<RegisterResponse> {
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

  private async handleResponse(response: AxiosResponse) {
    const data = response.data;
    if (response.status === 200) {
      return data;
    }
    if (response.status === 401) {
      // auto logout if 401 response returned from api
      this.logout();
      location.reload(true);
    }
    const error = (data && data) || response.statusText;
    return Promise.reject(error);
  }
}
