import Guid from '@/helpers/Guid';

/**
 * Represents the Request class.
 */
export default class Request {
  public id: string;
  public dateTime: Date;

  /**
   *  Initializes the Request class.
   */
  constructor() {
    this.id = Guid.newGuid().toString();
    this.dateTime = new Date();
  }
}
