<template>
  <v-footer id="core-footer" absolute height="82">
    <div class="footer-items">
      <span v-for="link in links" :key="link.name">
        <a :href="link.Link" class="tertiary--text footer-links">{{ link.name }}</a>
      </span>
    </div>
    <v-spacer />
    <span class="font-weight-light">
      Check out Neuralm on <a href="https://www.Github.com/neuralm/neuralm-server" target="_blank">Github</a> :-)
    </span>
  </v-footer>
</template>

<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';

@Component({
  data: () => ({
    links: [
      { name: 'Home', Link: '/' }
    ]
  })
})
export default class CoreFooter extends Vue {}
</script>

<style>
#core-footer {
  z-index: 0;
}
</style>
