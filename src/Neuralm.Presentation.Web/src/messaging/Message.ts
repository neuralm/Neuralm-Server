/**
 * Represents the Message class.
 */
export class Message {
  public readonly Header: Uint8Array;
  public readonly Body: Uint8Array;

  /**
   * Initializes a new instance of the Message class.
   */
  constructor(header: Uint8Array, body: Uint8Array) {
    this.Header = header;
    this.Body = body;
  }
}
