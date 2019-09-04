import IUserService from '@/interfaces/IUserService';
import router from './router';
import IStore from './Default.store';
import User from '../models/user';
import UserService from '@/services/UserService';

export interface IUserState {
  status: { 
    loggedIn: boolean,
    registering: boolean 
  };
  user: User | null;
};

export interface IUserActions {
  login(
    { dispatch, commit }: any, 
    { username, password }: { username: String, password: String }
  ): void;
  logout(
    { commit }: any
  ): void;
  register(
    { dispatch, commit }: any,
    user: User
  ): void;
};

export interface IUserMutations {
  loginRequest(state: IUserState, user: User): void;
  loginSuccess(state: IUserState, user: User): void;
  loginFailure(state: IUserState): void;
  logout(state: IUserState): void;
  registerRequest(state: IUserState): void;
  registerSuccess(state: IUserState): void;
  registerFailure(state: IUserState): void;
};

export interface IUserStore extends IStore {
  state: IUserState;
  actions: IUserActions;
  mutations: IUserMutations;
};

const user: User = JSON.parse(localStorage.getItem('user')) as User;

const state: IUserState = user 
  ? { status: { loggedIn: true, registering: false }, user }
  : { status: { loggedIn: false, registering: false }, user: null };

const actions: IUserActions = {
  login({ dispatch, commit }, { username, password }) {
    commit('loginRequest', { username });

    userService.login(username, password)
      .then(
        user => {
          commit('loginSuccess', user);
          router.push('/');
        },
        error => {
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
        user => {
          commit('registerSuccess', user);
          router.push('/login');
          setTimeout(() => {
            dispatch('alert/success', 'Registration successful', { root: true});
          });
        },
        error => {
          commit('registerFailure', error);
          dispatch('alert/error', error, { root: true })
        }
      )
  }
};

const mutations = {
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

/**
 * Represents the user store class, an implementation of the IUserStore interface.
 */
class UserStore implements IUserStore {
  private _userService: IUserService;
  public namespaced: boolean;
  public state: IUserState;
  public actions: IUserActions;
  public mutations: IUserMutations;

  /**
   * Initializes an instance of the UserStore.
   */
  public constructor(
    namespaced: boolean, 
    state: IUserState, 
    actions: IUserActions, 
    mutations: IUserMutations,
    userService: IUserService) {
    this.namespaced = namespaced;
    this.state = state;
    this.actions = actions;
    this.mutations = mutations;
    this._userService = userService;
  }
}

export const userStore: UserStore = new UserStore(
  true,
  state,
  actions,
  mutations,
  new UserService()
);
