<template></template>
<script setup>
import { onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()

const code = ref(null)

const handleRedirectFromCallBack = async () => {
  // check route query
  // if have -> send it to get access token, refresh token, and redirect to home page
  // TODO: store token
  if (route.query?.code) {
    code.value = route.query.code
    const getToken = await fetch('/zohoauth/oauth/v2/token', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded'
      },
      body: new URLSearchParams({
        code: code.value,
        client_id: import.meta.env.VITE_ZOHO_CLIENT_ID,
        client_secret: import.meta.env.VITE_ZOHO_CLIENT_SECRET,
        redirect_uri: import.meta.env.VITE_ZOHO_REDIRECT_URI,
        grant_type: import.meta.env.VITE_ZOHO_CODE_GRANT_TYPE
      })
    })
    const data = await getToken.json()

    if (data.error === 'invalid_code') {
      console.error('invalid code')
    }

    console.log('get token success:', data)

    localStorage.setItem('access-token', data?.access_token)
    localStorage.setItem('refresh-token', data?.refresh_token)
    localStorage.setItem('expired', data?.expires_in)
    document.cookie = data?.refresh_token

    router.push({path: '/'})
  } else {
    console.error('Can not get code')
  }
}

onMounted(async () => {
  await handleRedirectFromCallBack()
})

</script>
<style scoped>

</style>