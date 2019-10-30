import { Dictionary } from 'vuex';

/**
 * Represents the MessageTypeCache class.
 */
class MessageTypeCache {
  private _messageTypes: Dictionary<string>;

  /**
   * Initializes a new instance of the MessageTypeCache class.
   */
  constructor() {
    this._messageTypes = {
      // Requests
      ['AuthenticateRequest']: 'Neuralm.Services.UserService.Messages.AuthenticateRequest',
      ['GetEnabledTrainingRoomsRequest']: 'Neuralm.Services.TrainingRoomService.Messages.GetEnabledTrainingRoomsRequest',
      ['RegisterRequest']: 'Neuralm.Services.UserService.Messages.RegisterRequest',

      // Responses
      ['AuthenticateResponse']: 'Neuralm.Services.UserService.Messages.AuthenticateResponse',
      ['GetEnabledTrainingRoomsResponse']: 'Neuralm.Services.TrainingRoomService.Messages.GetEnabledTrainingRoomsResponse',
      ['RegisterResponse']: 'Neuralm.Services.UserService.Messages.RegisterResponse',
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
