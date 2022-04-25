import { createRouter } from 'vue-router'
import { setupLayouts } from 'virtual:generated-layouts'
import generatedRoutes from 'virtual:generated-pages'

const routes = setupLayouts(generatedRoutes)
const errorPage = {
    name: 'error',
    path: '/:pathMatch(.*)*',
    component: () => import('@/layouts/error.vue'),
    props: true
}
routes.push(errorPage)

export default function (history) {
    return  createRouter({
        history,
        routes
    })
}