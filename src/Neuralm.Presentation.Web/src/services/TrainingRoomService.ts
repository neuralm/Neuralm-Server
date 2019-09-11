import ITrainingRoomService from '@/interfaces/ITrainingRoomService';
import GetEnabledTrainingRoomsRequest from '../messages/requests/GetEnabledTrainingRoomsRequest';
import GetEnabledTrainingRoomsResponse from '../messages/responses/GetEnabledTrainingRoomsResponse';
import authHeader from '@/helpers/AuthorizationHeader';
import BaseRestService from './BaseRestService';
import axios from 'axios';

/**
 * Represents the training room service class.
 */
export default class TrainingRoomService extends BaseRestService implements ITrainingRoomService {
  public getEnabledTrainingRooms(getEnabledTrainingRoomsRequest: GetEnabledTrainingRoomsRequest): Promise<GetEnabledTrainingRoomsResponse> {
    const body: string = JSON.stringify(getEnabledTrainingRoomsRequest);
    return axios({
      method: 'POST',
      url: `${process.env.VUE_APP_API_URL}/trainingrooms/enabled`,
      headers: {
        'Content-Type': 'application/json',
        ...authHeader()
      },
      data: body
    })
    .then(this.handleResponse);
  }
}
