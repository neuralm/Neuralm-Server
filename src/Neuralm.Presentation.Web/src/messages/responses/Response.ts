/**
 * Represents the Response interface.
 */
export default interface Response {
  id: string;
  requestId: string;
  dateTime: Date;
  message: string;
  success: boolean;
}
