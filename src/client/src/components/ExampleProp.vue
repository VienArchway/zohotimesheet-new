<script setup>
const props = defineProps({
  message: String
})
</script>
<template>
  <div>
    <h1>Message here</h1>
    <span>{{ props.message }}</span>
  </div>
</template>

<script>
export default {
  name: "ExampleMessageCallApi"
}
</script>

<style scoped>

</style>