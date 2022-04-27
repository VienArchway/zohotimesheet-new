import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vuetify from '@vuetify/vite-plugin'
import VueI18n from '@intlify/vite-plugin-vue-i18n'
import Pages from 'vite-plugin-pages'
import Layouts from 'vite-plugin-vue-layouts'

const path = require('path')

export default defineConfig({
  plugins: [
    vue(),
    vuetify({
      autoImport: true, // https://github.com/vuetifyjs/vuetify-loader/tree/next/packages/vite-plugin
    }),
    VueI18n({
      runtimeOnly: true,
      compositionOnly: true,
      include: [path.resolve(__dirname, 'i18n/**')],
    }),
    Pages({
      dirs: [
        { dir: 'src/pages', baseRoute: '' },
      ],
    }),
    Layouts({
      defaultLayout: 'default'
    }),
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
      '/api': 'http://localhost:5000', // config server api
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
