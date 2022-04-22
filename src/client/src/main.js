import { createApp } from 'vue'
import App from './App.vue'
import pinia from '@/lib/store.js'
import router from '@/lib/router.js'
import i18n from '@/lib/i18n.js'
import vuetify from '@/plugins/vuetify/vuetify'
import { loadFonts } from '@/plugins/vuetify/webfontloader'
import useAuthStore from '@/store/auth.js'
import ZOHO_SETTINGS from '@/lib/zoho.js'

loadFonts()

const app = createApp(App)
app
    .use(pinia)
    .use(i18n)
    .use(router)
    .use(vuetify)
app.mount('#app')
