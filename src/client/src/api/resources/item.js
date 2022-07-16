import { fetchApi } from '@/api/apis'
import { useRouter } from 'vue-router'

const API_PATH_NAME = '/api/v1/taskitem'

export default {
    async find(requestInput) {
        const res = await fetchApi('POST', API_PATH_NAME + '/search', null, requestInput)
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
    async updateStatus(requestInput) {
        await fetchApi('POST', API_PATH_NAME + '/update-status', null, requestInput)
    },
    async deleteItem(requestInput) {
        await fetchApi('POST', API_PATH_NAME + '/delete', null, requestInput)
    }
}