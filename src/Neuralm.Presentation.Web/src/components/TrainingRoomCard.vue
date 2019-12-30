<template>
  <div>
    <v-card class="mx-auto">
      <v-card-title class="subheading font-weight-bold">{{ item.name }}</v-card-title>
      <v-divider></v-divider>
      <v-list dense>
        <v-list-item color="white">
          <v-list-item-content>Generation:</v-list-item-content>
          <v-list-item-content class="align-end">{{ item.generation }}</v-list-item-content>
        </v-list-item>
        <v-list-item color="white">
          <v-list-item-content>Owner:</v-list-item-content>
          <v-list-item-content class="align-end">{{ item.owner.username }}</v-list-item-content>
        </v-list-item>
      </v-list>
      <v-divider></v-divider>
      <v-list dense>
        <v-list-item v-for="(key, index) in propnames(item.trainingRoomSettings)" :key="index" color="white">
          <v-list-item-content>{{ toCap(key) }}:</v-list-item-content>
          <v-list-item-content class="align-end">{{ item.trainingRoomSettings[key] }}</v-list-item-content>
        </v-list-item>
      </v-list>
      <v-card-actions>
        <v-btn outlined @click="goToTrainingRoom(item)">Trainingroom</v-btn>
      </v-card-actions>
    </v-card>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';
import TrainingSession from '../models/TrainingSession';
import TrainingRoom from '../models/TrainingRoom';
import { mapMutations } from 'vuex';

@Component({
  props: [
    'item'
  ],
  methods: {
    ...mapMutations('trainingRoom', ['setTrainingRoom'])
  }
})
export default class TrainingRoomCard extends Vue {
  public propnames(item: object): string[] {
    return Object.keys(item).slice(1);
  }

  public toCap(key: string): string {
    return key.replace(/^\w/, (c) => c.toUpperCase());
  }

  public goToTrainingRoom(trainingRoom: any): void {
    this.$store.commit('trainingRoom/setTrainingRoom', trainingRoom);
    this.$router.push('trainingroom');
  }
}
</script>