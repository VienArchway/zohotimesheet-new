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
  <ExampleProp :message="state.message" />
  <p>with msg: {{ messageWithParameter }}</p>
  <p>default: {{ messageWithoutParameter }}</p>
</template>

<script>
export default {
  name: "ShowMessageApi"
}
</script>

<style scoped>

</style>
