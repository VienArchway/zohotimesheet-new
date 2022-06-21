import { onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { getAccessTokenFromCode, getZohoUserDisplayName, logout } from '@/api/resources/zohoToken'
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
            window.location.href = `${ZOHO_SETTINGS.login_url}?scope=${ZOHO_SETTINGS.scope}&client_id=${ZOHO_SETTINGS.client_id}&response_type=${ZOHO_SETTINGS.response_type}&redirect_uri=${ZOHO_SETTINGS.redirect_uri}&prompt=consent&access_type=offline`
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
            await getZohoUserDisplayName()
            
            window.location.href = '/'
        }
    }

    onMounted(async () => {
        await app.load(async () => {
            await handleRedirectFromCallBack()
        })
    })
}