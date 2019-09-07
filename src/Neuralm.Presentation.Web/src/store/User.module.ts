import IUserService from '@/interfaces/IUserService';
import router from '@/router';
import User from '@/models/user';
import { Module, MutationTree, ActionTree } from 'vuex';

export interface IUserState {
  status: {
    loggedIn: boolean,
    registering: boolean
  };
  user: User | null;
}

export interface IUserActions extends ActionTree<IUserState, User> {
  login(
    { dispatch, commit }: any,
    { username, password }: { username: string, password: string }
  ): void;
  logout(
    { commit }: any
  ): void;
  register(
    { dispatch, commit }: any,
    user: User
  ): void;
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
  namespaced: boolean;
  state: IUserState;
  actions: IUserActions;
  mutations: IUserMutations;
}

/**
 * Represents the user module class, an implementation of the IUserModule interface.
 */
export default class UserModule implements IUserModule, Module<IUserState, User> {
  private readonly _userService: IUserService;
  public namespaced: boolean;
  public state: IUserState;
  public actions: IUserActions;
  public mutations: IUserMutations;

  /**
   * Initializes an instance of the UserStore.
   */
  public constructor(userService: IUserService) {
    this.namespaced = true;
    this._userService = userService;
    this.state = this.getUserState();
    this.actions = this.getUserActions(this._userService);
    this.mutations = this.getMutations();
  }

  private getUserState(): IUserState {
    const userString: string = localStorage.getItem('user') ? localStorage.getItem('user')! : '';
    const user: User = JSON.parse(userString) as User;
    const userState: IUserState = user
      ? { status: { loggedIn: true, registering: false }, user }
      : { status: { loggedIn: false, registering: false }, user: null };
    return userState;
  }

  private getUserActions(userService: IUserService): IUserActions {
    const actions: IUserActions = {
      login({ dispatch, commit }, { username, password }) {
        commit('loginRequest', { username });
        userService.login(username, password)
          .then(
            (user: User) => {
              commit('loginSuccess', user);
              router.push('/');
            },
            (error: Promise<any>) => {
              commit('loginFailure', error);
              dispatch('alert/error', error, { root: true });
            }
          );
      },
      logout({ commit }) {
        userService.logout();
        commit('logout');
      },
      register({ dispatch, commit }, user) {
        commit('registerRequest', user);
        userService.register(user)
          .then(
            (success: boolean) => {
              commit('registerSuccess', success);
              router.push('/login');
              setTimeout(() => {
                dispatch('alert/success', 'Registration successful', { root: true });
              });
            },
            (error: Promise<any>) => {
              commit('registerFailure', error);
              dispatch('alert/error', error, { root: true });
            }
          );
      }
    };
    return actions;
  }

  private getMutations(): IUserMutations {
    const mutations: IUserMutations = {
      loginRequest(state: IUserState, user: User) {
        state.status = { loggedIn: true, registering: false };
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
