/**
 * Represents the training session class.
 */
export default interface TrainingSession {
  id: string;
  startedTimestamp: string;
  endedTimestamp: string;
  userId: string;
  trainingRoomId: string;
}
