import { MessageHandlerDestructor, MessageHandler } from '@/messaging/MessageHandler';
import MessageWrapper from '@/messaging/MessageWrapper';

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
  handlerDestructor(): MessageHandlerDestructor;

  /**
   * Processes the message.
   * @param messageWrapper the message wrapper.
   */
  processMessage(messageWrapper: MessageWrapper): void;
}
