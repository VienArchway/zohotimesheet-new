import { APISettings } from './config.js'
import { useRouter } from 'vue-router'

const API_PATH_NAME = '/api/v1/zohoschedule'

export default {
    get() {  
        const router = useRouter() 
        
        return fetch(API_PATH_NAME + '/status', {
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
    start(requestInput) {    
        const router = useRouter() 
            
        return fetch(API_PATH_NAME + '/start', {
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
    stop(requestInput) {    
        const router = useRouter() 
            
        return fetch(API_PATH_NAME + '/stop', {
            method: 'GET',
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