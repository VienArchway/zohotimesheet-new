import { setupServer } from 'msw/node'
import { render, screen } from '@testing-library/vue'
import { getDefaultMessageApi, getMessageWithParamApi } from '../../src/api/resources/Message'
import { messageApiHandlers, defaultMessage, paramMessage, unAuthorizedMesage } from '../api-mocks/show-message-api-handler'
import ShowMessage from '../../src/components/ShowMessage.vue'
import 'whatwg-fetch'

// mock router push
const routerPushMock = vi.fn()
vi.mock('vue-router', () => ({
    useRouter: () => ({
        push: routerPushMock,
    }),
}))
// regist server api
const server = setupServer(...messageApiHandlers)

describe('ShowMessage', () => {
    beforeEach(() => {
        server.listen()
    })
    afterEach(() => {
        server.close()
        server.resetHandlers()
    })
    
    it('should get default as one and return text message', async () => {
        server.use(messageApiHandlers[0])
        render(ShowMessage)

        const mockDefaultMessage = vi.fn().mockImplementation(getDefaultMessageApi)
        await mockDefaultMessage()
        
        const defaultMsg = await screen.getByTestId(/default-ms/i)
        console.log(defaultMsg.innerHTML)
        expect(defaultMsg.innerHTML).toEqual('default: default message')
        expect(mockDefaultMessage).toHaveBeenCalled(1)
    })

    it('should get param message as one and return text message', async () => {
        server.use(messageApiHandlers[1])
        render(ShowMessage)
        
        const mockWithParamMessage = vi.fn().mockImplementation(getMessageWithParamApi)
        await mockWithParamMessage()

        const paramMsg = await screen.getByTestId(/custom-ms/i)
        console.log(paramMsg.innerHTML)
        expect(mockWithParamMessage).toHaveBeenCalled(1)
        expect(paramMsg.innerHTML).toEqual(`with msg: ${paramMessage}`)
    })

    it('should return unthorized', async () => {
        server.use(messageApiHandlers[2])
        render(ShowMessage)

        const mockUnthorized = vi.fn().mockImplementation(getDefaultMessageApi)
        await mockUnthorized()

        expect(mockUnthorized).toBeTruthy()
        expect(mockUnthorized).toHaveBeenCalled(1)
    })
})