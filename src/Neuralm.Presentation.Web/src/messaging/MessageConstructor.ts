import { IMessageSerializer } from '@/interfaces/IMessageSerializer';
import { IMessage } from '@/interfaces/IMessage';
import messageTypeCache from './MessageTypeCache';
import { Message } from './Message';

/**
 * Represents the MessageConstructor class.
 */
export default class MessageConstructor {
  private readonly _textEncoder: TextEncoder = new TextEncoder();
  private readonly _messageSerializer: IMessageSerializer;

  /**
   * Initializes a new instance of the MessageConstructor class.
   */
  constructor(messageSerializer: IMessageSerializer) {
    this._messageSerializer = messageSerializer;
  }

  public constructMessage(message: IMessage): Message {
    const body: Uint8Array = this._messageSerializer.serialize(message);
    const bodySizeBytes: Uint8Array = new Uint8Array(4);
    let dv: DataView = new DataView(bodySizeBytes.buffer);
    dv.setInt32(0, body.length, true);
    const typeName: string = messageTypeCache.getMessageType(message.constructor.name);
    const typeNameBytes: Uint8Array = this._textEncoder.encode(typeName);
    const headerSizeBytes: Uint8Array = new Uint8Array(4);
    dv = new DataView(headerSizeBytes.buffer);
    dv.setInt32(0, 8 + typeNameBytes.length, true);
    const header: Uint8Array = this.concat(this.concat(headerSizeBytes, bodySizeBytes), typeNameBytes);
    return new Message(header, body);
  }

  private concat(a: Uint8Array, b: Uint8Array): Uint8Array {
    const tmp = new Uint8Array(a.length + b.length);
    tmp.set(a, 0);
    tmp.set(b, a.length);
    return tmp;
  }
}
