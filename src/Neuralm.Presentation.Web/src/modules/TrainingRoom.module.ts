import TrainingRoom from '@/models/TrainingRoom';
import { Module, MutationTree, ActionTree } from 'vuex';

export interface ITrainingRoomState {

}

export interface ITrainingRoomMutations extends MutationTree<ITrainingRoomState> {

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
    return { };
  }

  private getMutations(): ITrainingRoomMutations {
    const mutations: ITrainingRoomMutations = { };
    return mutations;
  }
}
