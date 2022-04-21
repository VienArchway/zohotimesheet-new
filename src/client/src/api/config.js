import ZOHO_SETTINGS from '@/lib/zoho.js'

export const APISettings = {
    headers: new Headers({
        'Accept': 'application/json',
        'Zoho-Verify-Token': localStorage.getItem('access-token'),
        'Zoho-Client-Id': ZOHO_SETTINGS.client_id,
        // 'Zoho-Expired': localStorage.getItem('expired')
    })
}