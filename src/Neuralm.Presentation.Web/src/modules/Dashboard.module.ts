import { Module, MutationTree, ActionTree } from 'vuex';
import TrainingRoom from '../models/TrainingRoom';


export interface IDashboardState {
  trainingrooms: TrainingRoom[];
  pagination: {
    page: number,
    itemsPerPage: number,
    itemsPerPageArray: number[]
  };
}

export interface IDashboardMutations extends MutationTree<IDashboardState> {
  updateItemsPerPage(dashboardState: IDashboardState, itemsPerPage: number): void;
  nextPage(dashboardState: IDashboardState): void;
  formerPage(dashboardState: IDashboardState): void;
  numberOfPages(dashboardState: IDashboardState): number;
}

export interface IDashboardModule {
  namespaced?: boolean;
  state?: IDashboardState;
  mutations?: IDashboardMutations;
}

/**
 * Represents the dashboard module class, an implementation of the IDashboardModule interface.
 */
export default class DashboardModule implements IDashboardModule, Module<IDashboardState, IDashboardState> {
  public namespaced?: boolean;
  public state?: IDashboardState;
  public mutations?: IDashboardMutations;

  /**
   * Initializes an instance of the DashboardModule.
   */
  public constructor() {
    this.namespaced = true;
    this.state = this.getDashboardState();
    this.mutations = this.getMutations();
  }

  private getDashboardState(): IDashboardState {
    const state: IDashboardState = {
      pagination: {
        page: 1,
        itemsPerPage: 4,
        itemsPerPageArray: [4, 8, 12]
      },
      trainingrooms: []
    };
    return state;
  }

  private getMutations(): IDashboardMutations {
    function numberOfPages(dashboardState: IDashboardState): number {
      console.log('asdsad');
      return Math.ceil(dashboardState.trainingrooms.length / dashboardState.pagination.itemsPerPage);
    }
    const mutations: IDashboardMutations = {
      updateItemsPerPage(dashboardState: IDashboardState, itemsPerPage: number): void {
        dashboardState.pagination.itemsPerPage = itemsPerPage;
      },
      nextPage(dashboardState: IDashboardState): void {
        if (dashboardState.pagination.page + 1 <= numberOfPages(dashboardState)) {
          dashboardState.pagination.page += 1;
        }
      },
      formerPage(dashboardState: IDashboardState): void {
        if (dashboardState.pagination.page - 1 <= numberOfPages(dashboardState)) {
          dashboardState.pagination.page -= 1;
        }
      },
      numberOfPages
    };
    return mutations;
  }
}
