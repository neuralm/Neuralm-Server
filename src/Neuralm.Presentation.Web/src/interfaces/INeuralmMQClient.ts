import { IMessage } from './IMessage';
import { MessageHandler, MessageHandlerDestructor } from '@/messaging/MessageHandler';

/**
 * Represents the INeuralmMQClient interface.
 * Used for communication with the Neuralm MessageQueue.
 */
export default interface INeuralmMQClient {
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
  handlerDestructor(): MessageHandlerDestructor;

  /**
   * Sends the message.
   * @param message The message.
   */
  sendMessage(message: IMessage): void;
}
