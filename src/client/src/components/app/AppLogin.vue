<template>
  <div v-if="isLogin">
    <slot />
  </div>
</template>

<script>
export default {
  name: "AppLogin",
  props: {
    isLogin: Boolean
  }
}
</script>