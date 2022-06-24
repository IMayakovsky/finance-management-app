import errorDialogMixin from 'src/mixins/decorators/withErrorDialogMixin';
import loadingMixin from 'src/mixins/decorators/withLoadingMixin';

export default {
  // facade for global loading and error dialog
  mixins: [errorDialogMixin, loadingMixin],
  methods: {
    async withGlobalLoadingAndErrorDialog(tryProcedure, catchProcedure) {
      await this.withGlobalLoading(() => this.withErrorDialog(tryProcedure, catchProcedure));
    },
    async withLocalLoadingAndErrorDialog(tryProcedure, catchProcedure) {
      await this.withLocalLoading(() => this.withErrorDialog(tryProcedure, catchProcedure));
    },
    async withComponentLoadingAndErrorDialog(tryProcedure, component, catchProcedure) {
      await this.withComponentLoading(
        () => this.withErrorDialog(tryProcedure, catchProcedure),
        component,
      );
    },
  },
};
