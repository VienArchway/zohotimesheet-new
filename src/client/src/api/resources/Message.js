import { APISettings } from '../config.js'
import { useRouter } from 'vue-router'

const API_PATH_NAME = '/api/v1/message'

export function getDefaultMessageApi() {
    const router = useRouter()
    
    return fetch(API_PATH_NAME, {
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

export function getMessageWithParamApi(msg) {
    const router = useRouter()

    return fetch(API_PATH_NAME + `?msg=${msg}`, {
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