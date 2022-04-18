import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vuetify from '@vuetify/vite-plugin'

const path = require('path')

export default defineConfig({
  plugins: [
    vue(),
    vuetify({
      autoImport: true, // https://github.com/vuetifyjs/vuetify-loader/tree/next/packages/vite-plugin
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
  }
})
