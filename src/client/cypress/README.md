## Run e2e

- npm run dev:e2e
- npm run test:e2e

Environment will be set `e2e` and have using msw worker to mock api also spy command to login

The app will detect env then start the worker like so

```js
if (import.meta.env.MODE === "e2e") {
    localStorage.setItem('access-token', 'test login')
    const auth = useAuthStore()
    auth.isAuthenticated = true
    if (auth.getAuthentication) {
        console.log('run e2e')
        worker.start()
    }
    
    if (window.Cypress) {
        window.appReady = true
    }
}
```