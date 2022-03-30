import { createRouter } from 'vue-router'
import Homepage from './home/Home.vue'
import About from './about/About.vue'
import AuthCallback from './auth/Callback.vue'


const routes = [
    {
        path: '/',
        component: Homepage
    },

    {
        path: '/about/',
        component: About
    },

    {
        path: '/auth/callback',
        component: AuthCallback
    }
]

export default function (history) {
    return createRouter({
        history,
        routes
    })
}