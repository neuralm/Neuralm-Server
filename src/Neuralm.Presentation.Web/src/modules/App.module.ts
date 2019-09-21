import { Module, MutationTree, ActionTree } from 'vuex';

export interface IAppState {
  drawer: boolean | null;
  color: string;
}

export interface IAppMutations extends MutationTree<IAppState> {
  setDrawer(state: IAppState, drawer: boolean | null): void;
  setColor(state: IAppState, color: string): void;
  toggleDrawer(state: IAppState): void;
}

export interface IAppModule {
  namespaced?: boolean;
  state?: IAppState;
  mutations?: IAppMutations;
}

/**
 * Represents the app module class, an implementation of the IAppModule interface.
 */
export default class AppModule implements IAppModule, Module<IAppState, IAppState> {
  public namespaced?: boolean;
  public state?: IAppState;
  public mutations?: IAppMutations;

  /**
   * Initializes an instance of the AppModule.
   */
  public constructor() {
    this.namespaced = true;
    this.state = this.getAppState();
    this.mutations = this.getMutations();
  }

  private getAppState(): IAppState {
    const state: IAppState = {
      drawer: null,
      color: 'success'
    };
    return state;
  }

  private getMutations(): IAppMutations {
    const mutations: IAppMutations = {
      setDrawer(state: IAppState, drawer: boolean | null): void {
        state.drawer = drawer;
      },
      setColor(state: IAppState, color: string): void {
        state.color = color;
      },
      toggleDrawer(state: IAppState): void {
        state.drawer = !state.drawer;
      }
    };
    return mutations;
  }
}
