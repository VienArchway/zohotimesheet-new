<template>
  <AppLogin :is-login="isLogin">
    <AppContainer>
      <h1>Homepage</h1>
      <div>
        <router-link to="/about">About</router-link>
      </div>
      <AsyncShowMessage />
    </AppContainer>
  </AppLogin>
</template>

<script setup>
import AppLogin from '@/components/app/AppLogin.vue'
import AppContainer from '@/components/app/AppContainer.vue'
import AppLoading from '@/components/app/AppLoading.vue'
import { defineAsyncComponent } from 'vue'
import { useLogin } from '@/lib/auth/login.js'

const AsyncShowMessage = defineAsyncComponent({
  loader: () => import('@/components/ShowMessage.vue'),
  loadingComponent: AppLoading,
  delay: 200
})
const { isLogin } = useLogin()
</script>

<style scoped>
</style>