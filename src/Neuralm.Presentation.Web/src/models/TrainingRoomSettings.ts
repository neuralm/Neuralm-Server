/**
 * Represents the training room settings interface.
 */
export default interface TrainingRoomSettings {
  Id: string;
  OrganismCount: number;
  InputCount: number;
  OutputCount: number;
  SpeciesExcessGeneWeight: number;
  SpeciesDisjointGeneWeight: number;
  SpeciesAverageWeightDiffWeight: number;
  Threshold: number;
  AddConnectionChance: number;
  AddNodeChance: number;
  CrossOverChance: number;
  InterSpeciesChance: number;
  MutationChance: number;
  MutateWeightChance: number;
  WeightReassignChance: number;
  TopAmountToSurvive: number;
  EnableConnectionChance: number;
  Seed: number;
}
