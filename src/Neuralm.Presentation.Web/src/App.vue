<template>
  <v-app>
    <core-app-bar />
    <core-alert />
    <core-drawer />
    <core-view />
    <core-footer />
  </v-app>
</template>

<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';

const loadCoreComponent = (name: string): any => import(`./components/core/${name}.vue`);

@Component({
  components: {
    coreAlert: () => loadCoreComponent('CoreAlert'),
    coreAppBar: () => loadCoreComponent('CoreAppBar'),
    coreDrawer: () => loadCoreComponent('CoreDrawer'),
    coreView: () => loadCoreComponent('CoreView'),
    coreFooter: () => loadCoreComponent('CoreFooter')
  }
})
export default class App extends Vue {}
</script> 

<style>
* {
  font-family: 'Nunito';
}
</style>
