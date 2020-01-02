/**
 * Represents the connection gene class.
 */
export default class ConnectionGene {
  public id: string;
  public organismId: string;
  public inNodeIdentifier: number;
  public outNodeIdentifier: number;
  public innovationNumber: number;
  public weight: number;
  public enabled: boolean;

  /**
   * Initializes a new instance of the ConnectionGene class.
   */
  constructor(id: string, organismId: string, inNodeIdentifier: number, outNodeIdentifier: number, innovationNumber: number, weight: number, enabled: boolean) {
    this.id = id;
    this.organismId = organismId;
    this.inNodeIdentifier = inNodeIdentifier;
    this.outNodeIdentifier = outNodeIdentifier;
    this.innovationNumber = innovationNumber;
    this.weight = weight;
    this.enabled = enabled;
  }
}
