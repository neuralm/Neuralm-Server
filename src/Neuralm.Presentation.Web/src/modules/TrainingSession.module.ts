import { Module, MutationTree, ActionTree } from 'vuex';
import TrainingSession from '../models/TrainingSession';
import Organism from '../models/Organism';

export interface ITrainingSessionState {
  trainingSession: TrainingSession | undefined;
  name: string;
  organisms: Organism[];
}

export interface ITrainingSessionMutations extends MutationTree<ITrainingSessionState> {
  setTrainingSession(trainingSessionState: ITrainingSessionState, payload: { trainingSession: TrainingSession, name: string }): void;
  setOrganisms(trainingSessionState: ITrainingSessionState, organisms: Organism[]): void;
}

export interface ITrainingSessionModule {
  namespaced?: boolean;
  state?: ITrainingSessionState;
  mutations?: ITrainingSessionMutations;
}

/**
 * Represents the training session module class, an implementation of the ITrainingSessionModule interface.
 */
export default class TrainingSessionModule implements ITrainingSessionModule, Module<ITrainingSessionState, TrainingSession> {
  public namespaced?: boolean;
  public state?: ITrainingSessionState;
  public mutations?: ITrainingSessionMutations;

  /**
   * Initializes an instance of the TrainingRoomModule.
   */
  public constructor() {
    this.namespaced = true;
    this.state = this.getTrainingSessionState();
    this.mutations = this.getMutations();
  }

  private getTrainingSessionState(): ITrainingSessionState {
    return {
      trainingSession: undefined,
      name: '',
      organisms: []
    };
  }

  private getMutations(): ITrainingSessionMutations {
    const mutations: ITrainingSessionMutations = {
      setTrainingSession(trainingSessionState: ITrainingSessionState, payload: { trainingSession: TrainingSession, name: string }): void {
        trainingSessionState.trainingSession = payload.trainingSession;
        trainingSessionState.name = payload.name;
        trainingSessionState.organisms = [];
      },
      setOrganisms(trainingSessionState: ITrainingSessionState, organisms: Organism[]): void {
        trainingSessionState.organisms = organisms;
      }
    };
    return mutations;
  }
}
