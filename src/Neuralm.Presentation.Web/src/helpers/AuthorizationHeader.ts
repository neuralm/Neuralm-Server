import User from '@/models/User';

/**
 * The auth header is used to make authenticated HTTP requests
 * to the server api using JWT authentication.
 */
export default function authHeader(): { Authorization: string } | {}  {
  // return authorization header with jwt token
  if (!!localStorage.getItem('user')) {
    const userString: string =  localStorage.getItem('user')!;
    const user: User | null = JSON.parse(userString) as User;

    if (user && user.AccessToken) {
      return { Authorization: 'Bearer ' + user.AccessToken };
    }
  }
  return { };
}
