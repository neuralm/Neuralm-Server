import { MessageHandler } from '@/messaging/MessageHandler';

/**
 * Represents the CreateTrainingRoomResponseHandler class.
 */
export default class CreateTrainingRoomResponseHandler extends MessageHandler {
  /**
   * Initializes a new instance of the CreateTrainingRoomResponseHandler class.
   */
  constructor(resolve: any, reject: any) {
    super('Neuralm.Services.TrainingRoomService.Messages.CreateTrainingRoomResponse', resolve, reject);
  }
}
