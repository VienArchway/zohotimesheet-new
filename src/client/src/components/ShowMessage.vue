<script setup>
// import { useI18n } from 'vue-i18n'
import { onMounted, ref, reactive } from "vue";
import ExampleProp from "./ExampleProp.vue"
import { getDefaultMessageApi, getMessageWithParamApi } from '@/api/resources/Message.js'

// const { t } = useI18n()

onMounted(() => {
  getDefaultMessage()
  getMessageWithParam()
})

const state = reactive({
  message: 'testing',
  customMessage: 'custom text',
})

const messageWithParameter = ref('')
const messageWithoutParameter = ref('')

// call api get message with msg parameter
const getMessageWithParam = async () => {
  messageWithParameter.value = await getMessageWithParamApi(state.customMessage)
}

// call api get message without msg parameter
const getDefaultMessage = async () => {
  messageWithoutParameter.value = await getDefaultMessageApi()
}
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
