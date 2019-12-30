import { Module, MutationTree, ActionTree } from 'vuex';
import TrainingRoom from '../models/TrainingRoom';
import PaginateTrainingRoomResponse from '../messages/responses/PaginateTrainingRoomResponse';


export interface IDashboardState {
  trainingrooms: TrainingRoom[];
  pagination: {
    page: number,
    itemsPerPage: number,
    itemsPerPageArray: number[],
    numberOfPages: number,
    totalRecords: number
  };
  loading: boolean;
}

export interface IDashboardMutations extends MutationTree<IDashboardState> {
  updateItemsPerPage(dashboardState: IDashboardState, itemsPerPage: number): void;
  nextPage(dashboardState: IDashboardState): void;
  formerPage(dashboardState: IDashboardState): void;
  updatePaginator(dashboardState: IDashboardState, response: PaginateTrainingRoomResponse): void;
  toggleLoading(dashboardState: IDashboardState): void;
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
        itemsPerPage: 5,
        itemsPerPageArray: [5, 8, 12],
        numberOfPages: 1,
        totalRecords: 0
      },
      loading: false,
      trainingrooms: []
    };
    return state;
  }

  private getMutations(): IDashboardMutations {
    const mutations: IDashboardMutations = {
      updateItemsPerPage(dashboardState: IDashboardState, itemsPerPage: number): void {
        dashboardState.pagination.itemsPerPage = itemsPerPage;
        dashboardState.pagination.page = 1;
        if (dashboardState.pagination.totalRecords > 0) {
          dashboardState.pagination.numberOfPages = Math.ceil(dashboardState.pagination.totalRecords / itemsPerPage);
        }
      },
      nextPage(dashboardState: IDashboardState): void {
        if (dashboardState.pagination.page + 1 <= dashboardState.pagination.numberOfPages) {
          dashboardState.pagination.page += 1;
        }
      },
      formerPage(dashboardState: IDashboardState): void {
        if (dashboardState.pagination.page - 1 <= dashboardState.pagination.numberOfPages && dashboardState.pagination.page - 1 > 0) {
          dashboardState.pagination.page -= 1;
        }
      },
      updatePaginator(dashboardState: IDashboardState, response: PaginateTrainingRoomResponse): void {
        dashboardState.trainingrooms = response.items;
        dashboardState.pagination = {
          itemsPerPage: response.pageSize,
          numberOfPages: response.pageCount,
          page: response.pageNumber,
          itemsPerPageArray: [5, 8, 12],
          totalRecords: response.totalRecords
        };
        dashboardState.loading = false;
      },
      toggleLoading(dashboardState: IDashboardState): void {
        dashboardState.loading = !dashboardState.loading;
      }
    };
    return mutations;
  }
}
