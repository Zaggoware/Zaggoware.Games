import Vue from 'vue';
import VueRouter from 'vue-router';
import { RouteConfig } from 'vue-router';

Vue.use(VueRouter);

export const routes: RouteConfig[] = [
    {
        path: '/',
        component: () => import('@/views/main/main.vue')
    }
];

export const router = new VueRouter({
    mode: 'history',
    routes,
    scrollBehavior: () => ({
        x: 0,
        y: 0
    })
});
export default router;
