import Guid from '@/helpers/Guid';

/**
 * Represents the Request class.
 */
export default class Request {
  public id: Guid;
  public dateTime: Date;

  /**
   *  Initializes the Request class.
   */
  constructor() {
    this.id = Guid.newGuid();
    this.dateTime = new Date();
  }
}
