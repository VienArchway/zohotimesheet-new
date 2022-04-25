<script setup>
import AppContainer from '@/components/app/AppContainer.vue'
</script>
<template>
  <main>
    <AppContainer>
      <router-view />
    </AppContainer>
  </main>
</template>

<script>
export default {
  name: "default"
}
</script>

<style scoped>

</style>