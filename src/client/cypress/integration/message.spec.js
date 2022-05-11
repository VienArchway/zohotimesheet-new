import { messageApiHandlers } from '../../test/api-mocks/show-message-api-handler.js'

context('Message page', () => {
    beforeEach(() => {
        cy.login()
        cy.visit('/message')
    })

    it('index infor', () => {
        cy.url()
            .should('eq', 'http://localhost:3000/message')

        cy.findByText(/Show message component/i).should('exist')
    })
    
    it('get default message api', () => {
        cy.window().then(window => {
            const { worker, rest, stop } = window.msw
            worker.use(messageApiHandlers[0])
            cy.findByText(/default: default message/i).should('exist')

            stop
        })
        
    })

    it('get param message api', () => {
        cy.window().then(window => {
            const { worker, rest, stop } = window.msw
            worker.use(messageApiHandlers[1])
            cy.findByText(/with msg: with custom test message/i).should('exist')

            stop
        })

    })

    it('can navigate to home', () => {
        cy.visit('http://localhost:3000/message')

        cy.get('[data-cy="link-index"]').should('be.visible').click()
        cy.location('pathname').should('eq', '/')

        cy.contains('main h1', 'Zoho index page').should('be.visible')
    })
})