import ZOHO_SETTINGS from '@/lib/zoho.js'

export const APISettings = {
    headers: {
        'Accept': 'application/json, text/plain, */*',
        'Zoho-Verify-Token': localStorage.getItem('access-token'),
        'Content-Type': 'application/json'
    }
}