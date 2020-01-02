import Node from './Node';
import ConnectionGene from './ConnectionGene';

/**
 * Represents the input node class.
 */
export default class InputNode extends Node {
  private value!: number;

  /**
   * Initializes a new instance of the InputNode class.
   */
  constructor(id: string, nodeIdentifier: number) {
    super(id, nodeIdentifier);
  }

  /**
   * Gets the value that is set to this input node.
   * Overrides the default Node getValue implementation.
   * @returns The input node value.
   */
  public getValue(): number {
    return this.value;
  }

  /**
   * Sets the input node value.
   * @param value The value.
   */
  public setValue(value: number): void {
    this.value = value;
  }

  /**
   * The addDependency method is overriden to make sure an input node
   * does not get a dependency added.
   * @param connectionGene The connection gene.
   */
  public addDependency(connectionGene: ConnectionGene): void {
    throw new Error('Trying to add a dependency to an input!');
  }
}
