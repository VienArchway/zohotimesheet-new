import { createApp } from 'vue'
import App from './App.vue'
import store from './lib/store.js'
import router from './lib/router.js'
import vuetify from './plugins/vuetify/vuetify'
import { loadFonts } from './plugins/vuetify/webfontloader'
import axios from 'axios'
import VueAxios from 'vue-axios'

loadFonts()

const app = createApp(App)
app
    .use(router)
    .use(store)
    .use(vuetify)
    .use(VueAxios, axios)
app.provide('axios', app.config.globalProperties.axios)
app.mount('#app')
