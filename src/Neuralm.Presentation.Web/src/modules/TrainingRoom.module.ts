import TrainingRoom from '@/models/TrainingRoom';
import { Module, MutationTree, ActionTree } from 'vuex';
import TrainingSession from '../models/TrainingSession';

export interface ITrainingRoomState {
  trainingRoom: TrainingRoom | undefined;
}

export interface ITrainingRoomMutations extends MutationTree<ITrainingRoomState> {
  setTrainingRoom(trainingRoomState: ITrainingRoomState, trainingRoom: TrainingRoom): void;
  addTrainingSession(trainingRoomState: ITrainingRoomState, trainingSession: TrainingSession): void;
}

export interface ITrainingRoomModule {
  namespaced?: boolean;
  state?: ITrainingRoomState;
  mutations?: ITrainingRoomMutations;
}

/**
 * Represents the trainingroom module class, an implementation of the ITrainingRoomModule interface.
 */
export default class TrainingRoomModule implements ITrainingRoomModule, Module<ITrainingRoomState, TrainingRoom> {
  public namespaced?: boolean;
  public state?: ITrainingRoomState;
  public mutations?: ITrainingRoomMutations;

  /**
   * Initializes an instance of the TrainingRoomModule.
   */
  public constructor() {
    this.namespaced = true;
    this.state = this.getTrainingRoomState();
    this.mutations = this.getMutations();
  }

  private getTrainingRoomState(): ITrainingRoomState {
    return {
      trainingRoom: undefined
    };
  }

  private getMutations(): ITrainingRoomMutations {
    const mutations: ITrainingRoomMutations = {
      setTrainingRoom(trainingRoomState: ITrainingRoomState, trainingRoom: TrainingRoom): void {
        trainingRoomState.trainingRoom = trainingRoom;
      },
      addTrainingSession(trainingRoomState: ITrainingRoomState, trainingSession: TrainingSession): void {
        const trainingRoom: TrainingRoom = trainingRoomState.trainingRoom!;
        trainingRoom.trainingSessions.push(trainingSession);
        trainingRoomState.trainingRoom = trainingRoom;
      }
    };
    return mutations;
  }
}
