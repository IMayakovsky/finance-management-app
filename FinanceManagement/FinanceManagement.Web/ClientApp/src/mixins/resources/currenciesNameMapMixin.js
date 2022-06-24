export default {
  data() {
    return {
      selectedCurrency: this.$t('account.selectCurrency'),
    };
  },
  computed: {
    currencyEnumTranslation() {
      // array indices are the same as enum values on backend
      return [
        this.$t('CZK'),
        this.$t('EUR'),
        this.$t('USD'),
        this.$t('RUB'),
        this.$t('PLN'),
        this.$t('GBP'),
      ];
    },
    currencyOptions() {
      return this.currencyEnumTranslation.map((e, idx) => ({
        label: e,
        value: idx,
      }));
    },
  },
};
