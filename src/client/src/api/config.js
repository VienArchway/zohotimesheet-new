import ZOHO_SETTINGS from '@/lib/zoho.js'

export const APISettings = {
    headers: {
        'Accept': 'application/json',
        'Zoho-Verify-Token': localStorage.getItem('access-token')
    }
}