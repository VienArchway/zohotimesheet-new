import { createI18n } from 'vue-i18n'
import en from '@/i18n/en.json'
import ja from '@/i18n/ja.json'

export default createI18n({
    legacy: false,
    locale: navigator.language.split('-')[0] || process.env.VUE_APP_I18N_LOCALE || 'en',
    messages: {
        en,
        ja
    }
})