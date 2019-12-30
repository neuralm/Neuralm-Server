import { IAppState } from '../modules/App.module';
import { IDashboardState } from '../modules/Dashboard.module';
import { ITrainingRoomState } from '../modules/TrainingRoom.module';
import { IUserState } from '../modules/User.module';
import { ITrainingSessionState } from '../modules/TrainingSession.module';

/**
 * Represents the IRootState interface.
 */
export interface IRootState {
  app: IAppState;
  dashboard: IDashboardState;
  trainingRoom: ITrainingRoomState;
  user: IUserState;
  trainingSession: ITrainingSessionState;
}
