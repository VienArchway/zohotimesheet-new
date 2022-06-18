<script setup>
import appStore from '@/store/app.js'

const app = appStore()
</script>


<template>
    <v-overlay :model-value="app.isLoading" :absolute="true" :z-index="1000" class="align-center justify-center">
        <v-progress-circular indeterminate color="white" size="64" />
    </v-overlay>
</template>

<script>
export default {
    name: "Loading"
}
</script>