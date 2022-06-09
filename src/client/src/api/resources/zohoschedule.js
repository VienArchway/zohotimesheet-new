import { fetchApi } from '@/api/apis'
const API_PATH_NAME = '/api/v1/zohoschedule'

export default {
    async get() {
        const res = await fetchApi('GET', API_PATH_NAME + '/status')
        if (res) {
            return await res.json()
        }
        
    },
    async start(requestInput) {
        const res = await fetchApi('POST', API_PATH_NAME + '/start', null, requestInput)
        if (res) {
            return await res.json()
        }
    },
    async stop(requestInput) {
        const res = await fetchApi('POST', API_PATH_NAME + '/stop', null, requestInput)
        if (res) {
            return await res.json()
        }
    }
}