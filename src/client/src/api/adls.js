import { APISettings } from './config.js'
import { useRouter } from 'vue-router'

const API_PATH_NAME = '/api/v1/adls'

export default {
    getAll() {
        const router = useRouter() 
        
        return fetch(API_PATH_NAME + '/get-from-adls', {
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
                return response.json()
            }
        })
        .catch((error) => {
            router.push({ name: 'error', params: { errorMessage: error.text, errorStatus: error.status } })
        })
    },
    delete(requestInput) {
        const router = useRouter() 

        return fetch(API_PATH_NAME + '/delete-from-adls', {
            method: 'POST',
            headers: APISettings.headers,
            body: JSON.stringify(requestInput)
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
    restore() {
        const router = useRouter() 
        
        return fetch(API_PATH_NAME + '/restore-from-adls', {
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
                return response.json()
            }
        })
        .catch((error) => {
            router.push({ name: 'error', params: { errorMessage: error.text, errorStatus: error.status } })
        })
    },
    transfer(requestInput) {
        const router = useRouter() 
        
        return fetch(API_PATH_NAME + '/transfer', {
            method: 'POST',
            headers: APISettings.headers,
            body: JSON.stringify(requestInput)
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