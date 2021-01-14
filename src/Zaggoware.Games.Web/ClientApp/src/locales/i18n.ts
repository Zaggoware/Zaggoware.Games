import VueI18n from 'vue-i18n';
import { Vue } from 'vue-property-decorator';
import en from './en';

Vue.use(VueI18n);

const locale = 'en';
export const messages = {
    en: en,
};

export const i18n: VueI18n = new VueI18n({
    locale,
    messages: messages
});

export default i18n;
