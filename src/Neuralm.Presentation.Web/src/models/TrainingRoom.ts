import User from './user';
import TrainingRoomSettings from './TrainingRoomSettings';

/**
 * Represents the training room class.
 */
export default interface TrainingRoom {
  Id: string;
  Name: string;
  Owner: User;
  Generation: number;
  TrainingRoomSettings: TrainingRoomSettings;
}
