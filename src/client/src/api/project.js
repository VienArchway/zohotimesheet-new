import { APISettings } from './config.js'

const API_PATH_NAME = '/api/v1/project'

export default {
    getAll() {
        return fetch(API_PATH_NAME, {
            method: 'GET',
            headers: APISettings.headers
        })
    }
}