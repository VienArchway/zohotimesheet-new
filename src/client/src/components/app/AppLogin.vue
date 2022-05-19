<script setup>
import useAuthStore from '@/store/auth.js'
import { onMounted } from 'vue'

const auth = useAuthStore()

onMounted(() => {
  if (!auth.getAuthentication) auth.login()
})

</script>

<template>
  <div>
    <slot v-if="auth.isAuthenticated" />
  </div>
</template>