<template>
  <ExampleProp :message="state.message" />
  <p>with msg: {{ messageWithParameter }}</p>
  <p>default: {{ messageWithoutParameter }}</p>
</template>

<script setup>
import { onMounted, ref, reactive } from "vue";
import ExampleProp from "./ExampleProp.vue"
import MessageAPI from '../api/resources/Message.js'

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
  messageWithParameter.value = await MessageAPI.getMessageWithParam(state.customMessage)
}

// call api get message without msg parameter
const getDefaultMessage = async () => {
  messageWithoutParameter.value = await MessageAPI.getDefaultMessage()
}
</script>