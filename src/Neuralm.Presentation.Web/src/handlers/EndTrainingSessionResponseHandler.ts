import { MessageHandler } from '@/messaging/MessageHandler';

/**
 * Represents the EndTrainingSessionResponseHandler class.
 */
export default class EndTrainingSessionResponseHandler extends MessageHandler {
  /**
   * Initializes a new instance of the EndTrainingSessionResponseHandler class.
   */
  constructor(resolve: any, reject: any) {
    super('Neuralm.Services.TrainingRoomService.Messages.EndTrainingSessionResponse', resolve, reject);
  }
}
