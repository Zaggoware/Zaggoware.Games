import Vue from 'vue';
import App from '@/components/app/app.vue';

// Portal Vue
import PortalVue from 'portal-vue';
Vue.use(PortalVue);

// CSS Vars
import cssVars from 'css-vars-ponyfill';
cssVars();

// Components
import Layout from '@/components/layout/layout.vue';
import PlayingCard from '@/components/playing-card/playing-card.vue';

Vue.component('layout', Layout);
Vue.component('playing-card', PlayingCard);

// Filters
import capitalize from '@/filters/capitalize';

Vue.filter('capitalize', capitalize);

// Config
Vue.config.productionTip = false;

import router from '@/router';
import i18n from '@/locales/i18n';

new Vue({
    router,
    i18n,
    render: (h): typeof Vue.prototype.$createElement => h(App)
}).$mount('#app');
