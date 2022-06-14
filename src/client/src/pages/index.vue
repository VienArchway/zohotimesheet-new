<script setup>
import { onMounted, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import { getVerifyTokenApi } from '@/api/resources/zohoToken'

const { t } = useI18n()
const status = ref(null)

onMounted(async () => {
  status.value = await getVerifyTokenApi()
})
</script>

<template>
  <div>
    <b>authorized: {{ status }}</b>
    <h1>Zoho index page</h1>
    {{ t('hello') }}
    <div>
      <router-link data-cy="link-about" to="/about">About</router-link><br>
      <router-link data-cy="link-about" to="/transfer">Transfer</router-link><br>
      <router-link data-cy="link-about" to="/schedule">Schedule</router-link><br>
    </div>
  </div>
</template>

<route lang="yaml">
meta:
  layout: default
</route>
