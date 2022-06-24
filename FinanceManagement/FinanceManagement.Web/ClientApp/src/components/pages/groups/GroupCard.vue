<template>
  <div
    class="col-4 q-pa-md"
  >
    <q-card
      flat
      bordered
      class="group-card bg-grey-1"
    >
      <q-card-actions align="between">
        <q-btn
          flat
          icon-right="add"
          :label="$t('group.income')"
          color="primary"
          @click="handleAddTransaction"
        />
      </q-card-actions>
      <q-card-section>
        <div class="row items-center no-wrap">
          <div class="col">
            <div class="text-h6">
              {{ group.name }}
              <q-popup-edit auto-save v-slot="scope">
                <q-input
                  v-model="groupForm.name"
                  dense autofocus counter
                  @keyup.enter="handleSaveGroupName(scope)"
                />
              </q-popup-edit>
            </div>
            <div class="text-subtitle2">{{ group.code }}</div>
          </div>

          <div class="col-auto">
            <q-btn color="grey-7" round flat icon="more_vert">
              <q-menu cover auto-close>
                <q-list>
                  <q-item clickable @click="handleDeleteGroup">
                    <q-item-section>{{ $t("group.delete") }}</q-item-section>
                  </q-item>
                </q-list>
              </q-menu>
            </q-btn>
          </div>
        </div>
      </q-card-section>

      <q-card-section>
        {{ group.amount }} {{ group.currency }}
      </q-card-section>

      <q-separator />

      <q-card-actions v-if="goToEnabled">
        <q-btn
          color="primary"
          flat @click="handleGoToGroup"
        >{{ $t("group.goToGroup") }}</q-btn>
      </q-card-actions>
    </q-card>
  </div>
</template>

<script>
import currenciesNameMapMixin from 'src/mixins/resources/currenciesNameMapMixin';
import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';
import groupStoreMixin from 'src/mixins/store/groupStoreMixin';
import transactionStoreMixin from 'src/mixins/store/transactionStoreMixin';
import CreateOrUpdateTransactionDialog from 'components/pages/account/CreateOrUpdateTransactionDialog';
import { TransactionStoreMeta } from 'src/store/transaction/store.meta';
import { OkActionEnum } from 'src/common/enums/components/okActions';

export default {
  name: 'GroupCard',
  mixins: [
    currenciesNameMapMixin,
    withLoadingAndErrorDialogMixin,
    groupStoreMixin,
    transactionStoreMixin,
  ],
  props: {
    goToEnabled: {
      default: true,
      type: Boolean,
    },
    group: {
      id: Number,
      code: String,
      currency: Number,
      name: String,
      userId: Number,
      amount: Number,
    },
  },
  data() {
    return {
      groupForm: {
        name: this.group.name,
      },
    };
  },
  methods: {
    async handleAddTransaction() {
      this.$q.dialog({
        component: CreateOrUpdateTransactionDialog,
        componentProps: {
          parentId: this.group.id,
          transactionType: TransactionStoreMeta.Types.Group,
          okAction: OkActionEnum.Create,
        },
      }).onOk(() => {
        this.$emit('add:transaction');
      });
    },
    handleDeleteGroup() {
      this.withGlobalLoadingAndErrorDialog(this.deleteGroup);
    },
    handleGoToGroup() {
      this.$router.push(`${this.$route.path}/${this.group.id}`);
    },
    handleSaveGroupName(scope) {
      scope.set();
      this.withGlobalLoadingAndErrorDialog(this.updateGroup);
    },
    async deleteGroup() {
      await this.deleteGroupAction(this.group.id);
      this.$emit('delete');
    },
    async updateGroup() {
      await this.updateGroupAction({
        ...this.group,
        name: this.groupForm.name,
      });
      this.$emit('edit');
    },
  },
};
</script>

<style scoped>

</style>
