import { Module, MutationTree, ActionTree } from 'vuex';

type AlertType = 'success' | 'warning' | 'info' | 'error';

export interface IAlertState {
  show: boolean;
  text: string;
  type: AlertType;
}

export interface IAlertMutations extends MutationTree<IAlertState> {
  dismissAlert(state: IAlertState): void;
  showErrorAlert(state: IAlertState, text: string): void;
  showInfoAlert(state: IAlertState, text: string): void;
  showSuccessAlert(state: IAlertState, text: string): void;
  showWarningAlert(state: IAlertState, text: string): void;
}

export interface IAlertModule {
  namespaced?: boolean;
  state?: IAlertState;
  mutations?: IAlertMutations;
}

/**
 * Represents the alert module class, an implementation of the IAlertModule interface.
 */
export default class AlertModule implements IAlertModule, Module<IAlertState, IAlertState> {
  public namespaced?: boolean;
  public state?: IAlertState;
  public mutations?: IAlertMutations;

  /**
   * Initializes an instance of the AlertModule.
   */
  public constructor() {
    this.namespaced = true;
    this.state = this.getAlertState();
    this.mutations = this.getMutations();
  }

  private getAlertState(): IAlertState {
    const state: IAlertState = {
      show: false,
      type: 'info',
      text: ''
    };
    return state;
  }

  private getMutations(): IAlertMutations {
    const mutations: IAlertMutations = {
      dismissAlert(state: IAlertState): void {
        state = { show: false, type: 'info', text: '' };
      },
      showInfoAlert(state: IAlertState, text: string): void {
        state = { show: true, type: 'info', text };
      },
      showErrorAlert(state: IAlertState, text: string): void {
        state = { show: true, type: 'error', text };
      },
      showSuccessAlert(state: IAlertState, text: string): void {
        state = { show: true, type: 'success', text };
      },
      showWarningAlert(state: IAlertState, text: string): void {
        state = { show: true, type: 'warning', text };
      },
    };
    return mutations;
  }
}
