<template>
  <v-container class="fill-height" fluid>
    <v-row align="center" justify="center">
      <v-col cols="12" sm="8" md="8">
        <ValidationObserver v-slot="{ handleSubmit }">
        <v-form @submit.prevent="handleSubmit(onSubmit)"> 
          <v-card class="elevation-12">
            <v-toolbar dark color="blue darken-3" flat>
              <v-toolbar-title>Create training room</v-toolbar-title>
            </v-toolbar>
            <v-card-text>
                <v-subheader>
                  <b>Training room</b>
                </v-subheader>
                <v-divider></v-divider>
                <v-text-field-with-validation rules="required" v-model="trainingroom.name" label="Name"/>

                <v-subheader>
                  <b>Settings</b>
                </v-subheader>
                <v-divider></v-divider>
                <v-row>
                  <v-col>
                    <v-text-field-with-validation rules="required" v-model="trainingroom.trainingRoomSettings.organismCount" label="OrganismCount" type="number"/>
                  </v-col>
                  <v-col>
                    <v-text-field-with-validation rules="required" v-model="trainingroom.trainingRoomSettings.inputCount" label="InputCount" type="number"/>
                  </v-col>
                  <v-col>
                    <v-text-field-with-validation rules="required" v-model="trainingroom.trainingRoomSettings.outputCount" label="OutputCount" type="number"/>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col>
                    <v-text-field-with-validation rules="required" v-model="trainingroom.trainingRoomSettings.speciesExcessGeneWeight" label="SpeciesExcessGeneWeight" type="number"/>
                  </v-col>
                  <v-col>
                    <v-text-field-with-validation rules="required" v-model="trainingroom.trainingRoomSettings.speciesDisjointGeneWeight" label="SpeciesDisjointGeneWeight" type="number"/>
                  </v-col>
                  <v-col>
                    <v-text-field-with-validation rules="required" v-model="trainingroom.trainingRoomSettings.speciesAverageWeightDiffWeight" label="SpeciesAverageWeightDiffWeight" type="number"/>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col>
                    <v-text-field-with-validation rules="required" v-model="trainingroom.trainingRoomSettings.threshold" label="Threshold" type="number"/>
                  </v-col>
                  <v-col>
                    <v-text-field-with-validation rules="required" v-model="trainingroom.trainingRoomSettings.addConnectionChance" label="AddConnectionChance" type="number"/>
                  </v-col>
                  <v-col>
                    <v-text-field-with-validation rules="required" v-model="trainingroom.trainingRoomSettings.addNodeChance" label="AddNodeChance" type="number"/>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col>
                    <v-text-field-with-validation rules="required" v-model="trainingroom.trainingRoomSettings.crossOverChance" label="CrossOverChance" type="number"/>
                  </v-col>
                  <v-col>
                    <v-text-field-with-validation rules="required" v-model="trainingroom.trainingRoomSettings.interSpeciesChance" label="InterSpeciesChance" type="number"/>
                  </v-col>
                  <v-col>
                    <v-text-field-with-validation rules="required" v-model="trainingroom.trainingRoomSettings.mutationChance" label="MutationChance" type="number"/>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col>
                    <v-text-field-with-validation rules="required" v-model="trainingroom.trainingRoomSettings.mutateWeightChance" label="MutateWeightChance" type="number"/>
                  </v-col>
                  <v-col>
                    <v-text-field-with-validation rules="required" v-model="trainingroom.trainingRoomSettings.weightReassignChance" label="WeightReassignChance" type="number"/>
                  </v-col>
                  <v-col>
                    <v-text-field-with-validation rules="required" v-model="trainingroom.trainingRoomSettings.topAmountToSurvive" label="TopAmountToSurvive" type="number"/>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col>
                    <v-text-field-with-validation rules="required" v-model="trainingroom.trainingRoomSettings.enableConnectionChance" label="EnableConnectionChance" type="number"/>
                  </v-col>
                  <v-col>
                    <v-text-field-with-validation rules="required" v-model="trainingroom.trainingRoomSettings.seed" label="Seed" type="number"/>
                  </v-col>
                </v-row>
            </v-card-text>
            <v-card-actions>
              <v-spacer />
              <v-btn dark color="blue darken-3" type="submit">Create</v-btn>
            </v-card-actions>
          </v-card>
        </v-form>
        </ValidationObserver>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import ITrainingRoomService from '../interfaces/ITrainingRoomService';
import TrainingRoom from '../models/TrainingRoom';
import ComponentLoader from '../helpers/ComponentLoader';
import { ValidationObserver } from 'vee-validate';
import CreateTrainingRoomRequest from '../messages/requests/CreateTrainingRoomRequest';
import CreateTrainingRoomResponse from '../messages/responses/CreateTrainingRoomResponse';
import Guid from '../helpers/Guid';
import Owner from '../models/Owner';
import User from '../models/User';

function getOwner(): Owner {
  const user: User = JSON.parse(localStorage.getItem('user')!);
  const owner: Owner = { id: user.userId, username: user.username };
  return owner;
}

@Component({
  components: {
    vTextFieldWithValidation: () => ComponentLoader('inputs/VTextFieldWithValidation'),
    ValidationObserver
  },
  data: () => ({
    trainingroom: {
      id: '',
      name: '',
      owner: getOwner(),
      generation: 0,
      trainingRoomSettings: {
        id: Guid.newGuid().toString(),
        organismCount: 50,
        inputCount: 2,
        outputCount: 1,
        speciesExcessGeneWeight: 1,
        speciesDisjointGeneWeight: 1,
        speciesAverageWeightDiffWeight: 0.4,
        threshold: 3,
        addConnectionChance: 0.05,
        addNodeChance: 0.03,
        crossOverChance: 0.75,
        interSpeciesChance: 0.001,
        mutationChance: 1,
        mutateWeightChance: 0.8,
        weightReassignChance: 0.1,
        topAmountToSurvive: 0.5,
        enableConnectionChance: 0.25,
        seed: 1
      }
    }
  })
})
export default class CreateTrainingRoomView extends Vue {
  @Prop() private trainingRoomService!: ITrainingRoomService;

  public onSubmit(e: Event): void {
    const trainingRoom: TrainingRoom = this.$data.trainingroom as TrainingRoom;
    console.log('trainingroom:', trainingRoom);
    this.trainingRoomService.createTrainingRoom(new CreateTrainingRoomRequest(trainingRoom)).then(
      (response: CreateTrainingRoomResponse) => {
        if (response.success) {
          this.$snotify.success('Successfully created a training room!');
          this.$router.push('/dashboard');
        } else {
          this.$snotify.error('Failed to create training room');
        }
      },
      (error: Promise<CreateTrainingRoomResponse>) => {
        this.$snotify.error('Failed to create training room');
      }
    );
  }
}
</script>