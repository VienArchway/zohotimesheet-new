<template>
  <RouterView v-slot="{ Component }">
    <Suspense timeout="0">
      <div class="zoho">
        <component :is="Component" />
      </div>
    </Suspense>
  </RouterView>
</template>

<script setup>
</script>
