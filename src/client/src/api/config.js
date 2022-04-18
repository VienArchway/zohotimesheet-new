export const APISettings = {
    headers: new Headers({
        'Accept': 'application/json',
        'Zoho-Verify-Token': localStorage.getItem('access-token'),
        'Zoho-Client-Id': import.meta.env.FE_ZOHO_CLIENT_ID,
        // 'Zoho-Expired': localStorage.getItem('expired')
    })
}