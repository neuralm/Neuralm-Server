import { MessageHandler } from '@/messaging/MessageHandler';

/**
 * Represents the AuthenticateResponseHandler class.
 */
export default class AuthenticateResponseHandler extends MessageHandler {
  /**
   * Initializes a new instance of the AuthenticateResponseHandler class.
   */
  constructor(resolve: any, reject: any) {
    super('Neuralm.Services.UserService.Messages.AuthenticateResponse', resolve, reject);
  }
}
