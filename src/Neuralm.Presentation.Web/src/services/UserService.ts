// import authHeader from '../helpers/auth-header';
import IUserService from '@/interfaces/IUserService';
import User from '@/models/user';

/**
 * Represents the UserService class, an implementation of the IUserService interface.
 */
export default class UserService implements IUserService {
  public logout(): void {
    // remove user from local storage to log user out
    localStorage.removeItem('user');
  }

  public login(username: string, password: string): Promise<User> {
    const requestOptions = {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ username, password })
    };

    return fetch(`${process.env.API_URL}/users/authenticate`, requestOptions)
    .then(this.handleResponse)
    .then((user: User) => {
      // login successful if there's a jwt token in the response
      if (user.token) {
        // store user details and jwt token in local storage to keep user logged in between page refreshes
        localStorage.setItem('user', JSON.stringify(user));
      }

      return user;
    });
  }

  public register(user: User): Promise<boolean> {
    const requestOptions = {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(user)
    };

    return fetch(`${process.env.API_URL}/users/register`, requestOptions)
      .then(this.handleResponse);
  }

  private async handleResponse(response: Response) {
    return response.text().then((text: string) => {
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
