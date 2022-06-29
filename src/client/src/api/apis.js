import { ApiHeaderSettings } from '@/api/config'
import router from '@/lib/router'

export const fetchApi = async (methodName, pathUrl, requestParam ,requestBody) => {
    try {
        const searchParam = new URLSearchParams(requestParam)
        if (searchParam.has('param')) {
            pathUrl = pathUrl + searchParam.get('param')
        }
        
        if (!requestBody) {
            requestBody = null
        } else {
            requestBody = JSON.stringify(requestBody)
        }
        
        const response = await fetch(pathUrl, {
            method: methodName,
            headers: ApiHeaderSettings,
            body: requestBody
        })
        if (!response.ok) {
            const { statusText, status } = response
            console.error(`${statusText} ${status}`)
            const error = {
                text: statusText,
                status: status
            }
            throw error
        }
        return response
    } catch (error) {
        router.push({ name: 'error', params: { errorMessage: error.text, errorStatus: error.status } })        
    }
}