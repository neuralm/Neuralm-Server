import { IMessage } from './IMessage';

/**
 * Represents the INetworkConnector interface.
 */
export interface INetworkConnector {

  /**
   * Gets a value whether the connector is connected.
   */
  isConnected: boolean;

  /**
   * Gets a value whether the network connector is running.
   */
  isRunning: boolean;

  /**
   * Connects asynchronously.
   */
  connectAsync(): Promise<boolean>;

  /**
   * Starts the network connector.
   */
  start(): void;

  /**
   * Stops the network connector.
   */
  stop(): void;

  /**
   * Sends a message.
   * @param message the message.
   */
  sendMessage<TMessage extends IMessage>(message: TMessage): void;
}
