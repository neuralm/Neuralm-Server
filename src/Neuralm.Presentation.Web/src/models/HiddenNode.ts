import Node from './Node';

/**
 * Represents the hidden node class.
 */
export default class HiddenNode extends Node {
  /**
   * Initializes a new instance of the HiddenNode class.
   */
  constructor(id: string, nodeIdentifier: number) {
    super(id, nodeIdentifier);
  }
}
