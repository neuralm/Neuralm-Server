import { IMessageProcessor } from "@/interfaces/IMessageProcessor";
import { MessageHandler, MessageHandlerDestructor } from './MessageHandler';
import { IMessage } from '@/interfaces/IMessage';
import { INetworkConnector } from '@/interfaces/INetworkConnector';

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
      this._messageHandlers.set(messageHandler.messageName, filteredMessageHandlers);
    }
  }

  public messageHandlerDestructor(): MessageHandlerDestructor {
    return (messageHandler: MessageHandler) => this.removeHandler(messageHandler);
  }

  public processMessageAsync(message: IMessage, networkConnector: INetworkConnector): Promise<void> {
    throw new Error("Method not implemented.");
  }

  private messageHandler(event: MessageEvent): void {
    const message: IMessage = JSON.parse(event.data);
    if (!this._messageHandlers.has(message.eventName)) {
      return;
    }
    if (message.success === false) {
      this._messageHandlers.get('errorHandler')!.forEach((eventHandler) => {
        eventHandler.callback(message.message);
      });
      this._messageHandlers.delete(message.eventName);
      return;
    }
    this._messageHandlers.get(message.eventName)!
      .forEach((eventHandler: EventHandler) => {
        eventHandler.callback(message.data);
    });
  }
}
