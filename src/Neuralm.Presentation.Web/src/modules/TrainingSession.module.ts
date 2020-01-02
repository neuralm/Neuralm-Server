import { Module, MutationTree, ActionTree } from 'vuex';
import TrainingSession from '../models/TrainingSession';
import Organism from '../models/Organism';

export interface ITrainingSessionState {
  trainingSession: TrainingSession | undefined;
  name: string;
  organisms: Organism[];
  tested: boolean;
}

export interface ITrainingSessionMutations extends MutationTree<ITrainingSessionState> {
  setTrainingSession(trainingSessionState: ITrainingSessionState, payload: { trainingSession: TrainingSession, name: string }): void;
  setOrganisms(trainingSessionState: ITrainingSessionState, payload: { organisms: Organism[], tested: boolean }): void;
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
      organisms: [],
      tested: false
    };
  }

  private getMutations(): ITrainingSessionMutations {
    const mutations: ITrainingSessionMutations = {
      setTrainingSession(trainingSessionState: ITrainingSessionState, payload: { trainingSession: TrainingSession, name: string }): void {
        trainingSessionState.trainingSession = payload.trainingSession;
        trainingSessionState.name = payload.name;
        trainingSessionState.organisms = [];
        trainingSessionState.tested = false;
      },
      setOrganisms(trainingSessionState: ITrainingSessionState, payload: { organisms: Organism[], tested: boolean }): void {
        trainingSessionState.organisms = payload.organisms;
        trainingSessionState.tested = payload.tested;
      }
    };
    return mutations;
  }
}
