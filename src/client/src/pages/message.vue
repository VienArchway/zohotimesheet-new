<script setup>
import AppLoading from '@/components/app/AppLoading.vue'
import { defineAsyncComponent } from 'vue'

const AsyncShowMessage = defineAsyncComponent({
  loader: () => import('@/components/ShowMessage.vue'),
  loadingComponent: AppLoading,
  delay: 200,
  suspensible: false
})
</script>

<template>
  <div>
    <AsyncShowMessage />
    <router-link data-cy="link-index" to="/">Back to home</router-link>
  </div>
</template>

<route lang="yaml">
meta:
  layout: default
</route>