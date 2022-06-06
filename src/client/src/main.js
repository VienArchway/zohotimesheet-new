import { createApp } from 'vue'
import App from './App.vue'
import pinia from '@/lib/store.js'
import router from '@/lib/router.js'
import i18n from '@/lib/i18n.js'
import vuetify from '@/plugins/vuetify/vuetify'
import { loadFonts } from '@/plugins/vuetify/webfontloader'
import useAuthStore from '@/store/auth.js'
import { worker } from '@/mocks/browser.js'
import _ from "lodash"

loadFonts()

const app = createApp(App)
app
    .use(pinia)
    .use(i18n)
    .use(router)
    .use(vuetify)
    window._ = _

if (import.meta.env.MODE === "e2e") {
    localStorage.setItem('access-token', 'test login')
    const auth = useAuthStore()
    auth.isAuthenticated = true
    if (auth.getAuthentication) {
        console.log('run e2e')
        worker.start()
    }
    
    if (window.Cypress) {
        window.appReady = true
    }
}

app.mount('#app')
