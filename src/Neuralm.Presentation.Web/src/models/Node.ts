/**
 * Represents the node class.
 */
export default class Node {
  public id: string;
  public layer: number;
  public nodeIdentifier: number;

  /**
   * Initializes a new instance of the Node class.
   */
  constructor(id: string, layer: number, nodeIdentifier: number) {
    this.id = id;
    this.layer = layer;
    this.nodeIdentifier = nodeIdentifier;
  }
}
