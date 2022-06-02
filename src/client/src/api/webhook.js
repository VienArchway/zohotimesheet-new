import { APISettings } from './config.js'
import { useRouter } from 'vue-router'

const API_PATH_NAME = '/api/v1/webhook'

export default {
    getAll(accessToken) {
        const router = useRouter() 
        
        return fetch(API_PATH_NAME, {
            method: 'GET',
            headers: {
                'Accept': 'application/json, text/plain, */*',
                'Zoho-Verify-Token': accessToken,
                'Content-Type': 'application/json'
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
    },
    update(accessToken, requestInput) {    
        const router = useRouter() 
            
        return fetch(API_PATH_NAME + '/update-status', {
            method: 'POST',
            headers: {
                'Accept': 'application/json, text/plain, */*',
                'Zoho-Verify-Token': accessToken,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(requestInput)
        })
        .then((response) => {
            if (response.status !== 201) {
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