import User from './User';
import TrainingRoomSettings from './TrainingRoomSettings';

/**
 * Represents the training room class.
 */
export default interface TrainingRoom {
  id: string;
  name: string;
  owner: User;
  generation: number;
  trainingRoomSettings: TrainingRoomSettings;
}
