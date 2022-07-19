import { fetchApi } from '@/api/apis'
const API_PATH_NAME = '/api/v1/project'

export default {
    async getAll() {
        const res = await fetchApi('GET', API_PATH_NAME)
        if (res) {
            return await res.json()
        }
    },
    async getProjectDetailAsync(projectNo) {
        const res = await fetchApi('GET', API_PATH_NAME + `/${projectNo}`)
        if (res) {
            return await res.json()
        }
    }
}