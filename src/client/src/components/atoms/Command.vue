<template>
  <v-btn-toggle multiple class="commands">
    <v-btn size="small" v-show="canDelete" @click="emit('delete')">
      <v-icon icon="mdi-trash-can-outline"></v-icon>
    </v-btn>

    <v-btn size="small" v-show="canCreate" @click="emit('create')">
      <v-icon icon="mdi-plus"></v-icon>
    </v-btn>

    <v-btn size="small" v-show="canUpdate" @click="emit('update')">
      <v-icon icon="mdi-pen"></v-icon>
    </v-btn>
  </v-btn-toggle>
</template>

<script setup>
const props = defineProps({
  canDelete: {
    type: Boolean,
    default: true
  },
  canCreate: {
    type: Boolean,
    default: true
  },
  canUpdate: {
    type: Boolean,
    default: true
  }
});

const emit = defineEmits(["delete", "create", "update"]);
</script>


<style scoped>
.commands .v-btn {
    padding: 0;
    min-width: 40px;
}
</style>