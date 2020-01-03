import Organism from '@/models/Organism';
import Guid from '@/helpers/Guid';
import ConnectionGene from '@/models/ConnectionGene';
import InputNode from '@/models/InputNode';
import OutputNode from '@/models/OutputNode';

describe('Organism', () => {
  test.each`
  inA   | inB   | inC   | expected
  ${1}  | ${2}  | ${3}  | ${0.8328355518722748}
  ${3}  | ${2}  | ${1}  | ${0.8377227092646183}
  ${0}  | ${0}  | ${0}  | ${0.5415704832167999}
  ${-1} | ${-2} | ${-3} | ${0.2188253499884587}
  ${-3} | ${-2} | ${-1} | ${0.21281378940936574}
  `('evaluate', ({inA, inB, inC, expected}): void => {
    const organismId: string = Guid.newGuid().toString();
    const inputNodes: InputNode[] = [
      new InputNode(Guid.newGuid().toString(), 0),
      new InputNode(Guid.newGuid().toString(), 1),
      new InputNode(Guid.newGuid().toString(), 2)
    ];
    const outputNodes: OutputNode[] = [
      new OutputNode(Guid.newGuid().toString(), 3)
    ];
    const connectionGenes: ConnectionGene[] = [
      new ConnectionGene(Guid.newGuid().toString(), organismId, 0, 3, 1, true),
      new ConnectionGene(Guid.newGuid().toString(), organismId, 1, 3, 1, false),
      new ConnectionGene(Guid.newGuid().toString(), organismId, 2, 3, 1, true),
      new ConnectionGene(Guid.newGuid().toString(), organismId, 1, 4, 1, true),
      new ConnectionGene(Guid.newGuid().toString(), organismId, 4, 3, 1, true),
      new ConnectionGene(Guid.newGuid().toString(), organismId, 0, 4, 1, true)
    ];
    const organism: Organism = new Organism(organismId, connectionGenes, 0, 0, 'coolio', inputNodes, outputNodes);
    expect(organism.evaluate([inA, inB, inC])[0]).toBeCloseTo(expected, 15);
  })
});
