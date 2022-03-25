<template>
  <ExampleProp :message="state.message" />
  <p>with msg: {{ messageWithParameter }}</p>
  <p>default: {{ messageWithoutParameter }}</p>
</template>

<script setup>
import { ref, reactive } from "vue";
import ExampleProp from "./ExampleProp.vue"

const state = reactive({
  message: 'hieu-san, han-san',
  customMessage: '',
})

const messageWithParameter = ref('')
const messageWithoutParameter = ref('')

// call api get message with msg parameter
fetch('/api/v1/message?msg=' + state.message)
    .then(res => res.text())
    .then(t => messageWithParameter.value = t)
// .then(t => state.message = t) // using reactive

// call api get message without msg parameter
fetch('/api/v1/message')
    .then(res => res.text())
    .then(t => messageWithoutParameter.value = t)

</script>