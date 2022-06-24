export default {
  methods: {
    async withErrorDialog(
      tryProcedure,
      catchProcedure = () => null,
    ) {
      try {
        await tryProcedure();
      } catch (err) {
        await catchProcedure(err);
        this.$emit('error-occurred', err);
        this.$q.dialog({
          title: this.$t('errorOccurred'),
          message: err,
          persistent: true,
        });
        // this allows to catch error on component level
        throw err;
      }
    },
  },
};
