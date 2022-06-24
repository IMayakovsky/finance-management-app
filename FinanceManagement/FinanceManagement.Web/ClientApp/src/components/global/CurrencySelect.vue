<template>
  <q-select
    filled
    class="q-pb-none q-mt-lg"
    :label="$t('account.selectCurrency')"
    v-model="selectedCurrency"
    :options="currencyOptions"
    @update:model-value="updateValue"
  />
</template>

<script>
import currenciesNameMapMixin from 'src/mixins/resources/currenciesNameMapMixin';

export default {
  name: 'CurrencySelect',
  props: {
    modelValue: Object,
  },
  mixins: [currenciesNameMapMixin],
  methods: {
    updateValue() {
      this.$emit('update:modelValue', this.selectedCurrency);
    },
  },
};
</script>

<style scoped>

</style>
