import GetEnabledTrainingRoomsRequest from '@/messages/requests/GetEnabledTrainingRoomsRequest';
import GetEnabledTrainingRoomsResponse from '@/messages/responses/GetEnabledTrainingRoomsResponse';

/**
 * Represents the training room service interface.
 */
export default interface ITrainingRoomService {
  /**
   * Gets the enabled training rooms.
   * @param getEnabledTrainingRoomsRequest The get enabled training rooms request object.
   * @returns The get enabled training rooms response.
   */
  getEnabledTrainingRooms(getEnabledTrainingRoomsRequest: GetEnabledTrainingRoomsRequest): Promise<GetEnabledTrainingRoomsResponse>;
}
