<template>
  <v-container v-if="trainingSession !== undefined" class="fill-height" fluid>
    <v-row align="center" justify="center">
      <v-col cols="12" sm="8" md="8">
        <v-card class="mx-auto">
          <v-card-title class="subheading font-weight-bold">{{ name }}</v-card-title>
          <v-divider></v-divider>
          <v-card-text>
            Id: {{ trainingSession.id }}
          </v-card-text>
          <v-divider></v-divider>
          <v-card-actions>
            <v-btn v-if="trainingSession.endedTimestamp === '0001-01-01T00:00:00'" @click="endTrainingSession(trainingSession.id)">End TrainingSession</v-btn>
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
import { mapState } from 'vuex';
import ITrainingSessionService from '../interfaces/ITrainingSessionService';
import EndTrainingSessionRequest from '../messages/requests/EndTrainingSessionRequest';
import EndTrainingSessionResponse from '../messages/responses/EndTrainingSessionResponse';

@Component({
  computed: {
    ...mapState('trainingSession', ['trainingSession', 'name'])
  }
})
export default class TrainingSessionView extends Vue {
  @Prop() private trainingSessionService!: ITrainingSessionService;

  public endTrainingSession(trainingSessionId: string): void {
    this.trainingSessionService.endTrainingSession(new EndTrainingSessionRequest(trainingSessionId))
    .then((response: EndTrainingSessionResponse) => {
      this.$snotify.success(response.message);
      this.$router.push('dashboard');
    },
    (error: Promise<EndTrainingSessionResponse>) => {
      error.then((value: EndTrainingSessionResponse) => {
        this.$snotify.error(value.message);
      });
    });
  }
}
</script>