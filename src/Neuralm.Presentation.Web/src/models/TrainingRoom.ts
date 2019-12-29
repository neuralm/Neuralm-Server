import TrainingRoomSettings from './TrainingRoomSettings';
import Owner from './Owner';

/**
 * Represents the training room class.
 */
export default interface TrainingRoom {
  id: string;
  name: string;
  owner: Owner;
  generation: number;
  trainingRoomSettings: TrainingRoomSettings;
}
