<template>
  <div>
    <img alt="Vue logo" src="../assets/logo.png" />
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
  components: {},
  async mounted(): Promise<void> {
    return;
    // NOTE: Temporarily removed this call because it will call the server everytime /Home is visited.
    // return await (this as Home).loadTrainingRooms();
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
