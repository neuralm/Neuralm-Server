import { MessageHandler } from '@/messaging/MessageHandler';

/**
 * Represents the GetTrainingRoomResponseHandler class.
 */
export default class GetTrainingRoomResponseHandler extends MessageHandler {
  /**
   * Initializes a new instance of the GetTrainingRoomResponseHandler class.
   */
  constructor(resolve: any, reject: any) {
    super('Neuralm.Services.TrainingRoomService.Messages.GetTrainingRoomResponse', resolve, reject);
  }
}
