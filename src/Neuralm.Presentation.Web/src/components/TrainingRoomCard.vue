<template>
  <div>
    <v-card class="mx-auto" :flat="flat" :loading="loading" :outlined="outlined" :elevation="elevation" :raised="raised" :width="width" :height="height">
      <v-card-title>I'm a title</v-card-title>
      <v-card-text>I'm card text</v-card-text>
      <v-card-actions>
        <v-btn outlined>View statistics</v-btn>
      </v-card-actions>
    </v-card>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';

@Component({})
export default class TrainingRoomCard extends Vue {}
</script>
