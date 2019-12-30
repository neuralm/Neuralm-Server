import { MessageHandler } from '@/messaging/MessageHandler';

/**
 * Represents the StartTrainingSessionResponseHandler class.
 */
export default class StartTrainingSessionResponseHandler extends MessageHandler {
  /**
   * Initializes a new instance of the StartTrainingSessionResponseHandler class.
   */
  constructor(resolve: any, reject: any) {
    super('Neuralm.Services.TrainingRoomService.Messages.StartTrainingSessionResponse', resolve, reject);
  }
}
