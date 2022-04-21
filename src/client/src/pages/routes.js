import { createRouter } from 'vue-router'
import routes from '~pages'

const errorPage =  {
    name: 'error',
    path: '/:pathMatch(.*)*',
    component: () => import('@/components/app/AppErrorPage.vue'),
    props: true
}

routes.push(errorPage)

export default function (history) {
    return  createRouter({
        history,
        routes
    })
}