import TrainingRoomSettings from './TrainingRoomSettings';
import Owner from './Owner';
import TrainingSession from './TrainingSession';
import Trainer from './Trainer';

/**
 * Represents the training room class.
 */
export default interface TrainingRoom {
  id: string;
  name: string;
  owner: Owner;
  generation: number;
  trainingSessions: TrainingSession[];
  authorizedTrainers: Trainer[];
  trainingRoomSettings: TrainingRoomSettings;
}
