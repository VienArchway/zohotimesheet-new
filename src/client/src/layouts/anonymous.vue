<script setup>
import AppContainer from '@/components/templates/AppContainer.vue'
import Loading from '@/components/atoms/Loading.vue'

</script>
<template>
  <v-main>
    <AppContainer>
      <Loading />
      <router-view />
    </AppContainer>
  </v-main>
</template>

<script>
export default {
  name: "anonymous"
}
</script>