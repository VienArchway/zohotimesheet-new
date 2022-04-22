<script setup>
import AppLogin from '@/components/app/AppLogin.vue'
import AppContainer from '@/components/app/AppContainer.vue'
</script>

<template>
  <AppLogin>
    <AppContainer>
      <p>About</p>
      <router-link to="/">Back to home</router-link>
    </AppContainer>
  </AppLogin>
</template>

