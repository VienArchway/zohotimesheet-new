<template>
  <h1>Homepage</h1>
  <div>
    <router-link to="/about">About</router-link>
    <a :href="zohoAuthUrl">
      Login
    </a>
  </div>
  <v-container>
    <show-message />
    <HelloWorld />
  </v-container>
</template>

<script setup>
import HelloWorld from '@/components/HelloWorld.vue'
import ShowMessage from '@/components/ShowMessage.vue'
import { onMounted, ref } from 'vue'

const zohoAuthUrl = 'https://accounts.zoho.com/oauth/v2/auth?scope=ZohoSprints.teams.READ+ZohoSprints.projects.READ&client_id=1000.1KQGLWBUATFDBEE1S19FVN59D339GN&response_type=code&redirect_uri=http://localhost:3000/auth/callback&access_type=offline'
onMounted(() => {
  console.log('access-token', localStorage.getItem('access-token'))
  console.log('refresh-token', localStorage.getItem('refresh-token'))
})
</script>

<style scoped>

</style>