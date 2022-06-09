import { onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { getAccessTokenByRefreshTokenApi, getAccessTokenFromCode } from '@/api/resources/zohoToken'
import ZOHO_SETTINGS from '@/lib/zoho.js'

export function useHandleCallBack() {
    const route = useRoute()
    const router = useRouter()

    const handleRedirectFromCallBack = async () => {
        if (route.query?.code) {
            const { accessToken } = await getAccessTokenFromCode(route.query.code)
            if (!accessToken) {
                console.error('invalid code')
                return router.push({ name: 'error', params: {
                        errorMessage: 'Invalid code. Please contact admin',
                        errorStatus: '500'
                    }
                })
            }

            localStorage.setItem('authorized', true)
            window.location.href = '/'
        } else {
            const { accessToken } = await getAccessTokenByRefreshTokenApi()
            if (accessToken) {
                localStorage.setItem('authorized', true)
                window.location.href = '/'
            } else {
                console.error('Failed when try to get access token. Please contact admin')
            }
        }
    }

    onMounted(async () => { await handleRedirectFromCallBack() })
}