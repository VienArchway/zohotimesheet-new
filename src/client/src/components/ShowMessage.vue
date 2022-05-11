<script setup>
import { onMounted, ref, reactive } from "vue";
import ExampleProp from "./ExampleProp.vue"
import { getDefaultMessageApi, getMessageWithParamApi } from '@/api/resources/Message.js'

const state = reactive({
  message: 'testing',
  customMessage: 'custom text',
})

const messageWithParameter = ref('')
const messageWithoutParameter = ref('')

onMounted(async () => {
  messageWithoutParameter.value = await getDefaultMessageApi()
  messageWithParameter.value = await getMessageWithParamApi(state.customMessage)
})

</script>

<template>
  <h2>Show message component</h2>
  <ExampleProp :message="state.message" />
  <p data-testid="custom-ms">with msg: {{ messageWithParameter }}</p>
  <p data-testid="default-ms">default: {{ messageWithoutParameter }}</p>
</template>

<script>
export default {
  name: "ShowMessageApi"
}
</script>

<style scoped>

</style>
