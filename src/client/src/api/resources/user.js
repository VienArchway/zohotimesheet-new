import { fetchApi } from '@/api/apis'

const API_PATH_NAME = '/api/v1/user'

export async function getCurrentUser() {
    const res = await fetchApi('GET', API_PATH_NAME + '/info')
    if (res) {
        return await res.json()
    }
}
export async function getAllUser() {
    const res = await fetchApi('GET', API_PATH_NAME + '/all')
    if (res) {
        return await res.json()
    }
}