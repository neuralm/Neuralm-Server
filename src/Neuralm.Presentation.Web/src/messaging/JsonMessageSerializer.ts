import { IMessageSerializer } from '@/interfaces/IMessageSerializer';

/**
 * Represents the JsonMessageSerializer class.
 */
export default class JsonMessageSerializer implements IMessageSerializer {
  private readonly _textDecoder: TextDecoder;
  private readonly _textEncoder: TextEncoder;

  /**
   * Initializes a new instance of the JsonMessageSerializer class.
   */
  constructor() {
    this._textDecoder = new TextDecoder();
    this._textEncoder = new TextEncoder();
  }

  public serialize(message: object): Uint8Array {
    const json: string = JSON.stringify(JsonMessageSerializer.toPascalCase(message));
    return this._textEncoder.encode(json);
  }

  public deserialize(uint8Array: Uint8Array): any {
    const json: string = this._textDecoder.decode(uint8Array);
    return JsonMessageSerializer.toCamelCase(JSON.parse(json));
  }
  private static toPascalCase(jsonObject: any): any {
    if (jsonObject instanceof Array) {
      return jsonObject.map((value: any) => {
        if (typeof value === 'object') {
          value = JsonMessageSerializer.toPascalCase(value);
        }
        return value;
      });
    } else {
      const newObject: any = {};
      for (const origKey in jsonObject) {
        if (jsonObject.hasOwnProperty(origKey)) {
          const newKey = (origKey.charAt(0).toUpperCase() + origKey.slice(1) || origKey).toString();
          let value = jsonObject[origKey];
          if (value instanceof Array || (value !== null && value.constructor === Object)) {
            value = JsonMessageSerializer.toPascalCase(value);
          }
          newObject[newKey] = value;
        }
      }
      return newObject;
    }
  }

  private static toCamelCase(jsonObject: any): any {
    if (jsonObject instanceof Array) {
      return jsonObject.map((value: any) => {
        if (typeof value === 'object') {
          value = JsonMessageSerializer.toCamelCase(value);
        }
        return value;
      });
    } else {
      const newObject: any = {};
      for (const origKey in jsonObject) {
        if (jsonObject.hasOwnProperty(origKey)) {
          const newKey = (origKey.charAt(0).toLowerCase() + origKey.slice(1) || origKey).toString();
          let value = jsonObject[origKey];
          if (value instanceof Array || (value !== null && value.constructor === Object)) {
            value = JsonMessageSerializer.toCamelCase(value);
          }
          newObject[newKey] = value;
        }
      }
      return newObject;
    }
  }

}
