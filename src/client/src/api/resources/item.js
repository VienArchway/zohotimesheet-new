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
    async updateStatus(requestInput) {
        const res = await fetchApi('POST', API_PATH_NAME + '/update-status', null, requestInput)
        if (res) {
            return await res.json()
        }
    }
}