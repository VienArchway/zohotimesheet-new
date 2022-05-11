import { onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { getAccessTokenByRefreshTokenApi, getAccessTokenFromCode } from '@/api/resources/ZohoToken.js'
import ZOHO_SETTINGS from '@/lib/zoho.js'

export function useHandleCallBack() {
    const route = useRoute()
    const router = useRouter()

    const handleRedirectFromCallBack = async () => {
        if (route.query?.code) {
            const { accessToken, refreshToken } = await getAccessTokenFromCode(route.query.code)
            if (!accessToken && !refreshToken) {
                console.error('invalid code')
                return router.push({ name: 'error', params: {
                        errorMessage: 'Invalid code. Please contact admin',
                        errorStatus: '500'
                    }
                })
            }

            localStorage.setItem('access-token', accessToken)
            if (refreshToken) {
               localStorage.setItem('refresh-token', refreshToken)
            }

            window.location.href = '/'
        } else {
            const refreshToken = localStorage.getItem('refresh-token')
            if (refreshToken === 'undefined' || !refreshToken) {
               return router.push({ name: 'error', params: {
                     errorMessage: 'Refresh token has undefined. Please contact admin',
                     errorStatus: '500'
                  }
               })
            }

            const { accessToken } = await getAccessTokenByRefreshTokenApi(refreshToken)
            if (accessToken) {
                localStorage.setItem('access-token', accessToken)
                window.location.href = '/'
            } else {
                console.error('Failed when try to get access token. Please contact admin')
            }
        }
    }

    onMounted(() => handleRedirectFromCallBack())
}