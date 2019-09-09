import { AxiosResponse } from 'axios';

/**
 * Represents the base rest service class.
 */
export default class BaseRestService {
  /**
   * Handles the axios response.
   * @param response The axios response.
   */
  protected async handleResponse(response: AxiosResponse) {
    const data = response.data;
    if (response.status === 200) {
      return data;
    }
    if (response.status === 401) {
      // auto logout if 401 response returned from api
      localStorage.removeItem('user');
      location.reload(true);
    }
    const error = (data && data) || response.statusText;
    return Promise.reject(error);
  }
}
