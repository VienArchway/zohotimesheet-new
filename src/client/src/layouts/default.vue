<script setup>
import AppLogin from '@/components/templates/AppLogin.vue'
import AppContainer from '@/components/templates/AppContainer.vue'
import Message from '@/components/atoms/Message.vue'
import Loading from '@/components/atoms/Loading.vue'
import AppHeader from '@/components/templates/AppHeader.vue'
</script>

<template>
  <AppLogin>
      <AppHeader />
      <v-main style="height: 100vh; overflow: auto;">
        <AppContainer>
          <Loading />
          <router-view />
        </AppContainer>
      </v-main>
      <v-footer app class="app-footer">
        <Message />
      </v-footer>
  </AppLogin>
</template>

<script>
export default {
  name: "default"
}
</script>

<style scoped>
.app-footer {
  position: fixed;
  margin-left: 0px;
  margin-top: 0px;
  width: calc((100% - 0px) - 0px);
  bottom: 0;
}
</style>