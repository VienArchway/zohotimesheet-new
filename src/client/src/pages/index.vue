<script setup>
import { useI18n } from 'vue-i18n'
const { t } = useI18n()
</script>

<template>
  <div>
    <h1>Zoho index page</h1>
    {{ t('hello') }}
  </div>
</template>

<route lang="yaml">
meta:
  layout: default
</route>
