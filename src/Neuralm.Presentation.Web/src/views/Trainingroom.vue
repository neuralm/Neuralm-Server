<template>
  <v-container v-if="trainingRoom !== undefined" class="fill-height" fluid style="margin-bottom: 75px;">
    <v-row align="center" justify="center">
      <v-col cols="12" sm="8" md="8">
        <v-card class="mx-auto">
          <v-card-title class="subheading font-weight-bold">
            <v-toolbar>
              <v-toolbar-title>{{ trainingRoom.name }}</v-toolbar-title>
              <v-spacer></v-spacer>
              <v-toolbar-items>
                <v-btn @click="showSettings = !showSettings" class="align-end">Toggle settings</v-btn>
              </v-toolbar-items>
            </v-toolbar>
          </v-card-title>
          <v-divider></v-divider>
          <div v-if="showSettings">
            <v-list dense>
              <v-list-item v-for="(key, index) in propnames(trainingRoom.trainingRoomSettings)" :key="index" color="white">
                <v-list-item-content>{{ toCap(key) }}:</v-list-item-content>
                <v-list-item-content class="align-end">{{ trainingRoom.trainingRoomSettings[key] }}</v-list-item-content>
              </v-list-item>
            </v-list>
          </div>
          <v-divider></v-divider>
          <v-toolbar>
            <v-toolbar-title>Training sessions</v-toolbar-title>
          </v-toolbar>
          <v-list dense>
            <v-list-item v-for="trainingSession in trainingRoom.trainingSessions" :key="trainingSession.id" color="white">
              <v-list-item-content>
                <v-card class="mx-auto">
                  <v-card-title class="font-weight-bold">{{ trainingSession.id }}</v-card-title>
                  <v-card-text>
                    <v-list dense>
                      <v-list-item v-for="(key, index) in propnames(trainingSession)" :key="index" color="white">
                        <v-list-item-content>{{ toCap(key) }}:</v-list-item-content>
                        <v-list-item-content class="align-end">{{ trainingSession[key] }}</v-list-item-content>
                      </v-list-item>
                    </v-list>
                  </v-card-text>
                  <v-card-actions>
                    <v-btn @click="goToTrainingSession(trainingSession, trainingRoom)">Go to TrainingSession</v-btn>
                  </v-card-actions>
                </v-card>
              </v-list-item-content>
            </v-list-item>
          </v-list>
          <v-card-actions v-if="trainingRoom.trainingSessions.length == 0">
            <v-btn @click="startTrainingSession(trainingRoom.id)">Start TrainingSession</v-btn>
          </v-card-actions>
         </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import TrainingRoom from '../models/TrainingRoom';
import ComponentLoader from '../helpers/ComponentLoader';
import { mapState, Store } from 'vuex';
import { IRootState } from '../interfaces/IRootState';
import TrainingSession from '../models/TrainingSession';
import ITrainingSessionService from '../interfaces/ITrainingSessionService';
import StartTrainingSessionRequest from '../messages/requests/StartTrainingSessionRequest';
import User from '../models/User';
import StartTrainingSessionResponse from '../messages/responses/StartTrainingSessionResponse';

@Component({
  data: () => ({
    showSettings: true
  }),
  computed: {
    ...mapState('trainingRoom', ['trainingRoom'])
  }
})
export default class TrainingRoomView extends Vue {
  @Prop() private trainingSessionService!: ITrainingSessionService;

  public propnames(item: object): string[] {
    return Object.keys(item).slice(1);
  }

  public toCap(key: string): string {
    return key.replace(/^\w/, (c) => c.toUpperCase());
  }

  public goToTrainingSession(trainingSession: TrainingSession, trainingRoom: TrainingRoom): void {
    this.$store.commit('trainingSession/setTrainingSession', { trainingSession, name: trainingRoom.name });
    this.$router.push('trainingsession');
  }

  public startTrainingSession(trainingRoomId: string): void {
    const user: User = JSON.parse(localStorage.getItem('user')!) as User;
    this.trainingSessionService.startTrainingSession(new StartTrainingSessionRequest(user.userId, trainingRoomId))
    .then((response: StartTrainingSessionResponse) => {
      if (response.success) {
        this.$store.commit('trainingRoom/addTrainingSession', response.trainingSession);
        this.$snotify.success(response.message);
      } else {
        this.$snotify.error(response.message);
      }
    })
    .catch((error: Promise<StartTrainingSessionResponse> | StartTrainingSessionResponse) => {
      if (error instanceof StartTrainingSessionResponse) {
        this.$snotify.error(error.message);
      } else {
        this.$snotify.error('Something happened try again later.');
      }
    });
  }
}
</script>