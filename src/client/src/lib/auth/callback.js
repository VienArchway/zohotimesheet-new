import { onMounted, ref } from 'vue'
import { useRoute } from 'vue-router'
import ZOHO_SETTINGS from '@/lib/zoho.js'

export function useHandleCallBack() {
    const route = useRoute()
    const code = ref(null)

    const handleRedirectFromCallBack = async () => {
        if (route.query?.code) {
            code.value = route.query.code
            const getToken = await fetch(`/zohoauth/${ZOHO_SETTINGS.token_path}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                },
                body: new URLSearchParams({
                    code: code.value,
                    client_id: ZOHO_SETTINGS.client_id,
                    client_secret: ZOHO_SETTINGS.client_secret,
                    redirect_uri: ZOHO_SETTINGS.redirect_uri,
                    grant_type: ZOHO_SETTINGS.grant_type
                })
            })
            const data = await getToken.json()

            if (data.error === 'invalid_code') {
                return console.error('invalid code')
            }

            localStorage.setItem('access-token', data?.access_token)
            localStorage.setItem('refresh-token', data?.refresh_token)
            localStorage.setItem('expired', data?.expires_in)

            window.location.href = '/'
        } else {
            console.error('Can not get code')
        }
    }

    onMounted(() => handleRedirectFromCallBack())
}