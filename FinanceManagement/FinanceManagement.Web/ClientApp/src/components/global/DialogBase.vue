<template>
  <q-dialog ref="dialog" @hide="onDialogHide">
    <q-card class="q-dialog-plugin">
      <!--
        ...content
        ... use q-card-section for it?
      -->
      <slot></slot>

      <!-- buttons example -->
      <q-card-actions align="right">
        <q-btn
          color="primary"
          label="OK"
          @click="onOKClick"
          id="dialog-base-modal-ok"
          :disable="disableOk"
        />
        <q-btn color="primary" label="Cancel" @click="onCancelClick"/>
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>

<script>
import dialogBaseMixin from 'src/mixins/components/dialogBaseMixin';

export default {
  // used to wrap content in the dialog, so it can be called via $q.dialog
  name: 'DialogBase',
  mixins: [dialogBaseMixin],
  props: {
    disableOk: Boolean,
  },
};
</script>
