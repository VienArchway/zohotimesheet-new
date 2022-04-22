import { defineStore } from 'pinia'
import ZOHO_SETTINGS from '@/lib/zoho.js'

const useAuthStore = defineStore('authStore', {
    state: () => ({
        isAuthenticated: false
    }),
    getters: {
        getAuthentication: (state) => {
            return state.isAuthenticated
        }
    },
    actions: {
        login() {
            if (!localStorage.getItem('access-token')) {
                window.location.href = `${ZOHO_SETTINGS.login_url}?scope=${ZOHO_SETTINGS.scope}&client_id=${ZOHO_SETTINGS.client_id}&response_type=${ZOHO_SETTINGS.response_type}&redirect_uri=${ZOHO_SETTINGS.redirect_uri}`
            } else {
                this.isAuthenticated = true
            }
        }
    }
})

export default useAuthStore