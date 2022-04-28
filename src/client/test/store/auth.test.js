import { setActivePinia, createPinia } from 'pinia'
import useAuthStore from '../../src/store/auth'

describe('Auth Store', () => {
    beforeEach(() => {
        setActivePinia(createPinia())
    })
    afterEach(() => {
        vi.mockClear
    })
    
    it('get state', () => {
        const auth = useAuthStore()
        expect(auth.isAuthenticated).toBe(false)
    })
    
    it('change state', () => {
        const auth = useAuthStore()
        auth.isAuthenticated = true
        expect(auth.isAuthenticated).toBe(true)
    })
    
    it('getters getAuthentication', () => {
        const auth = useAuthStore()
        expect(auth.getAuthentication).toBe(false)
        auth.isAuthenticated = true
        expect(auth.getAuthentication).toBe(true)
    })

    it('actions login with have access-token return isAuthenticated true', () => {
        const auth = useAuthStore()
        const mockToken = vi.fn().mockImplementation(() => window.localStorage.setItem('access-token', 'have token'))
        mockToken()
        
        const spyAuth = vi.fn(() => auth.login())
        spyAuth()
        
        expect(spyAuth).toHaveBeenCalledTimes(1)
        expect(auth.isAuthenticated).toBe(true)
    })

    it('actions login with have not access-token return href navigation', () => {
        const auth = useAuthStore()

        const mockToken = vi.fn().mockImplementation(() => {
            window.localStorage.setItem('access-token', '')
        })
        mockToken()

        delete window.location
        window.location = { href: vi.fn() }
        
        const spyAuth = vi.fn(() => auth.login())
        spyAuth()
        
        const mockWindowHref = vi.fn(() => {
            window.location.href = 'http://zoho.com/redirectLogin'
        })
        mockWindowHref()

        expect(window.location.href).toEqual('http://zoho.com/redirectLogin')
        expect(spyAuth).toHaveBeenCalledTimes(1)
        expect(auth.isAuthenticated).toBe(false)
    })
})