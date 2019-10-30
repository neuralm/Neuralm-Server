import INeuralmMQClient from '@/interfaces/INeuralmMQClient';
import { MessageHandler, MessageHandlerDestructor } from './MessageHandler';
import { IMessage } from '@/interfaces/IMessage';
import { IMessageProcessor } from '@/interfaces/IMessageProcessor';
import { INetworkConnector } from '@/interfaces/INetworkConnector';

/**
 * Represents the NeuralmMQClient class.
 */
export default class NeuralmMQClient implements INeuralmMQClient {
  private readonly _messageProcessor: IMessageProcessor;
  private readonly _networkConnector: INetworkConnector;

  /**
   * Initializes a new instance of the NeuralmMQClient class.
   */
  constructor(
    messageProcessor: IMessageProcessor,
    networkConnector: INetworkConnector
  ) {
    this._messageProcessor = messageProcessor;
    this._networkConnector = networkConnector;
  }

  public sendMessage(message: IMessage): void {
    this._networkConnector.sendMessage(message);
  }
  public addHandler(messageHandler: MessageHandler): void {
    this._messageProcessor.addHandler(messageHandler);
  }
  public removeHandler(messageHandler: MessageHandler): void {
    this._messageProcessor.removeHandler(messageHandler);
  }
  public handlerDestructor(): MessageHandlerDestructor {
    return this._messageProcessor.handlerDestructor();
  }
}
