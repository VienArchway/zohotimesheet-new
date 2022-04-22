<script setup>
import AppLogin from '@/components/app/AppLogin.vue'
import AppContainer from '@/components/app/AppContainer.vue'
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
  <AppLogin>
    <AppContainer>
      <h1>Zoho index page</h1>
      {{ t('hello') }}
      <div>
        <router-link to="/about">About</router-link>
      </div>
      <AsyncShowMessage />
    </AppContainer>
  </AppLogin>
</template>