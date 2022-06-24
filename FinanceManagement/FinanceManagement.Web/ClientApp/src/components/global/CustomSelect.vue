<template>
  <q-select
    filled
    class="q-pb-none q-mt-lg"
    :label="label"
    v-model="selected"
    :options="options"
    @update:model-value="updateValue"
  />
</template>

<script>
import accountStoreMixin from 'src/mixins/store/accountStoreMixin';

export default {
  name: 'CustomSelect',
  props: {
    modelValue: Object,
    options: Array,
    label: String,
  },
  mixins: [accountStoreMixin],
  data() {
    return {
      selected: this.$t('selectValue'),
    };
  },
  mounted() {
    const preSelected = this.options.find(({ value }) => value === this.modelValue);
    this.selected = preSelected || this.selected;
  },
  methods: {
    updateValue() {
      this.$emit('update:modelValue', this.selected.value);
    },
  },
};
</script>

<style scoped>

</style>
