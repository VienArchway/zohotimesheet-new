<script setup>
import {onMounted, ref} from 'vue'
import {useI18n} from 'vue-i18n'
import { getVerifyTokenApi } from '@/api/resources/ZohoToken.js'

const { t } = useI18n()
const status = ref(null)

onMounted(async () => {
  status.value = await getVerifyTokenApi()
})
</script>

<template>
  <b>authorized: {{ status }}</b>
  <h1>Zoho index page</h1>
  {{ t('hello') }}
  <div>
    <router-link data-cy="link-about" to="/about">About</router-link>
  </div>
</template>

<route lang="yaml">
meta:
  layout: auth
</route>
