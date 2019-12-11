import IUserService from '@/interfaces/IUserService';
import AuthenticateRequest from '@/messages/requests/AuthenticateRequest';
import AuthenticateResponse from '@/messages/responses/AuthenticateResponse';
import RegisterRequest from '@/messages/requests/RegisterRequest';
import RegisterResponse from '@/messages/responses/RegisterResponse';
import INeuralmMQClient from '@/interfaces/INeuralmMQClient';
import { MessageHandler } from '@/messaging/MessageHandler';
import AuthenticateResponseHandler from '@/handlers/AuthenticateResponseHandler';
import RegisterResponseHandler from '@/handlers/RegisterResponseHandler';
import User from '@/models/User';

/**
 * Represents the UserService class, an implementation of the IUserService interface.
 */
export default class UserService implements IUserService {
  private readonly _neuralmMQClient: INeuralmMQClient;

  /**
   * Initializes a new instance of the UserService class.
   */
  constructor(neuralmMQClient: INeuralmMQClient) {
    this._neuralmMQClient = neuralmMQClient;
  }

  public async login(authenticateRequest: AuthenticateRequest): Promise<AuthenticateResponse> {
    let messageHandler: MessageHandler;
    return new Promise<AuthenticateResponse>((resolve, reject) => {
      messageHandler = new AuthenticateResponseHandler(resolve, reject);
      this._neuralmMQClient.addHandler(messageHandler);
      this._neuralmMQClient.sendMessage(authenticateRequest);
    }).then((response) => {
      // login successful if there's a jwt token in the response
      if (response.accessToken.length > 0) {
        // store user details and jwt token in local storage to keep user logged in between page refreshes
        const user: User = { username: authenticateRequest.username, accessToken: response.accessToken, userId: response.userId };
        localStorage.setItem('user', JSON.stringify(user));
      }
      this._neuralmMQClient.removeHandler(messageHandler);
      return response;
    });
  }

  public async register(registerRequest: RegisterRequest): Promise<RegisterResponse> {
    let messageHandler: MessageHandler;
    return new Promise<RegisterResponse>((resolve, reject) => {
      messageHandler = new RegisterResponseHandler(resolve, reject);
      this._neuralmMQClient.addHandler(messageHandler);
      this._neuralmMQClient.sendMessage(registerRequest);
    }).then((response) => {
      this._neuralmMQClient.removeHandler(messageHandler);
      return response;
    });
  }
}
