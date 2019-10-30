import { INetworkConnector } from '@/interfaces/INetworkConnector';
import { IMessage } from '@/interfaces/IMessage';
import { IMessageSerializer } from '@/interfaces/IMessageSerializer';
import { IMessageProcessor } from '@/interfaces/IMessageProcessor';
import MessageConstructor from './MessageConstructor';
import { Message } from './Message';

/**
 * Represents the base implementation of the INetworkConnector interface.
 */
export abstract class BaseNetworkConnector implements INetworkConnector {
  private _isRunning: boolean = false;
  private readonly _messageConstructor: MessageConstructor;
  protected readonly _messsageSeriliazer: IMessageSerializer;
  protected readonly _messageProcessor: IMessageProcessor;

  public get isRunning(): boolean {
    return this._isRunning;
  }

  public abstract get isConnected(): boolean;

  /**
   * Initializes a new instance of the BaseNetworkConnetor class
   * @param messageSerializer the message serializer.
   * @param messageProcessor the message processor.
   */
  constructor(messageSerializer: IMessageSerializer, messageProcessor: IMessageProcessor) {
    this._messsageSeriliazer = messageSerializer;
    this._messageProcessor = messageProcessor;
    this._messageConstructor = new MessageConstructor(messageSerializer);
  }

  public abstract connectAsync(): Promise<boolean>;

  public start(): void {
    if (!this.isConnected) {
      throw new Error('The connector has not started yet. Call ConnectAsync() first.');
    }
    this._isRunning = true;
  }

  public stop(): void {
    this._isRunning = false;
  }

  public sendMessage<TMessage extends IMessage>(message: TMessage): void {
    const constructedMessage: Message = this._messageConstructor.constructMessage(message);
    console.log(constructedMessage);
    this.write(constructedMessage.Header);
    this.write(constructedMessage.Body);
  }

  /**
   * Writes the packet to the data transfer medium.
   * @param packet The packet.
   */
  protected abstract write(packet: Uint8Array): void;

  /**
   * Reads the packet from the data transfer medium.
   * @param packet The packet
   */
  protected abstract read(packet: Uint8Array): void;
}
