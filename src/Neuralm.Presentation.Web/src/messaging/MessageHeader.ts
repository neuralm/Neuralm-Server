/**
 * Represents the MessageHeader class.
 */
export class MessageHeader {
  private static readonly _textEncoder: TextEncoder = new TextEncoder();
  private static readonly _textDecoder: TextDecoder = new TextDecoder();
  public readonly bodySize: number;
  public typeName: string;

  /**
   * Initializes a new instance of the MessageHeader class.
   * @param messageBodySize The message body size.
   */
  constructor(messageBodySize: number) {
    this.bodySize = messageBodySize;
    this.typeName = '';
  }

  /**
   * Gets the header size.
   */
  public getHeaderSize(): number {
    const typeNameBytes = MessageHeader._textEncoder.encode(this.typeName);
    return 8 + typeNameBytes.length;
  }

  /**
   * Tries to parse the buffer into a Uint8Array.
   * @param buffer The Uint8Array buffer.
   * @returns The parsed message header or undefined if the message size is not the same as the parsed header size.
   */
  public static tryParseHeader(buffer: Uint8Array): MessageHeader | null {
    console.log('buffer', buffer);
    const dv: DataView = new DataView(buffer, 0);
    const headerSize: number = dv.getInt32(0, true);
    if (buffer.byteLength !== headerSize) {
      return null;
    }
    const bodySize: number = dv.getInt32(4, true);
    const header: MessageHeader = new MessageHeader(bodySize);
    header.typeName = MessageHeader._textDecoder.decode(
      buffer.slice(8, headerSize)
    );
    return header;
  }
}
