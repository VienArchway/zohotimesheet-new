<script setup>
import AppContainer from '@/components/app/AppContainer.vue'
import { useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
const router = useRouter()
const props = defineProps({
  errorStatus: String,
  errorMessage: String
})
</script>

<template>
  <main>
    <h1>Error page</h1>
    <AppContainer>
      <h1>{{ props.errorStatus }}</h1>
      <p style="color: red">{{ props.errorMessage }}</p>
      
      <router-view />

      <v-btn @click="router.back()">{{ t('button.back') }}</v-btn>
    </AppContainer>
  </main>
</template>

<script>
export default {
  name: "error"
}
</script>

<style scoped>

</style>