import { MessageHandler } from '@/messaging/MessageHandler';

/**
 * Represents the GetOrganismsResponseHandler class.
 */
export default class GetOrganismsResponseHandler extends MessageHandler {
  /**
   * Initializes a new instance of the GetOrganismsResponseHandler class.
   */
  constructor(resolve: any, reject: any) {
    super('Neuralm.Services.TrainingRoomService.Messages.GetOrganismsResponse', resolve, reject);
  }
}
