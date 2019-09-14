<template>
  <v-navigation-drawer id="app-drawer" v-model="drawerCP" src="/assets/drawer.png" app color="grey darken-2" dark floating mobile-break-point="991" persistent width="260">
    <template v-slot:img="attrs">
      <v-img v-bind="attrs" gradient="to top, rgba(0, 0, 0, .5), rgba(0, 0, 0, .5)" />
    </template>

    <v-list-item two-line>
      <v-list-item-avatar color="white">
        <v-img src="/assets/Neuralm.png" height="34" contain />
      </v-list-item-avatar>

      <v-list-item-title class="title">
        Neuralm
      </v-list-item-title>
    </v-list-item>

    <v-divider class="mx-3 mb-3" />

    <v-list nav>
      <!-- Bug in Vuetify for first child of v-list not receiving proper border-radius -->
      <div />

      <v-list-item v-for="(link, text) in links" :key="text" :to="link.to" active-class="primary white--text">
        <v-list-item-action>
          <v-icon>{{ link.icon }}</v-icon>
        </v-list-item-action>

        <v-list-item-title v-text="link.text" />
      </v-list-item>
    </v-list>

    <template v-if="status.loggedIn" v-slot:append>
      <div class="pa-3">
        <v-btn block @click="logout()">Logout</v-btn>
      </div>
    </template>
  </v-navigation-drawer>
</template>

<script>
import Vue from 'vue';
import { mapMutations, mapState, Store } from 'vuex';
import Component from 'vue-class-component';

@Component({
  props: {
    opened: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    links: [
      {
        to: '/',
        icon: 'mdi-view-dashboard',
        text: 'Home'
      },
      {
        to: '/about',
        icon: 'mdi-account',
        text: 'About'
      }
    ]
  }),
  methods: {
    ...mapMutations('app', ['setDrawer']),
    ...mapMutations('user', ['logout']),
    logout() {
      this.$store.commit('user/logout');
      this.$router.go('/');
    }
  },
  computed: {
    ...mapState('app', ['color', 'drawer']),
    ...mapState('user', ['status']),
    drawerCP: {
      get() {
        return this.drawer;
      },
      set(value) {
        this.setDrawer(value);
      }
    }
  }
})
export default class CoreDrawer extends Vue {}
</script>
