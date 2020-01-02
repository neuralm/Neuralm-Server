import ConnectionGene from './ConnectionGene';

/**
 * Represents the organism class.
 */
export default class Organism {
  public id: string;
  public connectionGenes: ConnectionGene[];
  public score: number;
  public generation: number;
  public name: string;
  public inputNodes: Node[];
  public outputNodes: Node[];

  /**
   * Initializes a new instance of the Organism class.
   */
  constructor(id: string, connectionGenes: ConnectionGene[], score: number, generation: number, name: string, inputNodes: Node[], outputNodes: Node[]) {
    this.id = id;
    this.connectionGenes = connectionGenes;
    this.score = score;
    this.generation = generation;
    this.name = name;
    this.inputNodes = inputNodes;
    this.outputNodes = outputNodes;
  }
}
