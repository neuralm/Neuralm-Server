import ConnectionGene from './ConnectionGene';
import Node from './Node';
import InputNode from './InputNode';
import OutputNode from './OutputNode';
import HiddenNode from './HiddenNode';
import Guid from '../helpers/Guid';

/**
 * Represents the organism class.
 */
export default class Organism {
  private hiddenNodes: HiddenNode[];

  public id: string;
  public connectionGenes: ConnectionGene[];
  public score: number;
  public generation: number;
  public name: string;
  public inputNodes: InputNode[];
  public outputNodes: OutputNode[];

  /**
   * Initializes a new instance of the Organism class.
   */
  constructor(id: string, connectionGenes: ConnectionGene[], score: number, generation: number, name: string, inputNodes: InputNode[], outputNodes: OutputNode[]) {
    this.id = id;
    this.connectionGenes = connectionGenes;
    this.score = score;
    this.generation = generation;
    this.name = name;
    this.inputNodes = inputNodes;
    this.outputNodes = outputNodes;
    this.hiddenNodes = [];
    this.initialize();
  }

  /**
   * Initialize the organism so it can be used.
   * This will build the internal connection gene structure.
   */
  public initialize(): void {
    for (const connectionGene of this.connectionGenes) {
      connectionGene.buildStructure(this);
    }
  }

  /**
   * Evaluates the inputs.
   * @param inputs The inputs.
   * @returns Returns the outputs.
   */
  public evaluate(inputs: number[]): number[] {
    if (inputs.length !== this.inputNodes.length) {
      throw new Error(`inputs length ( ${inputs.length} ) should match inputnodes length (${this.inputNodes.length} )`);
    }

    for (let i = 0; i < inputs.length; i++) {
      this.inputNodes[i].setValue(inputs[i]);
    }

    const outputs: number[] = new Array(this.outputNodes.length);
    for (let i = 0; i < this.outputNodes.length; i++) {
      outputs[i] = this.outputNodes[i].getValue();
    }

    return outputs;
  }

  /**
   * Gets a node by id.
   * @param nodeIdentifier the node identifier.
   * @returns Returns either an InputNode, OutputNode, or HiddenNode depending on
   * if the node identifier is found in either the inputNodes, outputNodes, or hiddenNodes arrays.
   * If no node was found a new hidden node is created and returned.
   */
  public getNodeFromIdentifier(nodeIdentifier: number): Node {
    const inputNode: InputNode | undefined = this.inputNodes.find((n) => n.nodeIdentifier === nodeIdentifier);
    if (inputNode !== undefined) {
      return inputNode;
    }

    const outputNode: OutputNode | undefined = this.outputNodes.find((n) => n.nodeIdentifier === nodeIdentifier);
    if (outputNode !== undefined) {
      return outputNode;
    }

    const hiddenNode: HiddenNode | undefined = this.hiddenNodes.find((n) => n.nodeIdentifier === nodeIdentifier);
    if (hiddenNode !== undefined) {
      return hiddenNode;
    } else {
      return this.createAndAddNode(nodeIdentifier);
    }
  }

  /**
   * Gets the hidden nodes.
   * @returns Returns the hidden nodes.
   */
  public getHiddenNodes(): HiddenNode[] {
    return this.hiddenNodes;
  }

  /**
   * Creates and adds a node to the hidden node list.
   * @param nodeIdentifier The node identifier.
   * @returns Returns the created node.
   */
  private createAndAddNode(nodeIdentifier: number): Node {
    const hiddenNode: HiddenNode = new HiddenNode(Guid.newGuid().toString(), nodeIdentifier);
    this.hiddenNodes.push(hiddenNode);
    return hiddenNode;
  }
}
