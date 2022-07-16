import { fetchApi } from '@/api/apis'
const API_PATH_NAME = '/api/v1/sprint'

export default {
    async search(projId) {
        const res = await fetchApi('GET', API_PATH_NAME + `?ProjectId=${projId}`)
        if (res) {
            return await res.json()
        }
    }
}