import { MessageHandlerDestructor, MessageHandler } from '@/messaging/MessageHandler';
import { INetworkConnector } from './INetworkConnector';
import { IMessage } from './IMessage';

/**
 * Represents the IMessageProcessor interface.
 */
export interface IMessageProcessor {

  /**
   * Adds a message handler.
   * @param messageHandler the message handler. 
   */
  addHandler(messageHandler: MessageHandler): void;

  /**
   * Removes a message handler.
   * @param messageHandler the message handler.
   */
  removeHandler(messageHandler: MessageHandler): void;

  /**
   * Gets the message handler destructor.
   */
  messageHandlerDestructor(): MessageHandlerDestructor;

  /**
   * Processes the message asynchronously.
   * @param message the message.
   * @param networkConnector the network connector.
   */
  processMessageAsync(message: IMessage, networkConnector : INetworkConnector): Promise<void>;
}
