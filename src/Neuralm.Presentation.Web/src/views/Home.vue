<template>
  <div class="home">
    <img alt="Vue logo" src="../assets/logo.png">
    <!-- <HelloWorld msg="Welcome to Your Vue.js + TypeScript App"/> -->
    <button v-on:click="loadTrainingRooms()">Load training rooms</button>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import HelloWorld from '@/components/HelloWorld.vue';
import ITrainingRoomService from '../interfaces/ITrainingRoomService';
import GetEnabledTrainingRoomsRequest from '../messages/requests/GetEnabledTrainingRoomsRequest';
import GetEnabledTrainingRoomsResponse from '../messages/responses/GetEnabledTrainingRoomsResponse';


@Component({
  components: {
    HelloWorld,
  },
  async mounted(): Promise<void> {
    return await (this as Home).loadTrainingRooms();
  }
})
export default class Home extends Vue {
  @Prop() private trainingRoomService!: ITrainingRoomService;

  public async loadTrainingRooms(): Promise<void> {
    const response: GetEnabledTrainingRoomsResponse = await this.trainingRoomService.getEnabledTrainingRooms(new GetEnabledTrainingRoomsRequest());
    console.log(response);
  }
}
</script>
