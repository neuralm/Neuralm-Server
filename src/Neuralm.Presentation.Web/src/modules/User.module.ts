import User from '@/models/User';
import { Module, MutationTree, ActionTree } from 'vuex';

export interface IUserState {
  status: {
    loggedIn: boolean;
    registering: boolean;
  };
  user: User | null;
}

export interface IUserMutations extends MutationTree<IUserState> {
  loginRequest(state: IUserState, user: User): void;
  loginSuccess(state: IUserState, user: User): void;
  loginFailure(state: IUserState): void;
  logout(state: IUserState): void;
  registerRequest(state: IUserState): void;
  registerSuccess(state: IUserState): void;
  registerFailure(state: IUserState): void;
}

export interface IUserModule {
  namespaced?: boolean;
  state?: IUserState;
  mutations?: IUserMutations;
}

/**
 * Represents the user module class, an implementation of the IUserModule interface.
 */
export default class UserModule implements IUserModule, Module<IUserState, User> {
  public namespaced?: boolean;
  public state?: IUserState;
  public mutations?: IUserMutations;

  /**
   * Initializes an instance of the UserModule.
   */
  public constructor() {
    this.namespaced = true;
    this.state = this.getUserState();
    this.mutations = this.getMutations();
  }

  private getUserState(): IUserState {
    if (!!localStorage.getItem('user')) {
      const user: User = JSON.parse(localStorage.getItem('user')!) as User;
      return { status: { loggedIn: true, registering: false }, user };
    }
    return { status: { loggedIn: false, registering: false }, user: null };
  }

  private getMutations(): IUserMutations {
    const mutations: IUserMutations = {
      loginRequest(state: IUserState, user: User) {
        state.status = { loggedIn: false, registering: false };
        state.user = user;
      },
      loginSuccess(state: IUserState, user: User) {
        state.status = { loggedIn: true, registering: false };
        state.user = user;
      },
      loginFailure(state: IUserState) {
        state.status = { loggedIn: false, registering: false };
        state.user = null;
      },
      logout(state: IUserState) {
        state.status = { loggedIn: false, registering: false };
        state.user = null;
        localStorage.removeItem('user');
      },
      registerRequest(state: IUserState) {
        state.status = { loggedIn: false, registering: true };
      },
      registerSuccess(state: IUserState) {
        state.status = { loggedIn: false, registering: false };
      },
      registerFailure(state: IUserState) {
        state.status = { loggedIn: false, registering: false };
      }
    };
    return mutations;
  }
}
