<template>
  <h1>Callback</h1>
  {{ code }}
</template>

<script setup>
import { onMounted, ref, reactive } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()
console.log(route.query.code)

const code = ref(null)
const error = ref({})
const dataAuth = ref({})

const handleRedirectFromCallBack = async () => {
  // check route query
  // if have -> send it to get access token, refresh token, and redirect to home page with code
  // set refresh token to local storage
  if (route.query?.code) {
    code.value = route.query.code
    fetch('/zohoauth/oauth/v2/token', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded'
      },
      body: new URLSearchParams({
        code: code.value,
        client_id: '1000.1KQGLWBUATFDBEE1S19FVN59D339GN',
        client_secret: '161c801bdade439069fe07d6c552807e106710d480',
        redirect_uri: 'http://localhost:3000/auth/callback',
        grant_type: 'authorization_code'
      })
    })
        .then(response => response.json())
        .then(data => {
          if (data.error === 'invalid_code') {
            // show error page: contact 
          }
          console.log('Success:', data)
          dataAuth.value = data
          // set to local storage
          localStorage.setItem('access-token', dataAuth.value?.access_token)
          localStorage.setItem('refresh-token', dataAuth.value?.refresh_token)

          router.push({path: '/'})
        })
        .catch((error) => {
          console.error('Error:', error);
        })
  } else {
    // else -> throw error interceptor
  }
}



onMounted(() => {
  return handleRedirectFromCallBack()
})

</script>

<style scoped>

</style>