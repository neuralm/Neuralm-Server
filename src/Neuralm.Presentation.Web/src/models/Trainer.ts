import Owner from './Owner';

/**
 * Represents the trainer class.
 */
export default interface Trainer {
  userId: string;
  user: Owner;
  trainingRoomId: string;
}
