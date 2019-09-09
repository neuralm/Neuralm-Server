import Guid from '@/helpers/Guid';

/**
 * Represents the Request class.
 */
export default class Request {
  public Id: string;
  public DateTime: Date;

  /**
   *  Initializes the Request class.
   */
  constructor() {
    this.Id = Guid.newGuid().toString();
    this.DateTime = new Date();
  }
}
