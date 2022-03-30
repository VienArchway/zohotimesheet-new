<template>
  <v-container>
    <ZohoTable />
    <div class="text-center">
      <v-pagination
          v-model="page"
          :length="4"
          prev-icon="mdi-menu-left"
          next-icon="mdi-menu-right"
      ></v-pagination>
    </div>
  </v-container>

</template>

<script setup>
import ZohoTable from "./table/ZohoTable.vue";

const page = 1
</script>

<style scoped>

</style>