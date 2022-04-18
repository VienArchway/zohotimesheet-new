import { APISettings } from '../config.js'

const API_PATH_NAME = '/api/v1/message'

export default {
    getDefaultMessage() {
        return fetch(API_PATH_NAME, {
            method: 'GET',
            // headers: APISettings.headers
        })
        .then((response) => {
            if (response.status !== 200) {
                throw new Error(response.statusText)
            } else {
                return response.text()
            }
        })
        .catch((error) => {
            console.error(error)
        })
    },
    
    getMessageWithParam(msg) {
        return fetch(API_PATH_NAME + `?msg=${msg}`, {
            method: 'GET',
            // headers: APISettings.headers
        })
        .then((response) => {
            if (response.status !== 200) {
                throw new Error(response.statusText)
            } else {
                return response.text()
            }
        })
    }
}