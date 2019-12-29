<template>
  <v-app>
    <vue-snotify></vue-snotify>
    <core-app-bar />
    <core-drawer />
    <core-view />
    <core-footer />
  </v-app>
</template>

<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';
import ComponentLoader from './helpers/ComponentLoader';

@Component({
  components: {
    coreAppBar: () => ComponentLoader('core/CoreAppBar'),
    coreDrawer: () => ComponentLoader('core/CoreDrawer'),
    coreView: () => ComponentLoader('core/CoreView'),
    coreFooter: () => ComponentLoader('core/CoreFooter')
  }
})
export default class App extends Vue {}
</script> 

<style>
* {
  font-family: 'Nunito';
}
</style>
