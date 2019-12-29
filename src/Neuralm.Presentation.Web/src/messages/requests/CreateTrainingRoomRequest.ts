import Request from './Request';
import TrainingRoom from '../../models/TrainingRoom';
import TrainingRoomSettings from '../../models/TrainingRoomSettings';
import Owner from '../../models/Owner';

/**
 * Represents the CreateTrainingRoomRequest class.
 */
export default class CreateTrainingRoomRequest extends Request {
    public owner: Owner;
    public name: string;
    public trainingRoomSettings: TrainingRoomSettings;

  /**
   * Initializes the CreateTrainingRoomRequest class.
   */
  constructor(trainingRoom: TrainingRoom) {
    super();
    this.owner = trainingRoom.owner;
    this.name = trainingRoom.name;
    this.trainingRoomSettings = trainingRoom.trainingRoomSettings;
  }
}
