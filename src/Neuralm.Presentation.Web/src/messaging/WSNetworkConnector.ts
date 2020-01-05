import { BaseNetworkConnector } from './BaseNetworkConnector';
import { IMessageSerializer } from '@/interfaces/IMessageSerializer';
import { IMessageProcessor } from '@/interfaces/IMessageProcessor';
import MessageWrapper from './MessageWrapper';
import { MessageHeader } from './MessageHeader';

/**
 * Represents the websocket implementation of the BaseNetworkConnector class.
 */
export default class WSNetworkConnector extends BaseNetworkConnector {
  private readonly _url: string;
  private _websocket!: WebSocket;
  private _isConnected: boolean = false;
  private _messageHeader: MessageHeader | null = null;
  private _maxAttempts: number = 200;

  public get isConnected(): boolean {
    return this._isConnected;
  }

  /**
   * Initializes a new instance of the WSNetworkConnector class.
   */
  constructor(
    messageSerializer: IMessageSerializer,
    messageProcessor: IMessageProcessor,
    url: string
  ) {
    super(messageSerializer, messageProcessor);
    this._url = url;
  }

  public connectAsync(): Promise<boolean> {
    return new Promise((resolve, reject) => {
      this._websocket = new WebSocket(this._url);
      this._websocket.binaryType = 'arraybuffer';
      this._websocket.onopen = (_) => {
        console.log('OPENED');
        this._isConnected = true;
        resolve(this._isConnected);
      };
      this._websocket.onclose = (_) => {
        console.log('CLOSED');
        this._isConnected = false;
      };
      this._websocket.onmessage = (evt) => {
        console.log('MESSAGE');
        this.read(evt.data);
      };
      this._websocket.onerror = (err) => {
        console.log('ERROR');
        reject(err);
      };
    });
  }

  public write(packet: Uint8Array): void {
    if (this.isConnected) {
      this._websocket.send(packet);
    } else {
      this.attemptWrite(packet);
    }
  }

  public read(packet: Uint8Array): void {
    if (!this.isConnected) {
      throw new Error('The network connector is not connected.');
    }
    if (this._messageHeader === null) {
      this._messageHeader = MessageHeader.tryParseHeader(packet);
      if (this._messageHeader === null) {
        console.log('Failed to parse header.');
        return;
      }
      console.log(this._messageHeader);
      return;
    }
    const messageWrapper: MessageWrapper = {
      name: this._messageHeader!.typeName,
      message: this._messsageSeriliazer.deserialize(packet)
    };
    this._messageProcessor.processMessage(messageWrapper);
    this._messageHeader = null;
  }

  private async attemptWrite(packet: Uint8Array): Promise<void> {
    let attempt: number = 0;
    while (this._maxAttempts > attempt) {
      if (this.isConnected) {
        this._websocket.send(packet);
        return Promise.resolve();
      } else {
        await new Promise((resolve) => setTimeout(resolve, 50));
        attempt++;
      }
    }
    throw new Error(`There was an attempt to write a message but the network connector is still not connected after ${attempt} attempts, waiting a total of ${attempt * 50}ms.`);
  }
}
