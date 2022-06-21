<script setup>
import { getZohoUserDisplayName } from '@/api/resources/zohoToken'
import { onMounted, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import useAppStore from '@/store/app'

const app = useAppStore()

const { t } = useI18n()
const userName = ref(null)

onMounted(async () => {
  await app.load(async () => {
    userName.value = await getZohoUserDisplayName()
  })
})
</script>

<template>
  <div>
    <h1>{{ userName }}</h1>
    {{ t('hello') }}
  </div>
</template>

<route lang="yaml">
meta:
  layout: default
</route>
