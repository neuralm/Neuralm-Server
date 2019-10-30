import Guid from '@/helpers/Guid';
import { IMessage } from '@/interfaces/IMessage';

/**
 * Represents the Request class.
 */
export default class Request implements IMessage {
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
