/**
 * Represents the MessageTypeCache class.
 */
class MessageTypeCache {
  private _messageTypes: { [key: string]: string };

  /**
   * Initializes a new instance of the MessageTypeCache class.
   */
  constructor() {
    this._messageTypes = {
      // Requests
      ['AuthenticateRequest']: 'Neuralm.Services.UserService.Messages.AuthenticateRequest',
      ['GetEnabledTrainingRoomsRequest']: 'Neuralm.Services.TrainingRoomService.Messages.GetEnabledTrainingRoomsRequest',
      ['RegisterRequest']: 'Neuralm.Services.UserService.Messages.RegisterRequest',
      ['GetTrainingRoomRequest']: 'Neuralm.Services.TrainingRoomService.Messages.GetTrainingRoomRequest',
      ['CreateTrainingRoomRequest']: 'Neuralm.Services.TrainingRoomService.Messages.CreateTrainingRoomRequest',
      ['PaginateTrainingRoomRequest']: 'Neuralm.Services.TrainingRoomService.Messages.PaginateTrainingRoomRequest',
      ['StartTrainingSessionRequest']: 'Neuralm.Services.TrainingRoomService.Messages.StartTrainingSessionRequest',
      ['EndTrainingSessionRequest']: 'Neuralm.Services.TrainingRoomService.Messages.EndTrainingSessionRequest',
      ['GetOrganismsRequest']: 'Neuralm.Services.TrainingRoomService.Messages.GetOrganismsRequest',
      ['PostOrganismsScoreRequest']: 'Neuralm.Services.TrainingRoomService.Messages.PostOrganismsScoreRequest',

      // Responses
      ['AuthenticateResponse']: 'Neuralm.Services.UserService.Messages.AuthenticateResponse',
      ['GetEnabledTrainingRoomsResponse']: 'Neuralm.Services.TrainingRoomService.Messages.GetEnabledTrainingRoomsResponse',
      ['RegisterResponse']: 'Neuralm.Services.UserService.Messages.RegisterResponse',
      ['GetTrainingRoomResponse']: 'Neuralm.Services.TrainingRoomService.Messages.GetTrainingRoomResponse',
      ['ErrorResponse']: 'Neuralm.Services.Common.Messages.ErrorResponse',
      ['CreateTrainingRoomResponse']: 'Neuralm.Services.TrainingRoomService.Messages.CreateTrainingRoomResponse',
      ['PaginateTrainingRoomResponse']: 'Neuralm.Services.TrainingRoomService.Messages.PaginateTrainingRoomResponse',
      ['StartTrainingSessionResponse']: 'Neuralm.Services.TrainingRoomService.Messages.StartTrainingSessionResponse',
      ['EndTrainingSessionResponse']: 'Neuralm.Services.TrainingRoomService.Messages.EndTrainingSessionResponse',
      ['GetOrganismsResponse']: 'Neuralm.Services.TrainingRoomService.Messages.GetOrganismsResponse',
      ['PostOrganismsScoreResponse']: 'Neuralm.Services.TrainingRoomService.Messages.PostOrganismsScoreResponse',
    };

    // instance.constructor.name
  }

  /**
   * Gets the message type by name.
   * @param messageName The message name.
   * @returns The message type.
   */
  public getMessageType(messageName: string): string {
    return this._messageTypes[messageName];
  }
}
const messageTypeCache: MessageTypeCache = new MessageTypeCache();
export default messageTypeCache;
