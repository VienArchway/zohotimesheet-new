import { zohoTokenApiHandlers } from '../../test/api-mocks/zoho-token-api-handler.js'

context('Index page', () => {
    beforeEach(() => {
        cy.login()
        cy.visit('/')
        // cy.window().should('have.property', 'appReady', true)
    })

    it('index infor', () => {
        cy.url()
            .should('eq', 'http://localhost:3000/')

        cy.findByText(/authorized/i).should('exist')
        cy.findByText(/hello/i).should('exist')
    })
    
    it('get status zoho token api', () => {
        cy.window().then(window => {
            const { worker, rest, stop } = window.msw
            worker.use(zohoTokenApiHandlers[0])
            cy.findByText(/authorized: success/i).should('exist')

            stop
        })
        
    })

    // it('get param message api', () => {
    //     cy.window().then(window => {
    //         const { worker, rest, stop } = window.msw
    //         worker.use(messageApiHandlers[1])
    //         cy.findByText(/with msg: with custom test message/i).should('exist')
    //
    //         stop
    //     })
    //
    // })

    it('can navigate to about', () => {
        cy.visit('http://localhost:3000')

        cy.get('[data-cy="link-about"]').should('be.visible').click()
        cy.location('pathname').should('eq', '/about')
        cy.contains('main p', 'About').should('be.visible')

        cy.get('[data-cy="link-index"]').click()
        cy.location('pathname').should('eq', '/')
        cy.contains('main h1', 'Zoho index page').should('be.visible')
    })
})