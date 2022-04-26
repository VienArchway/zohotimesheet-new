import { setupServer } from 'msw/node'
import { rest } from 'msw'

const defaultMessage = "default message"
const paramMessage = "with custom test message"

export const restHandlers = [
    rest.get('http://localhost:5000/api/v1/message', (req, res, ctx) => {
        return res(ctx.status(200), ctx.text(defaultMessage))
    }),
    rest.get(`http://localhost:5000/api/v1/message?msg=${paramMessage}`, (req, res, ctx) => {
        return res(ctx.status(200), ctx.text(paramMessage))
    })
]

const server = setupServer(...restHandlers)

// Start server before all tests
beforeAll(() => server.listen({ onUnhandledRequest: 'error' }))

//  Close server after all tests
afterAll(() => server.close())

// Reset handlers after each test `important for test isolation`
afterEach(() => server.resetHandlers())
