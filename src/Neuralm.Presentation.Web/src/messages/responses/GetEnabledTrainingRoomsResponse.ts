import Response from './Response';
import TrainingRoom from '@/models/TrainingRoom';

/**
 * Represents the get enabled training rooms response class.
 */
export default interface GetEnabledTrainingRoomsResponse extends Response {
  TrainingRooms: TrainingRoom[];
}
