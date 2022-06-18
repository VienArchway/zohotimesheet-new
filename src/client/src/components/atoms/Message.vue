<script setup>
import appStore from '@/store/app.js'

const app = appStore()
</script>


<template>
    <div class="notification-section">
        <div v-for="(message) in app.messages" :key="`message-${message.id}`">
            <v-alert :type="message.type" closable min-width="350">{{ message.content }}</v-alert>
        </div>
    </div>
</template>

<script>
export default {
    name: "Message"
}
</script>

<style scoped>
.notification-section {
    position: absolute;
    bottom: 5px;
    right: 5px;
    z-index: 1000;
}
</style>