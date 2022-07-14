import { fetchApi } from '@/api/apis'
const API_PATH_NAME = '/api/v1/project'

export default {
    async getAll() {
        const res = await fetchApi('GET', API_PATH_NAME)
        if (res) {
            return await res.json()
        }
    },
    async getProjectPriorityAsync(projectId) {
        const res = await fetchApi('GET', API_PATH_NAME + `/priority/${projectId}`)
        if (res) {
            return await res.json()
        }
    },
    async getProjectItemTypeAsync(projectId) {
        const res = await fetchApi('GET', API_PATH_NAME + `/item-type/${projectId}`)
        if (res) {
            return await res.json()
        }
    }
}