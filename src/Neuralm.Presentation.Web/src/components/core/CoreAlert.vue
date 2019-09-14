<template>
  <v-alert :value="show" :elevation="12" transition="slide-y-transition" @click="dismissAlert">
    {{ text }}
  </v-alert>
</template>

<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';
import { mapState, mapActions } from 'vuex';

@Component({
  computed: {
    ...mapState('alert', ['show', 'text', 'type'])
  },
  methods: {
    ...mapActions('alert', ['dismissAlert'])
  }
})
export default class CoreAlert extends Vue {}
</script>
