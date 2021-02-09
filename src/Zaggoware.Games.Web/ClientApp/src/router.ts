import Vue from 'vue';
import VueRouter from 'vue-router';
import { RouteConfig } from 'vue-router';

Vue.use(VueRouter);

export const routes: RouteConfig[] = [
    {
        path: '/',
        component: () => import('@/views/lobby/lobby-overview.vue')
    },
    {
        path: '/game',
        component: () => import('@/views/game/game.vue'),
        meta: {
            layout: 'bare'
        },
        children: [{
            path: 'crazy-eights/:id',
            component: () => import('@/views/game/crazy-eights/crazy-eights-game.vue'),
            meta: {
                layout: 'bare'
            }
        }]
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
