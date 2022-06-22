import { defineStore } from 'pinia'

export default defineStore('app', {
    state: () => ({
        zsUserId: localStorage.getItem('zsUserId'),
        isLoading: false,
        messages: []
    }),
    actions: {
        async load(callback) {
            this.isLoading = true
            await callback()
            this.isLoading = false
        },
        error(content, timeout) {
            this.message("error", content, timeout);
        },
        success(content, timeout) {
            this.message("success", content, timeout);
        },
        info(content, timeout) {
            this.message("info", content, timeout);
        },
        warning(content, timeout) {
            this.message("warning", content, timeout);
        },
        message(type, content, timeout) {
            const id = Date.now();
            this.messages.push({ id, type, content })
            if (timeout)
            setTimeout(() => {
                this.messages = this.messages.filter(m => m.id != id)
            }, timeout);
        },
        clearAllMessages() {
            this.messages = []
        }
    }
})