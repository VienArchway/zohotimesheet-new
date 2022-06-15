import { fetchApi } from '@/api/apis'
const API_PATH_NAME = '/api/v1/webhook'

export default {
    async getAll(accessToken) {
        const res = await fetchApi('GET', API_PATH_NAME)
        if (res) {
            return await res.json()
        }
    },
    async update(requestInput) {
        const res = await fetchApi('POST', API_PATH_NAME + '/update-status', null, requestInput)
        if (res) {
            return await res.json()
        }
    }
}
