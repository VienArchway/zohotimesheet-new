<script setup>
import AppLogin from '@/components/app/AppLogin.vue'
import AppContainer from '@/components/app/AppContainer.vue'
</script>
<template>
  <AppLogin>
    <main>
      <h1>Layout with require authentication</h1>
      <AppContainer>
        <router-view />
      </AppContainer>
    </main>
  </AppLogin>
</template>

<script>
export default {
  name: "auth"
}
</script>

<style scoped>

</style>