import Response from './Response';
import Organism from '../../models/Organism';

/**
 * Represents the get organisms response class.
 */
export default class GetOrganismsResponse extends Response {
  public organisms: Organism[];

  /**
   * Initializes a new instance of the GetOrganismsResponse class.
   */
  constructor(id: string, requestId: string, dateTime: Date, message: string, success: boolean, organisms: Organism[]) {
    super(id, requestId, dateTime, message, success);
    this.organisms = organisms;
  }
}
