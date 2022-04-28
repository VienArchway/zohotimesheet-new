import { createTestingPinia } from '@pinia/testing'
import AppLogin from '../../src/components/app/AppLogin.vue'
import { render, screen } from '@testing-library/vue'
import useAuthStore from '../../src/store/auth'

const renderAppLogin = render(AppLogin, {
    global: {
        plugins: [createTestingPinia()]
    },
    slots: {
        default: 'test'
    }
})

describe('App Login', () => {
    beforeEach(() => {
        renderAppLogin
    })
    afterEach(() => {
        vi.mockClear
    })

    it('call login if authenticated is false', () => {
        const auth = useAuthStore()
        const spyAuthLogin = vi.fn(() => auth.login())
        spyAuthLogin()

        expect(auth.getAuthentication).toBe(false)
        expect(spyAuthLogin).toHaveBeenCalledTimes(1)
        
    })
    
    // it('return slot view if authenticated is true', async () => {
    //     const auth = useAuthStore()
    //     auth.isAuthenticated = true
    //
    //     expect(auth.getAuthentication).toBe(true)
    //     expect(renderAppLogin.emitted()).toBe(/{}/i)
    // })
})