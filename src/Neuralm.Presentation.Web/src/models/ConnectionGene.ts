import Organism from './Organism';
import Node from './Node';

/**
 * Represents the connection gene class.
 */
export default class ConnectionGene {
  public id: string;
  public organismId: string;
  public inNodeIdentifier: number;
  public outNodeIdentifier: number;
  public weight: number;
  public enabled: boolean;

  public in: Node | undefined;
  public out: Node | undefined;

  /**
   * Initializes a new instance of the ConnectionGene class.
   */
  constructor(id: string, organismId: string, inNodeIdentifier: number, outNodeIdentifier: number, weight: number, enabled: boolean) {
    this.id = id;
    this.organismId = organismId;
    this.inNodeIdentifier = inNodeIdentifier;
    this.outNodeIdentifier = outNodeIdentifier;
    this.weight = weight;
    this.enabled = enabled;
  }

  /**
   * Builds the connection gene structure.
   * Finds the input and output node in the passed organism.
   * @param organism The organism.
   */
  public buildStructure(organism: Organism): void {
    if (!this.enabled) {
      return;
    }

    if (this.organismId !== organism.id) {
      throw new Error('The buildStructure function was passed a different organism then its orgnasmId.');
    }

    this.in = organism.getNodeFromIdentifier(this.inNodeIdentifier);
    this.out = organism.getNodeFromIdentifier(this.outNodeIdentifier);
    this.out.addDependency(this);
  }

  /**
   * Gets the value of the input node times the weight.
   * @returns The value of the connection gene.
   */
  public getValue(): number {
    if (this.in === undefined) {
      throw new Error('The input node is undefined!');
    }
    return this.in.getValue() * this.weight;
  }
}
