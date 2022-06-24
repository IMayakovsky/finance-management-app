<template>
  <q-input
    filled
    :label="$t('selectDate')"
    v-model="selectedDate"
    mask="date"
    :rules="['date']"
    class="q-mt-md"
    @update:model-value="updateValue"
  >
    <template v-slot:append>
      <q-icon name="event" class="cursor-pointer">
        <q-popup-proxy ref="qDateProxy" cover transition-show="scale" transition-hide="scale">
          <q-date v-model="selectedDate" @update:model-value="updateValue">
            <div class="row items-center justify-end">
              <q-btn v-close-popup label="Close" color="primary" flat />
            </div>
          </q-date>
        </q-popup-proxy>
      </q-icon>
    </template>
  </q-input>
</template>

<script>
import { convertDateToOnlyDateString } from 'src/utils/common';

export default {
  name: 'DateSelection',
  data() {
    return {
      selectedDate: convertDateToOnlyDateString(new Date()),
    };
  },
  methods: {
    updateValue() {
      this.$emit('update:modelValue', this.selectedDate);
    },
  },
};
</script>

<style scoped>

</style>
