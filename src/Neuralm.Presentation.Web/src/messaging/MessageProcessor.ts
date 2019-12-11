import { IMessageProcessor } from '@/interfaces/IMessageProcessor';
import { MessageHandler, MessageHandlerDestructor } from './MessageHandler';
import MessageWrapper from './MessageWrapper';

/**
 * Represents the MessageProcessor class.
 */
export default class MessageProcessor implements IMessageProcessor {
  private readonly _messageHandlers: Map<string, MessageHandler[]>;

  /**
   * Initializes a new instance of the MessageProcessor class.
   */
  constructor() {
    this._messageHandlers = new Map<string, MessageHandler[]>();
  }

  public addHandler(messageHandler: MessageHandler): void {
    if (this._messageHandlers.has(messageHandler.messageName)) {
      this._messageHandlers.get(messageHandler.messageName)!.push(messageHandler);
    } else {
      this._messageHandlers.set(messageHandler.messageName, [messageHandler]);
    }
  }

  public removeHandler(messageHandler: MessageHandler): void {
    if (this._messageHandlers.has(messageHandler.messageName)) {
      const filteredMessageHandlers: MessageHandler[] = this._messageHandlers
        .get(messageHandler.messageName)!
        .filter((handler: MessageHandler) => handler !== messageHandler);
      if (filteredMessageHandlers.length === 0) {
        this._messageHandlers.delete(messageHandler.messageName);
      } else {
        this._messageHandlers.set(messageHandler.messageName, filteredMessageHandlers);
      }
    }
    console.log(this._messageHandlers);
  }

  public handlerDestructor(): MessageHandlerDestructor {
    return (messageHandler: MessageHandler) => this.removeHandler(messageHandler);
  }

  public processMessage(messageWrapper: MessageWrapper): void {
    if (!this._messageHandlers.has(messageWrapper.name)) {
      console.log(`A message handler for '${messageWrapper.name}' was not found.`);
      return;
    }

    // TODO: fix errorhandling
    // if (!messageWrapper.message.success) {
    //   this._messageHandlers.get('errorHandler')!.forEach((eventHandler) => {
    //     eventHandler.callback(messageWrapper.message);
    //   });
    //   this._messageHandlers.delete(messageWrapper.name);
    //   return;
    // }
    this._messageHandlers.get(messageWrapper.name)!
      .forEach((eventHandler: MessageHandler) => {
        eventHandler.callback(messageWrapper.message);
    });
  }
}
