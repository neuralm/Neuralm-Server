import ConnectionGene from './ConnectionGene';

/**
 * Represents the node class.
 */
export default abstract class Node {
  public id: string;
  public nodeIdentifier: number;
  public dependencies: ConnectionGene[];

  /**
   * Initializes a new instance of the Node class.
   */
  constructor(id: string, nodeIdentifier: number) {
    this.id = id;
    this.nodeIdentifier = nodeIdentifier;
    this.dependencies = [];
  }

  /**
   * Gets the node value based on an activation function.
   * @returns The node value.
   */
  public getValue(): number {
    if (this.dependencies.length === 0) {
      return this.activationFunction(0);
    }

    let count: number = 0;
    let total: number = 0;

    for (const gene of this.dependencies) {
      total += gene.getValue();
      count++;
    }

    return this.activationFunction(total / count);
  }

  /**
   * Adds a dependency.
   * @param connectionGene The connection gene.
   */
  public addDependency(connectionGene: ConnectionGene): void {
    this.dependencies.push(connectionGene);
  }

  /**
   * The activation function.
   * Currently sigmoid.
   * @param x The value to activate.
   * @returns The activated value.
   */
  private activationFunction(x: number): number {
    // sigmoid
    const result: number = 1 / (1 + Math.exp(-x));
    // relu
    // const result: number = Math.max(0, x);
    return result;
  }
}
