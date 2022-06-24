import { GlobalOperationsStoreActions } from 'src/store/globalOperations/actions.meta';
import { GlobalOperationsStoreGetters } from 'src/store/globalOperations/getters.meta';
import { mapGetters } from 'vuex';

export default {
  data() {
    return {
      localLoading: false,
    };
  },
  computed: {
    ...mapGetters({
      globalLoadingGetter: GlobalOperationsStoreGetters.globalLoading,
    }),
  },
  methods: {
    async withGlobalLoading(procedure) {
      await this.$store.dispatch(GlobalOperationsStoreActions.setGlobalLoading, true);
      try {
        await procedure();
      } finally {
        await this.$store.dispatch(GlobalOperationsStoreActions.setGlobalLoading, false);
      }
    },
    async withLocalLoading(procedure) {
      this.localLoading = true;
      // to see that it actually works
      // await new Promise((r) => setTimeout(r, 1500));
      try {
        await procedure();
      } finally {
        this.localLoading = false;
      }
    },
    async withComponentLoading(procedure, component) {
      component.localLoading = true;
      // to see that it actually works
      // await new Promise((r) => setTimeout(r, 1500));
      try {
        await procedure();
      } finally {
        component.localLoading = false;
      }
    },
  },
};
