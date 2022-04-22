<script setup>
import useAuthStore from '@/store/auth.js'
import { onMounted } from 'vue'

const auth = useAuthStore()

onMounted(() => {
  if (!auth.getAuthentication) auth.login()
})

</script>

<template>
  <slot v-if="auth.isAuthenticated" />
</template>