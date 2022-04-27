<script setup>
import AppLoading from '@/components/app/AppLoading.vue'
import { defineAsyncComponent } from 'vue'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
const AsyncShowMessage = defineAsyncComponent({
  loader: () => import('@/components/ShowMessage.vue'),
  loadingComponent: AppLoading,
  delay: 200,
  suspensible: false
})
</script>

<template>
  <h1>Zoho index page</h1>
  {{ t('hello') }}
  <div>
    <router-link data-cy="link-about" to="/about">About</router-link>
  </div>
  <AsyncShowMessage />
</template>

<route lang="yaml">
meta:
  layout: auth
</route>
