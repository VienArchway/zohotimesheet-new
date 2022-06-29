import { fetchApi } from '@/api/apis'
const API_PATH_NAME = '/api/v1/logwork'

export default {
    async find(requestInput) {
        const res = await fetchApi('POST', API_PATH_NAME, null, requestInput)
        if (res) {
            return await res.json()
        }
    },
    async create(requestInput) {
        const res = await fetchApi('POST', API_PATH_NAME + '/create', null, requestInput)
        if (res) {
            return await res.json()
        }
    },
    async update(requestInput) {
        await fetchApi('PUT', API_PATH_NAME, null, requestInput)
    }
}