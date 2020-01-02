/**
 * Represents the training session interface.
 */
export default interface TrainingSession {
  id: string;
  startedTimestamp: string;
  endedTimestamp: string;
  userId: string;
  trainingRoomId: string;
}
