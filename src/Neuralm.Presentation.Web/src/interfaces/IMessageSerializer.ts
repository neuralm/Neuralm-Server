/**
 * Represents the IMessageSerializer interface.
 */
export interface IMessageSerializer {
  /**
   * Serializes the message.
   * @param message The message object.
   * @returns The message serialized as Uint8Array.
   */
  serialize(message: object): Uint8Array;

  /**
   * Deserializes the Uint8Array into an object.
   * @param uint8Array The uint8Array.
   * @returns The message deserialized from the Uint8Array.
   */
  deserialize(uint8Array: Uint8Array): any;
}
