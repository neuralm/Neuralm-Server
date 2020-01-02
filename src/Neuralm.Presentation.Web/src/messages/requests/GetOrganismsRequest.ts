import Request from './Request';

/**
 * Represents the get organisms request class.
 */
export default class GetOrganismsRequest extends Request {
  public userId: string;
  public trainingSessionId: string;
  public amount: number;

  /**
   * Initializes the GetOrganismsRequest class.
   */
  constructor(userId: string, trainingSessionId: string, amount: number) {
    super();
    this.userId = userId;
    this.trainingSessionId = trainingSessionId;
    this.amount = amount;
  }
}
