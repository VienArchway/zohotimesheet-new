<template>
  <RouterView v-slot="{ Component }">
    <Suspense timeout="0">
      <v-app>
        <component :is="Component" />
      </v-app>
    </Suspense>
  </RouterView>
</template>