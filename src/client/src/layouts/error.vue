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
    <AppContainer>
      <div class="text-center">
        <h1>Whoops, {{ props.errorStatus }}</h1>
        <p v-if="props.errorStatus === '401'" style="color: red">
          Authorize token has expired or not valid.
        </p>
        <p v-else>
          The operation has been error. Please contact admin.
        </p>
      </div>
      
      <router-view />
      
      <div class="text-center mt-4">
        <v-btn
          v-if="props.errorStatus === '401'"
          class="uppercase"
          @click="router.push('/auth/callback?revoke=true')"
        >
          {{ t('button.try-again') }}
        </v-btn>
        <v-btn v-else href="/">
          {{ t('button.back') }}
        </v-btn>
      </div>
      
    </AppContainer>
  </main>
</template>

<script>
export default {
  name: "error"
}
</script>
