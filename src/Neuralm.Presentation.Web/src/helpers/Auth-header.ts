import User from '@/models/user';

/**
 * The auth header is used to make authenticated HTTP requests
 * to the server api using JWT authentication.
 */
export function authHeader() {
  // return authorization header with jwt token
  if (!!localStorage.getItem('user')) {
    const userString: string =  localStorage.getItem('user')!;
    const user: User | null = JSON.parse(userString) as User;

    if (user && user.token) {
      return { Authorization: 'Bearer ' + user.token };
    }
  }
  return { };
}
