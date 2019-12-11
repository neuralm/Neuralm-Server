import GetEnabledTrainingRoomsRequest from '@/messages/requests/GetEnabledTrainingRoomsRequest';
import GetEnabledTrainingRoomsResponse from '@/messages/responses/GetEnabledTrainingRoomsResponse';
import GetTrainingRoomResponse from '../messages/responses/GetTrainingRoomResponse';
import GetTrainingRoomRequest from '../messages/requests/GetTrainingRoomRequest';

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

  /**
   * Gets a training room by id.
   * @param getTrainingRoomRequest The get training room request object.
   * @returns The training room.
   */
  getTrainingRoom(getTrainingRoomRequest: GetTrainingRoomRequest): Promise<GetTrainingRoomResponse>;
}
