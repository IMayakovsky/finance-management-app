<template>
  <q-page class="q-px-md">
    <q-btn @click="handleAdd" class="q-ml-md">{{ $t("account.add") }}</q-btn>
    <div class="row">
      <AccountCard
        v-for="account in accountsGetter"
        :key="account.id"
        :account="account"
        @add:transaction="refreshPage"
        @edit="refreshPage"
        @delete="refreshPage"
      />
    </div>
    <ComponentLoading
      :is-loading="localLoading"
    />
  </q-page>
</template>

<script>
import AddAccountDialog from 'components/pages/accounts/AddAccountDialog';
import AccountCard from 'components/pages/accounts/AccountCard';
import accountStoreMixin from 'src/mixins/store/accountStoreMixin';
import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';
import ComponentLoading from 'components/global/ComponentLoading';

export default {
  name: 'Accounts',
  components: { ComponentLoading, AccountCard },
  methods: {
    async handleAdd() {
      this.$q.dialog({
        component: AddAccountDialog,
      }).onOk(this.refreshPage);
    },
    async refreshPage() {
      await this.withLocalLoadingAndErrorDialog(this.fetchAccountsAction);
    },
  },
  mixins: [accountStoreMixin, withLoadingAndErrorDialogMixin],
  async mounted() {
    await this.refreshPage();
  },
};
</script>

<style scoped>

</style>
