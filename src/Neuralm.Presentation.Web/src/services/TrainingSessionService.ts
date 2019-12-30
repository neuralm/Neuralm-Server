import ITrainingSessionService from '../interfaces/ITrainingSessionService';
import INeuralmMQClient from '../interfaces/INeuralmMQClient';
import StartTrainingSessionRequest from '../messages/requests/StartTrainingSessionRequest';
import StartTrainingSessionResponse from '../messages/responses/StartTrainingSessionResponse';
import StartTrainingSessionResponseHandler from '../handlers/StartTrainingSessionResponseHandler';
import { MessageHandler } from '../messaging/MessageHandler';
import EndTrainingSessionRequest from '../messages/requests/EndTrainingSessionRequest';
import EndTrainingSessionResponse from '../messages/responses/EndTrainingSessionResponse';
import EndTrainingSessionResponseHandler from '../handlers/EndTrainingSessionResponseHandler';

/**
 * Represents the training session service class.
 */
export default class TrainingSessionService implements ITrainingSessionService {

  private readonly _neuralmMQClient: INeuralmMQClient;

  /**
   * Initializes a new instance of the TrainingSessionService class.
   */
  constructor(neuralmMQClient: INeuralmMQClient) {
    this._neuralmMQClient = neuralmMQClient;
  }

  public async startTrainingSession(
    startTrainingSessionRequest: StartTrainingSessionRequest
  ): Promise<StartTrainingSessionResponse> {
    let messageHandler: MessageHandler;
    return new Promise<StartTrainingSessionResponse>((resolve, reject) => {
      messageHandler = new StartTrainingSessionResponseHandler(
        resolve,
        reject
      );
      this._neuralmMQClient.addHandler(messageHandler);
      this._neuralmMQClient.sendMessage(startTrainingSessionRequest);
    }).then((response) => {
      this._neuralmMQClient.removeHandler(messageHandler);
      return response;
    });
  }

  public async endTrainingSession(
    endTrainingSessionRequest: EndTrainingSessionRequest
  ): Promise<EndTrainingSessionResponse> {
    let messageHandler: MessageHandler;
    return new Promise<EndTrainingSessionResponse>((resolve, reject) => {
      messageHandler = new EndTrainingSessionResponseHandler(
        resolve,
        reject
      );
      this._neuralmMQClient.addHandler(messageHandler);
      this._neuralmMQClient.sendMessage(endTrainingSessionRequest);
    }).then((response) => {
      this._neuralmMQClient.removeHandler(messageHandler);
      return response;
    });
  }
}
