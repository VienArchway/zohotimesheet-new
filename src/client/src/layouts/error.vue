<script setup>
import AppContainer from '@/components/templates/AppContainer.vue'
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
      <p v-if="props.errorStatus === '401'" style="color: red">
        Authorize token has expired or not valid. Please try again.
      </p>
      <p v-else>
        {{ props.errorMessage }}. Please contact admin.
      </p>
      
      <router-view />

      <v-btn href="/">
        {{ t('button.back') }}
      </v-btn>
      <v-btn
        v-if="props.errorStatus === '401'"
        @click="router.push('/auth/callback?revoke=true')"
      >
        {{ t('button.try-again') }}
      </v-btn>
    </AppContainer>
  </main>
</template>

<script>
export default {
  name: "error"
}
</script>
