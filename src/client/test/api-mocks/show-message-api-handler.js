import { rest } from 'msw'

export const defaultMessage = "default message"
export const paramMessage = "with custom test message"
export const unAuthorizedMesage = "Unauthorized"

export const messageApiHandlers = [
    rest.get('/api/v1/message', (req, res, ctx) => {
        return res(ctx.status(200), ctx.text(defaultMessage))
    }),
    rest.get('/api/v1/message', (req, res, ctx) => {
        req.url.searchParams.get('msg')
        return res(ctx.status(200), ctx.text(paramMessage))
    }),
    rest.get('/api/v1/message', (req, res, ctx) => {
        return res(ctx.status(401), ctx.text(unAuthorizedMesage))
    })
]