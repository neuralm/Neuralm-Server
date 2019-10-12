import { BaseNetworkConnector } from './BaseNetworkConnector';

/**
 * Represents the websocket implementation of the BaseNetworkConnector class.
 */
export default class WSNetworkConnector extends BaseNetworkConnector {
 
  /**
   *Initializes a new instance of the WSNetworkConnector class.
   */
  constructor() {
    super();
  }
 
  public writeAsync(packet: Uint8Array): Promise<any> {
    throw new Error("Method not implemented.");
  }  
  
  public readAsync(packet: Uint8Array): Promise<any> {
    throw new Error("Method not implemented.");
  }
}
