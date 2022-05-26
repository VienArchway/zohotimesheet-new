import { APISettings } from './config.js'
import { useRouter } from 'vue-router'

const API_PATH_NAME = '/api/v1/logwork'

export default {

    find(requestInput) {  
    const router = useRouter() 
        
        return fetch(API_PATH_NAME, {
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