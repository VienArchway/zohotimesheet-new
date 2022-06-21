<script setup>
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()

onMounted(() => {
  router.replace({ path: '/transfer' })
})
</script>

<template>
  <div />
</template>

<route lang="yaml">
meta:
  layout: default
</route>
