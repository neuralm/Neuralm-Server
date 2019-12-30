import StartTrainingSessionRequest from '../messages/requests/StartTrainingSessionRequest';
import StartTrainingSessionResponse from '../messages/responses/StartTrainingSessionResponse';
import EndTrainingSessionRequest from '../messages/requests/EndTrainingSessionRequest';
import EndTrainingSessionResponse from '../messages/responses/EndTrainingSessionResponse';

/**
 * Represents the training session service interface.
 */
export default interface ITrainingSessionService {
  /**
   * Starts the training session.
   * @param startTrainingSessionRequest The start training session request object.
   * @returns The start training session response.
   */
  startTrainingSession(startTrainingSessionRequest: StartTrainingSessionRequest): Promise<StartTrainingSessionResponse>;

  /**
   * Ends the training session.
   * @param endTrainingSessionRequest The end training session request object.
   * @returns The end training session response.
   */
  endTrainingSession(endTrainingSessionRequest: EndTrainingSessionRequest): Promise<EndTrainingSessionResponse>;
}
