<template>
  <RouterView v-slot="{ Component }">
    <Suspense timeout="0">
      <div>
        <component :is="Component" />
      </div>
    </Suspense>
  </RouterView>
</template>

<script setup>
</script>
