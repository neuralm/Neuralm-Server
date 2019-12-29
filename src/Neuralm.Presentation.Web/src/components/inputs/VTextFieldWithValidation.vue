<template>
  <ValidationProvider :name="$attrs.label" :rules="rules" v-slot="{ errors, valid }">
    <v-text-field
      v-model="innerValue"
      :error-messages="errors"
      :success="valid"
      v-bind="$attrs"
      v-on="$listeners"
    ></v-text-field>
  </ValidationProvider>
</template>

<script lang="ts">
import { ValidationProvider } from 'vee-validate';
import Vue from 'vue';
import Component from 'vue-class-component';

@Component({
  components: {
    ValidationProvider
  },
  props: {
    rules: {
      type: [Object, String],
      default: ''
    },
    // must be included in props
    value: {
      type: undefined
    }
  },
  data: () => ({
    innerValue: ''
  }),
  watch: {
    // Handles internal model changes.
    innerValue(newVal): any {
      ((this as unknown) as VTextFieldWithValidation).$emit('input', newVal);
    },
    // Handles external model changes.
    value(newVal): any {
      (this as any).innerValue = newVal;
    }
  },
  created() {
    if ((this as any).value) {
      (this as any).innerValue = (this as any).value;
    }
  }
})
export default class VTextFieldWithValidation extends Vue {}
</script>
