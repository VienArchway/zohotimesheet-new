import { APISettings } from './config.js'

const API_PATH_NAME = '/api/v1/logwork'

export default {
    find(requestInput) {
        return fetch(API_PATH_NAME, {
            method: 'POST',
            headers: APISettings.headers,
            body: {
                requestInput
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
}