import config from 'config';
import authHeader from '../helpers/auth-header';
import IUserService from '@/interfaces/IUserService';

/**
 * Represents the UserService class, an implementation of the IUserService interface.
 */
export default class UserService implements IUserService {
  /**
   * Initializes an instance of the UserService class.
   */
  public constructor() {

  }

  public logout(): void {
    // remove user from local storage to log user out
    localStorage.removeItem('user');
  }

  public login(username: String, password: String): User {
    const requestOptions = {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ username, password })
    };

    return fetch(`${config.apiUrl}/users/authenticate`, requestOptions)
    .then(this.handleResponse)
    .then(user => {
      // login successful if there's a jwt token in the response
      if (user.token) {
          // store user details and jwt token in local storage to keep user logged in between page refreshes
          localStorage.setItem('user', JSON.stringify(user));
      }

      return user;
    });
  }

  public register(user: any): any {
    const requestOptions = {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(user)
    };

    return fetch(`${config.apiUrl}/users/register`, requestOptions)
      .then(this.handleResponse);
  }

  private async handleResponse(response: Response) {
    return response.text().then(text => {
      const data = text && JSON.parse(text);
      if (!response.ok) {
        if (response.status === 401) {
          // auto logout if 401 response returned from api
          this.logout();
          location.reload(true);
        }

        const error = (data && data.message) || response.statusText;
        return Promise.reject(error);
      }

      return data;
    });
  }
}
