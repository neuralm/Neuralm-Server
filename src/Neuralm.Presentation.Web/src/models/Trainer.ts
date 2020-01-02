import Owner from './Owner';

/**
 * Represents the trainer interface.
 */
export default interface Trainer {
  userId: string;
  user: Owner;
  trainingRoomId: string;
}
