import { rest } from 'msw'

export const unAuthorizedMesage = "Unauthorized"

export const zohoTokenApiHandlers = [
    rest.get('/api/v1/zohotoken/verify-token', (req, res, ctx) => {
        return res(ctx.status(200), ctx.text('success'))
    }),
    rest.get('/api/v1/zohotoken/get-access-by-refresh-token', (req, res, ctx) => {
        req.url.searchParams.get('refreshToken')
        return res(ctx.status(200), ctx.text('refresh token'))
    }),
    rest.get('/api/v1/zohotoken', (req, res, ctx) => {
        return res(ctx.status(401), ctx.text(unAuthorizedMesage))
    })
]