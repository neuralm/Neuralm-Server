import { MessageHandler } from '@/messaging/MessageHandler';

/**
 * Represents the PaginateTrainingRoomResponseHandler class.
 */
export default class PaginateTrainingRoomResponseHandler extends MessageHandler {
  /**
   * Initializes a new instance of the PaginateTrainingRoomResponseHandler class.
   */
  constructor(resolve: any, reject: any) {
    super('Neuralm.Services.TrainingRoomService.Messages.PaginateTrainingRoomResponse', resolve, reject);
  }
}
