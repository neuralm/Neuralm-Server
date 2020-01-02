import Response from './Response';

/**
 * Represents the PostOrganismsScoreResponse class.
 */
export default class PostOrganismsScoreResponse extends Response {
  /**
   * Initializes a new instance of the PostOrganismsScoreResponse class.
   */
  constructor(id: string, requestId: string, dateTime: Date, message: string, success: boolean) {
    super(id, requestId, dateTime, message, success);
  }
}
