import { fetchApi } from '@/api/apis'
const API_PATH_NAME = '/api/v1/adls'

export default {
    async getAll() {
        const res = await fetchApi('GET', API_PATH_NAME + '/get-from-adls')
        if (res) {
            return await res.json()
        }
    },
    async delete(requestInput) {
        const res = await fetchApi('POST', API_PATH_NAME + '/delete-from-adls', null, requestInput)
        if (res) {
            return await res.json()
        }
    },
    async restore() {
        const res = await fetchApi('GET', API_PATH_NAME + '/restore-from-adls')
        if (res) {
            return await res.json()
        }
    },
    async transfer(requestInput) {
        const res = await fetchApi('POST', API_PATH_NAME + '/transfer', null, requestInput)
        if (res) {
            return await res.json()
        }
    }
}