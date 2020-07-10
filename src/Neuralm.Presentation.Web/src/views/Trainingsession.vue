<template>
  <v-container v-if="trainingSession !== undefined" class="fill-height" fluid style="margin-bottom: 75px;">
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
            <v-btn class="getOrganisms" v-if="trainingSession.endedTimestamp === '0001-01-01T00:00:00' && organisms.length == 0 || tested" @click="getOrganisms(trainingSession.id)">Get Organisms</v-btn>
            <v-btn class="testOrganisms" v-if="trainingSession.endedTimestamp === '0001-01-01T00:00:00' && organisms.length > 0 && !tested" @click="testOrganisms(organisms, trainingSession.id)">Test Organisms XOR</v-btn>
          </v-card-actions>
          <v-divider></v-divider>
          <v-data-table v-if="organisms.length > 0" :headers="headers" :items="organisms" class="elevation-1" @click:row="showOrganismGraph"></v-data-table>
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
import ITrainingSessionService from '../interfaces/ITrainingSessionService';
import EndTrainingSessionRequest from '../messages/requests/EndTrainingSessionRequest';
import EndTrainingSessionResponse from '../messages/responses/EndTrainingSessionResponse';
import User from '../models/User';
import GetOrganismsRequest from '../messages/requests/GetOrganismsRequest';
import GetOrganismsResponse from '../messages/responses/GetOrganismsResponse';
import Xor from '../models/Xor';
import Organism from '../models/Organism';
import { ITrainingSessionState } from '../modules/TrainingSession.module';
import { IRootState } from '../interfaces/IRootState';
import ConnectionGene from '../models/ConnectionGene';
import InputNode from '../models/InputNode';
import OutputNode from '../models/OutputNode';
import PostOrganismsScoreRequest from '../messages/requests/PostOrganismsScoreRequest';
import PostOrganismsScoreResponse from '../messages/responses/PostOrganismsScoreResponse';
import { setModalPayload } from '../modules/Modal.module';
import OrganismGraph from '@/components/OrganismGraph.vue';

@Component({
  async created() {
    const trainingSessionState: ITrainingSessionState = (this.$store as Store<IRootState>).state.trainingSession as ITrainingSessionState;
    if (trainingSessionState.trainingSession === undefined) {
      this.$router.push('/dashboard');
    }
  },
  data: () => ({
    headers: [
      {
        text: 'Name',
        align: 'left',
        value: 'name',
      },
      { text: 'Score', value: 'score' },
      { text: 'ConnectionGenes', value: 'connectionGenes.length' }
    ]
  }),
  computed: {
    ...mapState('trainingSession', ['trainingSession', 'name', 'organisms', 'tested'])
  }
})
export default class TrainingSessionView extends Vue {
  @Prop() private trainingSessionService!: ITrainingSessionService;

  public showOrganismGraph(fakeOrganism: Organism) {
    const organism: Organism = this.observerOrganismToOrganism(fakeOrganism);
    const payload: setModalPayload = {
      modalName: 'OrganismGraph',
      propsData: { organism }
    };
    this.$store.commit('modal/setModal', payload);
    this.$store.commit('modal/toggleModal');
  }

  public endTrainingSession(trainingSessionId: string): void {
    this.trainingSessionService.endTrainingSession(new EndTrainingSessionRequest(trainingSessionId))
    .then((response: EndTrainingSessionResponse) => {
      this.$snotify.success(response.message);
      this.$router.push('dashboard');
    })
    .catch((error: EndTrainingSessionResponse) => {
      this.$snotify.error(error.message);
    });
  }

  public getOrganisms(trainingSessionId: string): void {
    const amount: number = 20;
    const user: User = JSON.parse(localStorage.getItem('user')!) as User;
    this.trainingSessionService.getOrganisms(new GetOrganismsRequest(user.userId, trainingSessionId, amount))
    .then((response: GetOrganismsResponse) => {
      this.$snotify.success(response.message);
      this.$store.commit('trainingSession/setOrganisms', { organisms: response.organisms, tested: false });
    })
    .catch((value: GetOrganismsResponse) => {
      this.$snotify.error(value.message);
    });
  }

  public testOrganisms(organisms: Organism[], trainingSessionId: string): void {
    const xor: Xor = new Xor();
    let newOrganisms: Organism[] = [];
    try {
      newOrganisms = this.observerOrganismsToOrganisms(organisms);
    } catch (error) {
      this.$snotify.error('Faulty network structure, failed to construct network!');
      return;
    }
    const organismScores: Map<string, number> = new Map<string, number>();
    for (const organism of newOrganisms) {
      xor.test(organism);
      organismScores.set(organism.id, organism.score);
    }
    this.$store.commit('trainingSession/setOrganisms', { organisms: newOrganisms, tested: true });
    const request: PostOrganismsScoreRequest = new PostOrganismsScoreRequest(trainingSessionId, organismScores);
    this.trainingSessionService.postOrganismsScores(request)
    .then((response: PostOrganismsScoreResponse) => {
      this.$snotify.success(response.message);
    })
    .catch((value: PostOrganismsScoreResponse) => {
      this.$snotify.error(value.message);
      console.log(value);
    });
  }

  private observerOrganismsToOrganisms(organisms: Organism[]): Organism[] {
    const newOrganisms: Organism[] = new Array(organisms.length);
    for (let i = 0; i < organisms.length; i++) {
      const organism: Organism = this.observerOrganismToOrganism(organisms[i]);
      newOrganisms[i] = organism;
    }
    return newOrganisms;
  }

  private observerOrganismToOrganism(fakeOrganism: Organism) {
    const connectionGenes: ConnectionGene[] = new Array(
      fakeOrganism.connectionGenes.length
    );
      for (let j = 0; j < fakeOrganism.connectionGenes.length; j++) {
      const fakeConnectionGene: ConnectionGene =
        fakeOrganism.connectionGenes[j];
        const connectionGene: ConnectionGene = new ConnectionGene(
          fakeConnectionGene.id,
          fakeConnectionGene.organismId,
          fakeConnectionGene.inNodeIdentifier,
          fakeConnectionGene.outNodeIdentifier,
          fakeConnectionGene.weight,
          fakeConnectionGene.enabled
        );
        connectionGenes[j] = connectionGene;
      }
      const inputNodes: InputNode[] = new Array(fakeOrganism.inputNodes.length);
      for (let j = 0; j < fakeOrganism.inputNodes.length; j++) {
        const fakeInputNode: InputNode = fakeOrganism.inputNodes[j];
        const inputnode: InputNode = new InputNode(
          fakeInputNode.id,
          fakeInputNode.nodeIdentifier
        );
        inputNodes[j] = inputnode;
      }
    const outputNodes: OutputNode[] = new Array(
      fakeOrganism.outputNodes.length
    );
      for (let j = 0; j < fakeOrganism.outputNodes.length; j++) {
        const fakeOutputNode: OutputNode = fakeOrganism.outputNodes[j];
        const outputNode: OutputNode = new OutputNode(
          fakeOutputNode.id,
          fakeOutputNode.nodeIdentifier
        );
        outputNodes[j] = outputNode;
      }
      const organism: Organism = new Organism(
        fakeOrganism.id,
        connectionGenes,
        fakeOrganism.score,
        fakeOrganism.generation,
        fakeOrganism.name,
        inputNodes,
      outputNodes
    );
    return organism;
  }
}
</script>
<style scoped>
.getOrganisms {
  /* marker class */
}
.testOrganisms {
  /* marker class */
}
</style>
