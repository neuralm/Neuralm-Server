import { MessageHandler } from '@/messaging/MessageHandler';

/**
 * Represents the PostOrganismsScoreResponseHandler class.
 */
export default class PostOrganismsScoreResponseHandler extends MessageHandler {
  /**
   * Initializes a new instance of the PostOrganismsScoreResponseHandler class.
   */
  constructor(resolve: any, reject: any) {
    super('Neuralm.Services.TrainingRoomService.Messages.PostOrganismsScoreResponse', resolve, reject);
  }
}
