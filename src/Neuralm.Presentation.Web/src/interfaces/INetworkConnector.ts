import { IMessage } from './IMessage';

/**
 * Represents the INetworkConnector interface.
 */
export interface INetworkConnector {
  /**
   * Connects asynchronously.
   */
  connectAsync(): Promise<Boolean>;

  /**
   * Starts the network connector.
   */
  start(): void;

  /**
   * Stops the network connector.
   */
  stop(): void;

  /**
   * Sends a message asynchronously.
   * @param message the message.
   */
  sendMessageAsync<TMessage extends IMessage>(message: TMessage): Promise<any>; 
}
