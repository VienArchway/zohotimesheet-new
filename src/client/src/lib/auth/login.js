import { ref, onMounted } from 'vue'
import ZOHO_SETTINGS from '@/lib/zoho.js'

export function useLogin() {
    const isLogin = ref(false)

    function login() {
        if (!localStorage.getItem('access-token')) {
            window.location.href = `${ZOHO_SETTINGS.login_url}?scope=${ZOHO_SETTINGS.scope}&client_id=${ZOHO_SETTINGS.client_id}&response_type=${ZOHO_SETTINGS.response_type}&redirect_uri=${ZOHO_SETTINGS.redirect_uri}`
        } else {
            isLogin.value = true
        }
    }
    
    onMounted(() => login())
    
    return { isLogin }
}