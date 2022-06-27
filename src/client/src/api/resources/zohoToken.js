import { fetchApi } from '@/api/apis'
const API_PATH_NAME = '/api/v1/zohotoken'

export async function getAccessTokenByRefreshTokenApi(firstName, zsUserId) {
    const res = await fetchApi('GET', API_PATH_NAME + '/refresh-access-token', { param: `?firstName=${firstName}&zsUserId=${zsUserId}`})
    if (res) {
        return await res.json()
    }
}

export async function getAccessTokenFromCode(code) {
    const res = await fetchApi('GET', API_PATH_NAME, {param: `?code=${code}` })
    if (res) {
        return await res.json()
    }
}

export async function logout() {
    const res = await fetchApi('GET', API_PATH_NAME + '/logout')
    if (res) {
        return
    }
}