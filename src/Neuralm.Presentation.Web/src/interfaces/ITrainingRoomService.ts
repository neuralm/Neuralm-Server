import GetEnabledTrainingRoomsRequest from '@/messages/requests/GetEnabledTrainingRoomsRequest';
import GetEnabledTrainingRoomsResponse from '@/messages/responses/GetEnabledTrainingRoomsResponse';
import GetTrainingRoomResponse from '../messages/responses/GetTrainingRoomResponse';
import GetTrainingRoomRequest from '../messages/requests/GetTrainingRoomRequest';
import CreateTrainingRoomRequest from '../messages/requests/CreateTrainingRoomRequest';
import CreateTrainingRoomResponse from '../messages/responses/CreateTrainingRoomResponse';
import PaginateTrainingRoomRequest from '../messages/requests/PaginateTrainingRoomRequest';
import PaginateTrainingRoomResponse from '../messages/responses/PaginateTrainingRoomResponse';

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

  /**
   * Paginates the training rooms.
   * @param paginateTrainingRoomRequest The paginate training room request.
   * @returns The pagination response.
   */
  paginateTrainingRooms(paginateTrainingRoomRequest: PaginateTrainingRoomRequest): Promise<PaginateTrainingRoomResponse>;

  /**
   * Posts a create training room request.
   * @param createTrainingRoomRequest The create training room request object.
   * @returns The training room id.
   */
  createTrainingRoom(createTrainingRoomRequest: CreateTrainingRoomRequest): Promise<CreateTrainingRoomResponse>;
}
