import { createApp } from 'vue'
import App from './App.vue'
import store from '@/lib/store.js'
import router from '@/lib/router.js'
import i18n from '@/lib/i18n.js'
import vuetify from '@/plugins/vuetify/vuetify'
import { loadFonts } from '@/plugins/vuetify/webfontloader'

loadFonts()

// TODO: handle auth state
// router.beforeEach((to, from, next) => {
// })

const app = createApp(App)
app
    .use(i18n)
    .use(router)
    .use(store)
    .use(vuetify)
app.mount('#app')
