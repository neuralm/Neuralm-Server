import { MessageHandler } from '@/messaging/MessageHandler';

/**
 * Represents the RegisterResponseHandler class.
 */
export default class RegisterResponseHandler extends MessageHandler {
  /**
   * Initializes a new instance of the RegisterResponseHandler class.
   */
  constructor(resolve: any, reject: any) {
    super('Neuralm.Services.UserService.Messages.RegisterResponse', resolve, reject);
  }
}
