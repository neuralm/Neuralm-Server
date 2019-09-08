import Guid from '@/helpers/Guid';

/**
 * Represents the Response interface.
 */
export default interface Response {
  id: Guid;
  requestId: Guid;
  dateTime: Date;
  message: string;
  success: boolean;
}
