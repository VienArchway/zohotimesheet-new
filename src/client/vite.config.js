import { defineConfig, loadEnv } from 'vite'
import vue from '@vitejs/plugin-vue'
import { quasar, transformAssetUrls } from '@quasar/vite-plugin'
import vuetify from '@vuetify/vite-plugin'
import VueI18n from '@intlify/vite-plugin-vue-i18n'
import Pages from 'vite-plugin-pages'
import Layouts from 'vite-plugin-vue-layouts'

const path = require('path')

export default ({ mode }) => {
  process.env = {...process.env, ...loadEnv(mode, process.cwd(), '')}

  return defineConfig({
    build: {
      minify: false
    },
    plugins: [
      vue({
        template: { transformAssetUrls }
      }),
      vuetify({
        autoImport: true, // https://github.com/vuetifyjs/vuetify-loader/tree/next/packages/vite-plugin
      }),
      VueI18n({
        runtimeOnly: false,
        compositionOnly: false,
        include: [path.resolve(__dirname, './i18n/**')],
      }),
      Pages({
        dirs: [
          { dir: 'src/pages', baseRoute: '' },
        ],
      }),
      Layouts({
        defaultLayout: 'default'
      }),
      quasar({
        sassVariables: 'src/quasar-variables.sass'
      })
    ],
    define: { 'process.env': {} },
    envPrefix: 'FE_',
    resolve: {
      alias: {
        '@': path.resolve(__dirname, 'src'),
      },
    },
    server: {
      proxy: {
        '/api': process.env.FE_API_URL, // config server api
        '/zohoauth': {
          target: 'https://accounts.zoho.com',
          changeOrigin: true,
          rewrite: (path) => path.replace(/^\/zohoauth/, ''),
        },
      }
    },
    test: {
      globals: true,
      environment: 'jsdom'
    },
  })
}
