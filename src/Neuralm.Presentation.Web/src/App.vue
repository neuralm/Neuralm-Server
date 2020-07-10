<template>
  <v-app>
    <vue-snotify></vue-snotify>
    <core-app-bar />
    <core-drawer />
    <core-view />
    <core-footer />
    <core-modal v-if="showModal" @close="hideModal()" />
  </v-app>
</template>

<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';
import ComponentLoader from './helpers/ComponentLoader';
import { mapState } from 'vuex';

@Component({
  components: {
    coreAppBar: () => ComponentLoader('core/CoreAppBar'),
    coreDrawer: () => ComponentLoader('core/CoreDrawer'),
    coreView: () => ComponentLoader('core/CoreView'),
    coreFooter: () => ComponentLoader('core/CoreFooter'),
    coreModal: () => ComponentLoader('core/CoreModal')
  },
  computed: {
    ...mapState('modal', ['showModal'])
  }
})
export default class App extends Vue {
  private hideModal(): void {
    this.$store.commit('modal/toggleModal');
  }
}
</script> 

<style>
* {
  font-family: 'Nunito';
}
</style>
