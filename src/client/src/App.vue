<template>
  <RouterView v-slot="{ Component }">
    <Suspense timeout="0">
      <div class="app-component">
        <component :is="Component" />
      </div>
    </Suspense>
  </RouterView>
</template>

<script setup>
</script>
