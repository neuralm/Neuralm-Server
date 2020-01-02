import Request from './Request';
/**
 * Represents the DictionaryEntry interface.
 * Used to mimic a dictionary entry for better serialization.
 */
interface DictionaryEntry {
  key: string;
  value: number;
}

/**
 * Represents the PostOrganismsScoreRequest class.
 */
export default class PostOrganismsScoreRequest extends Request {
  public trainingSessionId: string;
  public organismScores: DictionaryEntry[];

  /**
   * Initializes a new isntance of the PostOrganismsScoreRequest class.
   * @param trainingSessionId The training session id.
   * @param organismScores The organism scores map.
   */
  constructor(trainingSessionId: string, organismScores: Map<string, number>) {
    super();
    this.trainingSessionId = trainingSessionId;
    this.organismScores = this.map_to_array(organismScores);
  }

  /**
   * Convert a `Map` to a standard array.
   * @param map to convert.
   * @returns converted array.
   */
  private map_to_array(map: Map<string, number>): DictionaryEntry[] {
    const array: DictionaryEntry[] = new Array(map.entries.length);
    map.forEach((value, key) => {
      const val: DictionaryEntry = { key, value };
      array.push(val);
    });
    return array;
  }
}
