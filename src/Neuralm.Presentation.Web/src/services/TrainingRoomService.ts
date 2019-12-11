import ITrainingRoomService from '@/interfaces/ITrainingRoomService';
import GetEnabledTrainingRoomsRequest from '../messages/requests/GetEnabledTrainingRoomsRequest';
import GetEnabledTrainingRoomsResponse from '../messages/responses/GetEnabledTrainingRoomsResponse';
import INeuralmMQClient from '@/interfaces/INeuralmMQClient';
import { MessageHandler } from '@/messaging/MessageHandler';
import GetEnabledTrainingRoomsResponseHandler from '@/handlers/GetEnabledTrainingRoomsResponseHandler';
import GetTrainingRoomResponse from '../messages/responses/GetTrainingRoomResponse';
import GetTrainingRoomRequest from '../messages/requests/GetTrainingRoomRequest';
import GetTrainingRoomResponseHandler from '../handlers/GetTrainingRoomResponseHandler';

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
    let messageHandler: MessageHandler;
    return new Promise<GetEnabledTrainingRoomsResponse>((resolve, reject) => {
      messageHandler = new GetEnabledTrainingRoomsResponseHandler(
        resolve,
        reject
      );
      this._neuralmMQClient.addHandler(messageHandler);
      this._neuralmMQClient.sendMessage(getEnabledTrainingRoomsRequest);
    }).then((response) => {
        this._neuralmMQClient.removeHandler(messageHandler);
        return response;
    });
  }

  public async getTrainingRoom(
    getTrainingRoomRequest: GetTrainingRoomRequest
  ): Promise<GetTrainingRoomResponse> {
    let messageHandler: MessageHandler;
    return new Promise<GetTrainingRoomResponse>((resolve, reject) => {
      messageHandler = new GetTrainingRoomResponseHandler(
        resolve,
        reject
      );
      this._neuralmMQClient.addHandler(messageHandler);
      this._neuralmMQClient.sendMessage(getTrainingRoomRequest);
    }).then((response) => {
        this._neuralmMQClient.removeHandler(messageHandler);
        return response;
    });
  }
}
