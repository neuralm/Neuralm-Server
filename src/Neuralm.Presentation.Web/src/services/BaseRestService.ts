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
    const data = BaseRestService.toCamelCase(response.data);
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

  private static toCamelCase(jsonObject: any): any {
    if (jsonObject instanceof Array) {
      return jsonObject.map((value: any) => {
        if (typeof value === 'object') {
          value = BaseRestService.toCamelCase(value);
        }
        return value;
      });
    } else {
      const newObject: any = {};
      for (const origKey in jsonObject) {
        if (jsonObject.hasOwnProperty(origKey)) {
          const newKey = (origKey.charAt(0).toLowerCase() + origKey.slice(1) || origKey).toString();
          let value = jsonObject[origKey];
          if (value instanceof Array || (value !== null && value.constructor === Object)) {
            value = BaseRestService.toCamelCase(value);
          }
          newObject[newKey] = value;
        }
      }
      return newObject;
    }
  }

  private static toPascalCase(jsonObject: any): any {
    if (jsonObject instanceof Array) {
      return jsonObject.map((value: any) => {
        if (typeof value === 'object') {
          value = BaseRestService.toPascalCase(value);
        }
        return value;
      });
    } else {
      const newObject: any = {};
      for (const origKey in jsonObject) {
        if (jsonObject.hasOwnProperty(origKey)) {
          const newKey = (origKey.charAt(0).toUpperCase() + origKey.slice(1) || origKey).toString();
          let value = jsonObject[origKey];
          if (value instanceof Array || (value !== null && value.constructor === Object)) {
            value = BaseRestService.toPascalCase(value);
          }
          newObject[newKey] = value;
        }
      }
      return newObject;
    }
  }
}
