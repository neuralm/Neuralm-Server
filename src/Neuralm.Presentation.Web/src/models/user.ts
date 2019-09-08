import Guid from '@/helpers/Guid';

/**
 * Represents the user interface.
 */
export default interface User {
  userId: Guid;
  accessToken: string;
}
