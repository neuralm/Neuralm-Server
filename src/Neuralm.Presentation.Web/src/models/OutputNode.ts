import Node from './Node';

/**
 * Represents the output node class.
 */
export default class OutputNode extends Node {
  /**
   * Initializes a new instance of the OutputNode class.
   */
  constructor(id: string, nodeIdentifier: number) {
    super(id, nodeIdentifier);
  }
}
