<script setup>
import { onBeforeMount } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()

onBeforeMount(() => {
  router.replace({ path: '/transfer' })
})
</script>

<template>
  <div />
</template>
