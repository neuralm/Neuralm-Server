import Request from './Request';

/**
 * Represents the GetTrainingRoomRequest class.
 */
export default class GetTrainingRoomRequest extends Request {
  public getId: string;

  /**
   * Initializes the GetTrainingRoomRequest class.
   * @param getId The get id.
   */
  constructor(getId: string) {
    super();
    this.getId = getId;
  }
}
