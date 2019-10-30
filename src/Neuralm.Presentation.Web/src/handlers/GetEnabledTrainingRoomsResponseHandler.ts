import { MessageHandler } from '@/messaging/MessageHandler';

/**
 * Represents the GetEnabledTrainingRoomsResponseHandler class.
 */
export default class GetEnabledTrainingRoomsResponseHandler extends MessageHandler {
  /**
   * Initializes a new instance of the GetEnabledTrainingRoomsResponseHandler class.
   */
  constructor(resolve: any, reject: any) {
    super('Neuralm.Services.TrainingRoomService.Messages.GetEnabledTrainingRoomsResponse', resolve, reject);
  }
}
