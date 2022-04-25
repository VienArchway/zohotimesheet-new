<script setup>
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
</script>

<template>
  <div>
    {{ t('pages.not-found') }}
  </div>
</template>

<route lang="yaml">
meta:
  layout: error
</route>