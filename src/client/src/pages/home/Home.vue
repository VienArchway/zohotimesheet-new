<template>
  <a ref="redirectLogin" :href="zohoAuthUrl" />
  <v-container v-if="isLogin">
    <h1>Homepage</h1>
    <div>
      <router-link to="/about">About</router-link>
    </div>
    <ShowMessageComp />
    <HelloWorld />
  </v-container>
</template>

<script setup>
import { defineAsyncComponent, onMounted, ref } from 'vue'
import HelloWorld from '@/components/HelloWorld.vue'
const ShowMessageComp = defineAsyncComponent(() => import('@/components/ShowMessage.vue'))

const zohoAuthUrl = import.meta.env.VITE_ZOHO_REDIRECT_LOGIN
const redirectLogin = ref(null)
const isLogin = ref(false)

onMounted(() => {
  console.log('access-token', localStorage.getItem('access-token'))
  console.log('refresh-token', localStorage.getItem('refresh-token'))
  if (!localStorage.getItem('access-token')) {
    window.location.href = redirectLogin.value.href
  } else {
    isLogin.value = true
  }
})
</script>

<style scoped>

</style>