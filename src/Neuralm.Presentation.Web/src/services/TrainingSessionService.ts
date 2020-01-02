import ITrainingSessionService from '../interfaces/ITrainingSessionService';
import INeuralmMQClient from '../interfaces/INeuralmMQClient';
import StartTrainingSessionRequest from '../messages/requests/StartTrainingSessionRequest';
import StartTrainingSessionResponse from '../messages/responses/StartTrainingSessionResponse';
import StartTrainingSessionResponseHandler from '../handlers/StartTrainingSessionResponseHandler';
import { MessageHandler } from '../messaging/MessageHandler';
import EndTrainingSessionRequest from '../messages/requests/EndTrainingSessionRequest';
import EndTrainingSessionResponse from '../messages/responses/EndTrainingSessionResponse';
import EndTrainingSessionResponseHandler from '../handlers/EndTrainingSessionResponseHandler';
import GetOrganismsRequest from '../messages/requests/GetOrganismsRequest';
import GetOrganismsResponse from '../messages/responses/GetOrganismsResponse';
import GetOrganismsResponseHandler from '../handlers/GetOrganismsResponseHandler';
import PostOrganismsScoreRequest from '../messages/requests/PostOrganismsScoreRequest';
import PostOrganismsScoreResponse from '../messages/responses/PostOrganismsScoreResponse';
import PostOrganismsScoreResponseHandler from '../handlers/PostOrganismsScoreResponseHandler';

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

  public async getOrganisms(
    getOrganismsRequest: GetOrganismsRequest
  ): Promise<GetOrganismsResponse> {
    let messageHandler: MessageHandler;
    return new Promise<GetOrganismsResponse>((resolve, reject) => {
      messageHandler = new GetOrganismsResponseHandler(
        resolve,
        reject
      );
      this._neuralmMQClient.addHandler(messageHandler);
      this._neuralmMQClient.sendMessage(getOrganismsRequest);
    }).then((response) => {
      this._neuralmMQClient.removeHandler(messageHandler);
      return response;
    });
  }

  public async postOrganismsScores(
    postOrganismsScoreRequest: PostOrganismsScoreRequest
  ): Promise<PostOrganismsScoreResponse> {
    let messageHandler: MessageHandler;
    return new Promise<PostOrganismsScoreResponse>((resolve, reject) => {
      messageHandler = new PostOrganismsScoreResponseHandler(
        resolve,
        reject
      );
      this._neuralmMQClient.addHandler(messageHandler);
      this._neuralmMQClient.sendMessage(postOrganismsScoreRequest);
    }).then((response) => {
      this._neuralmMQClient.removeHandler(messageHandler);
      return response;
    });
  }
}
