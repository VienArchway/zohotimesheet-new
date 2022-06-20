import { fetchApi } from '@/api/apis'

const API_PATH_NAME = '/api/v1/zohotoken'

export async function getVerifyTokenApi() {
    const res = await fetchApi('GET', API_PATH_NAME + '/verify-token')
    if (res) {
        return await res.text()
    }
}

export async function getAccessTokenByRefreshTokenApi() {
    const res = await fetchApi('GET', API_PATH_NAME + '/refresh-access-token')
    if (res) {
        return await res.json()
    }
}

export async function getAccessTokenFromCode(code) {
    const res = await fetchApi('GET', API_PATH_NAME, {param: `?code=${code}` })
    if (res) {
        return await res.json()
    }
    // 'Content-Type': 'application/x-www-form-urlencoded'
}

export async function getZohoUserDisplayName() {
    const res = await fetchApi('GET', API_PATH_NAME + '/display-name')
    if (res) {
        const displayName = await res.text()
        localStorage.setItem('displayName', displayName)
        return displayName
    }
}