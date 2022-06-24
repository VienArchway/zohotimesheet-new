import { onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { getAccessTokenFromCode, getAccessTokenByRefreshTokenApi, logout } from '@/api/resources/zohoToken'
import { getCurrentUser } from '@/api/resources/user'
import ZOHO_SETTINGS from '@/lib/zoho'
import useAppStore from '@/store/app'

const app = useAppStore()

export function useHandleCallBack() {
    const route = useRoute()
    const router = useRouter()

    const handleRedirectFromCallBack = async () => {
        // logout
        if (route.query?.logout) {
            await logout()
            localStorage.setItem('authorized', false)
            window.location.href = '/'
        }
        
        // revoke token
        if (route.query?.revoke) {
            const firstName = localStorage.getItem('firstName')
            const zpUserId = localStorage.getItem('zpUserId')
            if (firstName && zpUserId) {
                const { accessToken } = await getAccessTokenByRefreshTokenApi(firstName, zpUserId)
                if (accessToken) {
                    localStorage.setItem('authorized', true)
                    window.location.href = '/'
                } else {
                    console.error('Failed when try to get access token. Please contact admin')
                }
            } else {
                window.location.href = `${ZOHO_SETTINGS.login_url}?scope=${ZOHO_SETTINGS.scope}&client_id=${ZOHO_SETTINGS.client_id}&response_type=${ZOHO_SETTINGS.response_type}&redirect_uri=${ZOHO_SETTINGS.redirect_uri}&prompt=consent&access_type=offline`
            }
        }

        // login
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
            var user = await getCurrentUser()
            localStorage.setItem('firstName', user.firstName)
            localStorage.setItem('zsUserId', user.zsUserId)
            
            window.location.href = '/'
        }
    }

    onMounted(async () => {
        await app.load(async () => {
            await handleRedirectFromCallBack()
        })
    })
}