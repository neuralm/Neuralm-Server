import ITrainingRoomService from '@/interfaces/ITrainingRoomService';
import GetEnabledTrainingRoomsRequest from '../messages/requests/GetEnabledTrainingRoomsRequest';
import GetEnabledTrainingRoomsResponse from '../messages/responses/GetEnabledTrainingRoomsResponse';
import INeuralmMQClient from '@/interfaces/INeuralmMQClient';
import { MessageHandler } from '@/messaging/MessageHandler';
import GetEnabledTrainingRoomsResponseHandler from '@/handlers/GetEnabledTrainingRoomsResponseHandler';

/**
 * Represents the training room service class.
 */
export default class TrainingRoomService implements ITrainingRoomService {
  private readonly _neuralmMQClient: INeuralmMQClient;

  /**
   * Initializes a new instance of the TrainingRoomService class.
   */
  constructor(neuralmMQClient: INeuralmMQClient) {
    this._neuralmMQClient = neuralmMQClient;
  }

  public async getEnabledTrainingRooms(
    getEnabledTrainingRoomsRequest: GetEnabledTrainingRoomsRequest
  ): Promise<GetEnabledTrainingRoomsResponse> {
    return new Promise((resolve, reject) => {
      const messageHandler: MessageHandler = new GetEnabledTrainingRoomsResponseHandler(
        resolve,
        reject
      );
      this._neuralmMQClient.addHandler(messageHandler);
      this._neuralmMQClient.sendMessage(getEnabledTrainingRoomsRequest);
    });
  }
}
