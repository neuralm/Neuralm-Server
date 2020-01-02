import Organism from './Organism';

/**
 * Represents the xor class.
 * User to test an organism.
 */
export default class Xor {
  /**
   * Tests the organism with XOR and sets its score.
   * @param organism The organism.
   */
  public test(organism: Organism): void {
    let error = 0;
    for (let i = 0; i <= 1; i++) {
      for (let j = 0; j <= 1; j++) {
        const output: number[] = organism.evaluate([i, j]); // ,1
        const expected: number = i ^ j;
        error += Math.abs(expected - output[0]);
      }
    }
    const score: number = 4 - error;
    organism.score = score;
  }
}
