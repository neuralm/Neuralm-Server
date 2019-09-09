/**
 * Represents the Response interface.
 */
export default interface Response {
  Id: string;
  RequestId: string;
  DateTime: Date;
  Message: string;
  Success: boolean;
}
