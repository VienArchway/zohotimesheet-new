import { APISettings } from '../config.js'
import { useRouter } from 'vue-router'

const API_PATH_NAME = '/api/v1/zohotoken'

export function getVerifyTokenApi() {
    const router = useRouter()
    
    return fetch(API_PATH_NAME + '/verify-token', {
        method: 'GET',
        headers: APISettings.headers
    })
    .then((response) => {
        if (response.status !== 200) {
            console.error(response.statusText)
            const error = {
                text: response.statusText,
                status: response.status
            }
            throw error
        } else {
            return response.text()
        }
    })
    .catch((error) => {
        router.push({ name: 'error', params: { errorMessage: error.text, errorStatus: error.status } })
    })
}

export function getAccessTokenByRefreshTokenApi(refreshToken) {
    const router = useRouter()

    return fetch(API_PATH_NAME + '/refresh-access-token' + `?refreshToken=${refreshToken}`, {
        method: 'GET',
        headers: { ...APISettings.headers, 'Zoho-Refresh-Token': refreshToken }
    })
    .then((response) => {
        if (response.status !== 200) {
            console.error(response.statusText)
            const error = {
                text: response.statusText,
                status: response.status
            }
            throw error
        } else {
            return response.json()
        }
    })
    .catch((error) => {
        router.push({ name: 'error', params: { errorMessage: error.text, errorStatus: error.status } })
    })
}

export function getAccessTokenFromCode(code) {
    const router = useRouter()
    
    return fetch(API_PATH_NAME + `?code=${code}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
            'Is-Redirect-Uri': true
        }
    })
    .then((response) => {
        if (response.status !== 200) {
            console.error(response.statusText)
            const error = {
                text: response.statusText,
                status: response.status
            }
            throw error
        } else {
            return response.json()
        }
    })
    .catch((error) => {
        router.push({ name: 'error', params: { errorMessage: error.text, errorStatus: error.status } })
    })
}