<template>
  <v-container fluid>
    <v-data-iterator :items="trainingrooms" :items-per-page.sync="pagination.itemsPerPage" :page="pagination.page" :loading="loading" loading-text="Loading... Please wait" hide-default-footer>
      <template v-slot:header>
        <v-toolbar dark color="blue darken-3" class="mb-1">
          <v-toolbar-title>Training Rooms</v-toolbar-title>
        </v-toolbar>
      </template>

      <v-row>
        <v-col v-for="item in trainingrooms" :key="item.name" cols="12" sm="6" md="4" lg="3">
          <training-room-card v-bind:item="item" ></training-room-card>
        </v-col>
      </v-row>

      <template v-slot:footer>
        <v-row class="mt-2" align="center" justify="center" style="margin-bottom: 75px; margin-left: 10px; margin-right: 10px;">
          <span class="grey--text">Items per page</span>
          <v-menu offset-y>
            <template v-slot:activator="{ on }">
              <v-btn dark text color="primary" class="ml-2" v-on="on">
                {{ pagination.itemsPerPage }}
                <v-icon>mdi-chevron-down</v-icon>
              </v-btn>
            </template>
            <v-list>
              <v-list-item v-for="(number, index) in pagination.itemsPerPageArray" :key="index" @click="updateItemsPerPage(number)">
                <v-list-item-title>{{ number }}</v-list-item-title>
              </v-list-item>
            </v-list>
          </v-menu>

          <div class="flex-grow-1"></div>

          <span class="mr-4 grey--text">
            Page {{ pagination.page }} of {{ pagination.numberOfPages }}
          </span>
          <v-btn fab dark color="blue darken-3" class="mr-1" @click="formerPage()">
            <v-icon>mdi-chevron-left</v-icon>
          </v-btn>
          <v-btn fab dark color="blue darken-3" class="ml-1" @click="nextPage()">
            <v-icon>mdi-chevron-right</v-icon>
          </v-btn>
        </v-row>
      </template>
    </v-data-iterator>
  </v-container>
</template>

<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';
import ITrainingRoomService from '../interfaces/ITrainingRoomService';
import { Prop } from 'vue-property-decorator';
import { DefaultMethods, DefaultData } from 'vue/types/options';
import GetEnabledTrainingRoomsResponse from '../messages/responses/GetEnabledTrainingRoomsResponse';
import GetEnabledTrainingRoomsRequest from '../messages/requests/GetEnabledTrainingRoomsRequest';
import ComponentLoader from '../helpers/ComponentLoader';
import { mapGetters, mapMutations, mapState, Store } from 'vuex';
import DashboardModule, { IDashboardState } from '../modules/Dashboard.module';
import TrainingRoom from '../models/TrainingRoom';
import PaginateTrainingRoomRequest from '../messages/requests/PaginateTrainingRoomRequest';
import { IRootState } from '../interfaces/IRootState';
import PaginateTrainingRoomResponse from '../messages/responses/PaginateTrainingRoomResponse';

@Component({
  components: {
    trainingRoomCard: () => ComponentLoader('TrainingRoomCard')
  },
  computed: {
    ...mapState('dashboard', ['pagination', 'trainingrooms', 'loading'])
  },
  methods: {
    ...mapMutations('dashboard', ['updateItemsPerPage', 'nextPage', 'formerPage', 'updatePaginator'])
  },
  async mounted(): Promise<void> {
    const view: DashboardView = (this as unknown as DashboardView);
    const dashboardState: IDashboardState = (view.$store as Store<IRootState>).state.dashboard as IDashboardState;
    await view.paginateTrainingRooms(dashboardState.pagination.page, dashboardState.pagination.itemsPerPage);
  },
  created() {
    const view: DashboardView = (this as unknown as DashboardView);
    const dashboardState: IDashboardState = (view.$store as Store<IRootState>).state.dashboard as IDashboardState;
    view.$store.subscribe(async (mutation, payload) => {
      const mutations = ['dashboard/updateItemsPerPage', 'dashboard/nextPage', 'dashboard/formerPage'];
      if (mutations.find((mut) => mut === mutation.type)) {
        if (dashboardState.loading) {
          return;
        }
        await view.paginateTrainingRooms(dashboardState.pagination.page, dashboardState.pagination.itemsPerPage);
      }
    });
  }
})
export default class DashboardView extends Vue {
  @Prop() private trainingRoomService!: ITrainingRoomService;

  public paginateTrainingRooms(pageNumber: number, pageSize: number) {
    this.$store.commit('dashboard/toggleLoading');
    return this.trainingRoomService.paginateTrainingRooms(
      new PaginateTrainingRoomRequest(pageNumber, pageSize)
    ).then((response: PaginateTrainingRoomResponse) => {
      this.$store.commit('dashboard/updatePaginator', response);
    },
    (error: Promise<PaginateTrainingRoomResponse>) => {
      error.then((value: PaginateTrainingRoomResponse) => {
        this.$snotify.error(value.message);
      });
    });
  }
}
</script>
