import { INetworkConnector } from "@/interfaces/INetworkConnector";
import { IMessage } from '@/interfaces/IMessage';
import { IMessageSerializer } from '@/interfaces/IMessageSerializer';
import { IMessageProcessor } from '@/interfaces/IMessageProcessor';

/**
 * Represents the base implementation of the INetworkConnector interface.
 */
export abstract class BaseNetworkConnector implements INetworkConnector {

  protected readonly _messsageSeriliazer: IMessageSerializer;
  protected readonly _messageProcessor: IMessageProcessor;

  /**
   * Initializes a new instance of the BaseNetworkConnetor class
   * @param messageSerializer the message serializer.
   * @param messageProcessor the message processor.
   */
  constructor(messageSerializer: IMessageSerializer, messageProcessor: IMessageProcessor) {
    this._messsageSeriliazer = messageSerializer;
    this._messageProcessor = messageProcessor;
  }

  public connectAsync(): Promise<Boolean> {
    throw new Error("Method not implemented.");
  }

  public start(): void {
    throw new Error("Method not implemented.");
  }

  public stop(): void {
    throw new Error("Method not implemented.");
  }

  public sendMessageAsync<TMessage extends IMessage>(message: TMessage): Promise<any> {
    
    throw new Error("Method not implemented.");
  }
  abstract writeAsync(packet: Uint8Array): Promise<any>;
  abstract readAsync(packet: Uint8Array): Promise<any>;
}
