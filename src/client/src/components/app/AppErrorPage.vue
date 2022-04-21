<script setup>
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
  <div>
    <RouterView />
    <h1>{{ props.errorStatus }}</h1>
    <p style="color: red">{{ props.errorMessage }}</p>
    <button type="button" @click="router.back()">{{ t('button.back') }}</button>
  </div>
</template>


<style scoped>

</style>