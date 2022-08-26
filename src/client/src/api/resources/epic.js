import { fetchApi } from '@/api/apis'
const API_PATH_NAME = '/api/v1/epic'

export default {
    async search(projId) {
        const res = await fetchApi('GET', API_PATH_NAME + `/${projId}`)
        if (res) {
            return await res.json()
        }
    }
}