<script setup>
import AppLogin from '@/components/app/AppLogin.vue'
import AppContainer from '@/components/app/AppContainer.vue'
</script>

<template>
  <AppLogin>
    <main>
      <AppContainer>
        <router-view />
      </AppContainer>
    </main>
  </AppLogin>
</template>

<script>
export default {
  name: "default"
}
</script>