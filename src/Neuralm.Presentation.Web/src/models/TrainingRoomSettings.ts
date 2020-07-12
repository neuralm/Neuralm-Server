/**
 * Represents the training room settings interface.
 */
export default interface TrainingRoomSettings {
  id: string;
  organismCount: number;
  inputCount: number;
  outputCount: number;
  speciesExcessGeneWeight: number;
  speciesDisjointGeneWeight: number;
  speciesAverageWeightDiffWeight: number;
  threshold: number;
  addConnectionChance: number;
  addNodeChance: number;
  crossOverChance: number;
  interSpeciesChance: number;
  mutationChance: number;
  mutateWeightChance: number;
  weightReassignChance: number;
  topAmountToSurvive: number;
  enableConnectionChance: number;
  seed: number;
  maxStagnantTime: number;
  championCloneMinSpeciesSize: number;
}
